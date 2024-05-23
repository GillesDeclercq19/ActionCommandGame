using System;
using System.ComponentModel.DataAnnotations;

namespace ActionCommandGame.Services.Model.Requests
{
    public class PlayerRequest
    {
        [Required]
        [Display(Name = "Name")]
        public required string Name { get; set; }
        public int Zeni { get; set; }
        public int Experience { get; set; }
        public int playerId { get; set; }
        public string UserId { get; set; }
    }
}
