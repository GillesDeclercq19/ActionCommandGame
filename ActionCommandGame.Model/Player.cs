using System;
using System.Collections.Generic;
using ActionCommandGame.Model.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace ActionCommandGame.Model
{
    public class Player: IIdentifiable
    {
        public Player()
        {
            Inventory = new List<PlayerItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Zeni { get; set; }
        public int Experience { get; set; }
        public DateTime? LastActionExecutedDateTime { get; set; }
        public int? CurrentKiPlayerItemId { get; set; }
        public PlayerItem CurrentKiPlayerItem { get; set; }
        public int? CurrentAttackPlayerItemId { get; set; }
        public PlayerItem CurrentAttackPlayerItem { get; set; }
        public int? CurrentDefensePlayerItemId { get; set; }
        public PlayerItem CurrentDefensePlayerItem { get; set; }
        public IList<PlayerItem> Inventory { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        
    }
}
