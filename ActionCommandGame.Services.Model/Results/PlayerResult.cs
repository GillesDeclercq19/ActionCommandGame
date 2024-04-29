using System;
using System.Collections.Generic;
using ActionCommandGame.Services.Model.Results.DTO;

namespace ActionCommandGame.Services.Model.Results
{
    public class PlayerResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Money { get; set; }
        public int Experience { get; set; }
        public DateTime? LastActionExecutedDateTime { get; set; }
        public int? CurrentFuelPlayerItemId { get; set; }
        public int? CurrentAttackPlayerItemId { get; set; }
        public int? CurrentDefensePlayerItemId { get; set; }
        public IList<ItemDto> Inventory { get; set; }
    }

}