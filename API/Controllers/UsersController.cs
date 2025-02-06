using System;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;


public class UserController(DataContext context) : BaseApiController
{   
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers (){

        var users = await context.users.ToListAsync();

        return users;

    }

    [Authorize]
     [HttpGet("{id:int}")]
    public ActionResult<AppUser> GetUsers (int id){

        var user = context.users.Find(id);

        if (user == null) return NotFound();

        return user;

    }
}