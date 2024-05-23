using System;
using System.ComponentModel.DataAnnotations;

namespace ActionCommandGame.Services.Model.Requests
{
    public class PlayerRequest
    {
        public string Name { get; set; }
        public int Zeni { get; set; }
        public int Experience { get; set; }
        public String UserId { get; set; }
    }
}
