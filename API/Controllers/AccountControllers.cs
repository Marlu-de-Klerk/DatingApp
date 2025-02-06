using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")] //acount/register
    
    public async Task<ActionResult<UserDto>> Register(RegisterDtos registerDtos){

        if (await UserExists(registerDtos.Username)) return BadRequest("Username is taken");

        using var hmac = new HMACSHA512();

        var user = new AppUser{
            UserName = registerDtos.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDtos.Password)),
            PasswordSalt = hmac.Key
        };

        context.users.Add(user);
        await context.SaveChangesAsync();

        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login (LoginDto loginDto){
        var user = await context.users.FirstOrDefaultAsync (x => 
            x.UserName == loginDto.Username.ToLower());

        if (user == null) return Unauthorized("Invalid username");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < ComputeHash.Length; i++)
        {
            if (ComputeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
        }

        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };    }

    private async Task<bool> UserExists (string username){
        return await context.users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
    }

}
