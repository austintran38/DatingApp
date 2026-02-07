using System;
using API.Entities;
//an class that responsible for issuing tokens

namespace API.Interfaces;

public interface ITokenService
{
    string  CreateToken(AppUser user); //create token from user
}
