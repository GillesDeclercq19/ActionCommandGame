using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Repository.Core;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Extensions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class PlayerItemService : IPlayerItemService
    {
        private readonly ActionButtonGameDbContext _database;

        public PlayerItemService(ActionButtonGameDbContext database)
        {
            _database = database;
        }

        public async Task<PlayerItemResult?> Get(int id)
        {
            return await _database.PlayerItems
                .Where(p => p.Id == id)
                .Select(p => new PlayerItemResult()
                {
                    Id = p.Id,
                    PlayerId = p.PlayerId,
                    ItemId = p.ItemId,
                    RemainingAttack = p.RemainingAttack,
                    RemainingDefense = p.RemainingDefense,
                    RemainingFuel = p.RemainingFuel

                })
                .FirstOrDefaultAsync();
        }

        public async Task<IList<PlayerItemResult>> Find(int? playerId = null)
        {
            IQueryable<PlayerItem> query = _database.PlayerItems.Include(p => p.Player)
                .Include(p => p.Item);

            if (playerId.HasValue)
            {
                query = query
                    .Where(p => p.PlayerId == playerId.Value);
            }

            var playerItems = await query.ToListAsync();

            // Mapping PlayerItem to PlayerItemResult
            var results = playerItems.Select(p => new PlayerItemResult
            {
                Id = p.Id,
                PlayerId = p.PlayerId,
                ItemId = p.ItemId,
                RemainingFuel = p.RemainingFuel,
                RemainingAttack = p.RemainingAttack,
                RemainingDefense = p.RemainingDefense
            }).ToList();

            return results;
        }

        public async Task<ServiceResult<PlayerItemResult>> Create(int playerId, int itemId)
        {
            var player = await _database.Players.SingleOrDefaultAsync(p => p.Id == playerId);
            if (player == null)
            {
                return new ServiceResult<PlayerItemResult>().PlayerNotFound();
            }

            var item = await _database.Items.SingleOrDefaultAsync(i => i.Id == itemId);
            if (item == null)
            {
                return new ServiceResult<PlayerItemResult>().ItemNotFound();
            }

            var playerItem = new PlayerItem
            {
                ItemId = itemId,
                Item = item,
                PlayerId = playerId,
                Player = player
            };
            _database.PlayerItems.Add(playerItem);
            player.Inventory.Add(playerItem);
            item.PlayerItems.Add(playerItem);

            // Auto Equip the item you bought
            if (item.Fuel > 0)
            {
                playerItem.RemainingFuel = item.Fuel;
                player.CurrentFuelPlayerItemId = playerItem.Id;
                player.CurrentFuelPlayerItem = playerItem;
            }
            if (item.Attack > 0)
            {
                playerItem.RemainingAttack = item.Attack;
                player.CurrentAttackPlayerItemId = playerItem.Id;
                player.CurrentAttackPlayerItem = playerItem;
            }
            if (item.Defense > 0)
            {
                playerItem.RemainingDefense = item.Defense;
                player.CurrentDefensePlayerItemId = playerItem.Id;
                player.CurrentDefensePlayerItem = playerItem;
            }

            await _database.SaveChangesAsync();

            var playerItemResult = new PlayerItemResult
            {
                Id = playerItem.Id,
                PlayerId = playerItem.PlayerId,
                ItemId = playerItem.ItemId,
                RemainingFuel = playerItem.RemainingFuel,
                RemainingAttack = playerItem.RemainingAttack,
                RemainingDefense = playerItem.RemainingDefense
            };

            return new ServiceResult<PlayerItemResult>(playerItemResult);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var playerItem = await _database.PlayerItems
                .Include(pi => pi.Player)
                .Include(pi => pi.Item)
                .SingleOrDefaultAsync(pi => pi.Id == id);

            if (playerItem == null)
            {
                return new ServiceResult().NotFound();
            }
            
            var player = playerItem.Player;
            player.Inventory.Remove(playerItem);
            
            var item = playerItem.Item;
            item.PlayerItems.Remove(playerItem);

            //Clear up equipment
            if (player.CurrentFuelPlayerItemId == id)
            {
                player.CurrentFuelPlayerItemId = null;
                player.CurrentFuelPlayerItem = null;
            }
            if (player.CurrentAttackPlayerItemId == id)
            {
                player.CurrentAttackPlayerItemId = null;
                player.CurrentAttackPlayerItem = null;
            }
            if (player.CurrentDefensePlayerItemId == id)
            {
                player.CurrentDefensePlayerItemId = null;
                player.CurrentDefensePlayerItem = null;
            }

            _database.PlayerItems.Remove(playerItem);

            //Save Changes
            await _database.SaveChangesAsync();

            return new ServiceResult();
        }
        
    }
}
