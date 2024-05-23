using System.ComponentModel.DataAnnotations;

namespace ActionCommandGame.Services.Model.Requests
{
    public class UserSignInRequest
    {
        [Required]
        public required string Username { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public required string Password { get; set; }
    }
}
