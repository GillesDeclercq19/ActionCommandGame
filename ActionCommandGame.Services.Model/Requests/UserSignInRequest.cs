﻿using System.ComponentModel.DataAnnotations;

namespace ActionCommandGame.Services.Model.Requests
{
    public class UserSignInRequest
    {
        [Required]
        [EmailAddress]
        public required string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public required string Password { get; set; }
    }
}