using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Repository.Core;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Helpers;
using ActionCommandGame.Services.Mappings;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class NegativeGameEventService: INegativeGameEventService
    {
        private readonly ActionButtonGameDbContext _database;

        public NegativeGameEventService(ActionButtonGameDbContext database)
        {
            _database = database;
        }

        public async Task<NegativeGameEventResult?> Get(int id)
        {
            return await _database.NegativeGameEvents
                .Where(p => p.Id == id)
                .MapToResults()
                .FirstOrDefaultAsync();
        }
        public async Task<NegativeGameEvent> GetRandomNegativeGameEvent()
        {
            var gameEvents = await _database.NegativeGameEvents.ToListAsync();
            return GameEventHelper.GetRandomNegativeGameEvent(gameEvents);
        }

        public async Task<IList<NegativeGameEventResult>> Find()
        {
            return await _database.NegativeGameEvents
                .MapToResults()
                .ToListAsync();
        }

        public async Task<NegativeGameEventResult?> Create(NegativeGameEventRequest request)
        {
            var negativeEvent = new NegativeGameEvent()
            {
                Name = request.Name,
                Description = request.Description,
                DefenseWithGearDescription = request.DefenseWithGearDescription,
                DefenseWithoutGearDescription  = request.DefenseWithoutGearDescription,
                DefenseLoss = request.DefenseLoss,
                Probability = request.Probability
            };

            _database.NegativeGameEvents.Add(negativeEvent);
            await _database.SaveChangesAsync();

            return await Get(negativeEvent.Id);
        }

        public async Task<NegativeGameEventResult?> Update(int id, NegativeGameEventRequest request)
        {
            var negativeEvent = await _database.NegativeGameEvents.FirstOrDefaultAsync(p => p.Id == id);

            if (negativeEvent is null)
            {
                return null;
            }

            negativeEvent.Name = request.Name;
            negativeEvent.Description = request.Description;
            negativeEvent.DefenseWithGearDescription = request.DefenseWithGearDescription;
            negativeEvent.DefenseWithoutGearDescription = request.DefenseWithoutGearDescription;
            negativeEvent.DefenseLoss = request.DefenseLoss;
            negativeEvent.Probability = request.Probability;

            await _database.SaveChangesAsync();

            return await Get(id);
        }

        public async Task<bool> Delete(int id)
        {
            var negativeEvent = await _database.NegativeGameEvents.FindAsync(id);

            if (negativeEvent == null)
            {
                return false;
            }

            _database.NegativeGameEvents.Remove(negativeEvent);
            await _database.SaveChangesAsync();
            return true;
        }
    }
}
