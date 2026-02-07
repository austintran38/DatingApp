using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(AppDbContext context, ITokenService tokenService) : BaseAPIController
{
    [HttpPost("register")] // api/account/register
    public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
    {
        if (await EmailExist(registerDTO.Email)) return BadRequest("Email Taken");//if email existed, return error

        var hmac = new HMACSHA512(); //create a a randomly generated key
        var user = new AppUser //create a new app user with the following instances
        {
            DisplayName = registerDTO.DisplayName,
            Email = registerDTO.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
            PasswordSalt = hmac.Key
        };
        context.Users.Add(user); //add user to the database
        await context.SaveChangesAsync(); //save any of the tracked changes to the database
        return user.ToDTO(tokenService);   
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
    {
        //query the database to find the user with exactly the email provided
        var user = await context.Users.SingleOrDefaultAsync(x => x.Email == loginDTO.Email);
        //if return null (email not in database), return error
        if(user == null) return Unauthorized("Invalid Email Address");
        //if found user email, start check password
        using var hmac = new HMACSHA512(user.PasswordSalt); //perform hasing using the recorded user password salt

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

        for (var i = 0; i < computedHash.Length; i++) //loop over the recorded password to compare every bytes
        {
            if(computedHash[i] != user.PasswordHash[i]) return Unauthorized ("Invalid Password");
        }
        return user.ToDTO(tokenService);
    }

    //private method to check for duplicate email
    private async Task<bool> EmailExist(string email)
    {
        return await context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }
}
