using System.ComponentModel.DataAnnotations;

namespace ActionCommandGame.UI.Mvc.Models
{
    public class RegisterModel
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password))]
        public required string ConfirmPassword { get; set; }
    }
}
