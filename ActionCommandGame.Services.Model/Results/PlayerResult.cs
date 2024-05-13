using System;

namespace ActionCommandGame.Services.Model.Results
{
    public class PlayerResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Zeni { get; set; }
        public int Experience { get; set; }
        public DateTime? LastActionExecutedDateTime { get; set; }
        public int? CurrentFuelPlayerItemId { get; set; }
        public int? CurrentAttackPlayerItemId { get; set; }
        public int? CurrentDefensePlayerItemId { get; set; }
    }

}