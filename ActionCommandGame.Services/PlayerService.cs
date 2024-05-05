﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Repository.Core;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Mappings;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class PlayerService: IPlayerService
    {
        private readonly ActionButtonGameDbContext _database;

        public PlayerService(ActionButtonGameDbContext database)
        {
            _database = database;
        }
        public async Task<PlayerResult?> Get(int id)
        {
            return await _database.Players
                .Where(p => p.Id == id)
                .MapToResults()
                .FirstOrDefaultAsync();
        }

        public async Task<IList<PlayerResult>> Find()
        {
            return await _database.Players
                .MapToResults()
                .ToListAsync();
        }

        public async Task<PlayerResult?> Create(PlayerRequest request)
        {
            var player = new Player()
            {
                Name = request.Name
            };

            _database.Players.Add(player);
            await _database.SaveChangesAsync();

            return await Get(player.Id);
        }

        public async Task<PlayerResult?> Update(int id, PlayerRequest request)
        {
            var player = await _database.Players.FirstOrDefaultAsync(p => p.Id == id);

            if (player is null)
            {
                return null;
            }

            player.Name = request.Name;
           

            await _database.SaveChangesAsync();

            return await Get(id);
        }

        public async Task<bool> Delete(int id)
        {
            var player = await _database.Players.FindAsync(id);

            if (player == null)
            {
                return false;
            }

            _database.Players.Remove(player);
            await _database.SaveChangesAsync();
            return true;
        }
    }
}
