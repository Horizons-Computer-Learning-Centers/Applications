﻿namespace Horizons.Core.Auth.Dtos
{
    public class RegistrationRequest
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string Password { get; set; }
        public List<string>? Roles { get; set; }
    }
}
