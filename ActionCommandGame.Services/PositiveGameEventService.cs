using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Repository.Core;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Extensions;
using ActionCommandGame.Services.Helpers;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class PositiveGameEventService: IPositiveGameEventService
    {
        private readonly ActionButtonGameDbContext _database;

        public PositiveGameEventService(ActionButtonGameDbContext database)
        {
            _database = database;
        }

        public async Task<PositiveGameEventResult?> Get(int id)
        {
            return await _database.PositiveGameEvents
                .Where(p => p.Id == id)
                .MapToResults()
                .FirstOrDefaultAsync();
        }


        public async Task<PositiveGameEvent> GetRandomPositiveGameEvent(bool hasAttackItem)
        {
            var query = _database.PositiveGameEvents.AsQueryable();

            //If we don't have an attack item, we can only get low-reward items.
            if (!hasAttackItem)
            {
                query = query.Where(p => p.Zeni < 50);
            }

            var gameEvents = await query.ToListAsync();

            return GameEventHelper.GetRandomPositiveGameEvent(gameEvents);
        }

        public async Task<IList<PositiveGameEventResult>> Find()
        {
            return await _database.PositiveGameEvents
                .MapToResults()
                .ToListAsync();
        }

        public async Task<PositiveGameEventResult?> Create(PositiveGameEventRequest request)
        {
            var positiveEvent = new PositiveGameEvent()
            {
                Name = request.Name,
                Description = request.Description,
                Zeni = request.Zeni,
                Experience = request.Experience,
                Probability = request.Probability
            };

            _database.PositiveGameEvents.Add(positiveEvent);
            await _database.SaveChangesAsync();

            return await Get(positiveEvent.Id);
        }

        public async Task<PositiveGameEventResult?> Update(int id, PositiveGameEventRequest request)
        {
            var positiveEvent = await _database.PositiveGameEvents.FirstOrDefaultAsync(p => p.Id == id);

            if (positiveEvent is null)
            {
                return null;
            }

            positiveEvent.Name = request.Name;
            positiveEvent.Description = request.Description;
            positiveEvent.Zeni = request.Zeni;
            positiveEvent.Experience = request.Experience;
            positiveEvent.Probability = request.Probability;

            await _database.SaveChangesAsync();

            return await Get(id);
        }

        public async Task<bool> Delete(int id)
        {
            var positiveEvent = await _database.PositiveGameEvents.FindAsync(id);

            if (positiveEvent == null)
            {
                return false;
            }

            _database.PositiveGameEvents.Remove(positiveEvent);
            await _database.SaveChangesAsync();
            return true;
        }
    }
}
