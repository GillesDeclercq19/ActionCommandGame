using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionCommandGame.Services.Model.Requests
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public required string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public required string Password { get; set; }
    }
}
