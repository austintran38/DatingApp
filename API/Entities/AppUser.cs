using System;
/*
Entity class that store basic user information
*/
namespace API.Entities;

public class AppUser
{
    //create a new guid when new user id is created
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public required string DisplayName { get; set; } //display name cannot be null
    
    public required string Email { get; set; } //email cannot be null
}
