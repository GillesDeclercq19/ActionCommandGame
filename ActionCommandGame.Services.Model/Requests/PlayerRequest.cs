using System;
using System.ComponentModel.DataAnnotations;

namespace ActionCommandGame.Services.Model.Requests
{
    public class PlayerRequest
    {
        public int playerId { get; set; }
        public string Name { get; set; }
        public int Zeni { get; set; }
        public int Experience { get; set; }
        public string UserId { get; set; }
    }
}
