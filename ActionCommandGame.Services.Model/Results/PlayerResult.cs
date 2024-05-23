using System;
using System.Collections.Generic;

namespace ActionCommandGame.Services.Model.Results
{
    public class PlayerResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Zeni { get; set; }
        public int Experience { get; set; }
        public DateTime LastActionExecutedDateTime { get; set; }
        public int? CurrentKiPlayerItem { get; set; }
        public int? CurrentAttackPlayerItem { get; set; }
        public int? CurrentDefensePlayerItem { get; set; }
        public IList<int> Inventory { get; set; }

        public string UserId { get; set; }
    }

}