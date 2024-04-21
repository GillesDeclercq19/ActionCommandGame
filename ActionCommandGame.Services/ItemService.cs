using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Repository.Core;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class ItemService: IItemService
    {
        private readonly ActionButtonGameDbContext _database;

        public ItemService(ActionButtonGameDbContext database)
        {
            _database = database;
        }

        public async Task<ItemResult?> Get(int id)
        {
            return await _database.Items
                .Where(p => p.Id == id)
                .Select(p => new ItemResult()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Fuel = p.Fuel,
                    Attack = p.Attack,
                    Defense = p.Defense,   
                    ActionCooldownSeconds = p.ActionCooldownSeconds,
                    PlayerItems = p.PlayerItems.Select(item => new ItemDto
                    {
                        Id = item.Id,
                        Name = item.Item.Name
                    }).ToList(),
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IList<ItemResult>> Find()
        {
            return await _database.Items
                .Select(p => new ItemResult()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Fuel = p.Fuel,
                    Attack = p.Attack,
                    Defense = p.Defense,
                    ActionCooldownSeconds = p.ActionCooldownSeconds,
                    PlayerItems = p.PlayerItems.Select(item => new ItemDto
                    {
                        Id = item.Id,
                        Name = item.Item.Name
                    }).ToList(),
                })
                .ToListAsync();
        }

        public async Task<ItemResult?> Create(ItemRequest request)
        {
            var item = new Item()
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Fuel = request.Fuel,
                Attack = request.Attack,
                Defense = request.Defense,
                ActionCooldownSeconds = request.ActionCooldownSeconds
            };

            _database.Items.Add(item);
            await _database.SaveChangesAsync();

            return await Get(item.Id);
        }

        public async Task<ItemResult?> Update(int id, ItemRequest request)
        {
            var item = await _database.Items.FirstOrDefaultAsync(p => p.Id == id);

            if (item is null)
            {
                return null;
            }

            item.Name = request.Name;
            item.Description = request.Description;
            item.Price = request.Price;
            item.Fuel = request.Fuel;
            item.Attack = request.Attack;
            item.Defense = request.Defense;
            item.ActionCooldownSeconds = request.ActionCooldownSeconds;

            await _database.SaveChangesAsync();

            return await Get(id);
        }

        public async Task<bool> Delete(int id)
        {
            var item = await _database.Items.FindAsync(id);

            if (item == null)
            {
                return false;
            }

            _database.Items.Remove(item);
            await _database.SaveChangesAsync();
            return true;
        }
    }
}
