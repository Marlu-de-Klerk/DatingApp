using System;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace API;
[ApiController]
[Route("api/[controller]")]
public class UserController(DataContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers (){

        var users = await context.users.ToListAsync();

        return users;

    }

     [HttpGet("{id:int}")]
    public ActionResult<AppUser> GetUsers (int id){

        var user = context.users.Find(id);

        if (user == null) return NotFound();

        return user;

    }
}