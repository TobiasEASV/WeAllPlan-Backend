﻿namespace Application.DTO;

public class LoginUserDto
{
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public Boolean KeepMeLoggedIn { get; set; }
}