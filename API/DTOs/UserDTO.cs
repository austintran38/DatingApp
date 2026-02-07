using System;

namespace API.DTOs;

public class UserDTO
{
    public required string ID { get; set; }
    public required string Email { get; set; }
    public required string DisplayName { get; set; }
    public string? ImageURL { get; set; }
    public string Token { get; set; }
}
