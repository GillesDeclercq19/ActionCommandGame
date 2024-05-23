using System.ComponentModel.DataAnnotations;

namespace ActionCommandGame.Services.Model.Requests
{
    public class UserRegisterRequest
    {
        [Required]
        public required string Username { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public required string Password { get; set; }

        [Required]
        public required string Player { get; set; }
    }
}
