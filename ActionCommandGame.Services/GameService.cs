using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Repository.Core;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Extensions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;
using ActionCommandGame.Settings;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class GameService : IGameService
    {
        private readonly AppSettings _appSettings;
        private readonly ActionButtonGameDbContext _database;
        private readonly IPlayerService _playerService;
        private readonly IPositiveGameEventService _positiveGameEventService;
        private readonly INegativeGameEventService _negativeGameEventService;
        private readonly IItemService _itemService;
        private readonly IPlayerItemService _playerItemService;

        public GameService(
            AppSettings appSettings,
            ActionButtonGameDbContext database,
            IPlayerService playerService,
            IPositiveGameEventService positiveGameEventService,
            INegativeGameEventService negativeGameEventService,
            IItemService itemService,
            IPlayerItemService playerItemService)
        {
            _appSettings = appSettings;
            _database = database;
            _playerService = playerService;
            _positiveGameEventService = positiveGameEventService;
            _negativeGameEventService = negativeGameEventService;
            _itemService = itemService;
            _playerItemService = playerItemService;
        }

        public async Task<ServiceResult<GameResult>> PerformAction(int playerId)
        {
            //Check Cooldown
            var player = await _database.Players
                .Include(p => p.CurrentKiPlayerItem.Item)
                .Include(p => p.CurrentAttackPlayerItem.Item)
                .Include(p => p.CurrentDefensePlayerItem.Item)
                .FirstOrDefaultAsync(p => p.Id == playerId);
            
            if (player == null)
            {
                return new ServiceResult<GameResult>().PlayerNotFound();
            }

            var elapsedSeconds = DateTime.UtcNow.Subtract(player.LastActionExecutedDateTime).TotalSeconds;
            var cooldownSeconds = _appSettings.DefaultCooldown;
            if (player.CurrentKiPlayerItem != null)
            {
                cooldownSeconds = player.CurrentKiPlayerItem.Item.ActionCooldownSeconds;
            }

            if (elapsedSeconds < cooldownSeconds)
            {
                var waitSeconds = Math.Ceiling(cooldownSeconds - elapsedSeconds);
                var waitText = $"You are still a bit tired. You have to wait another {waitSeconds} seconds.";
                return new ServiceResult<GameResult>
                {
                    Data = new GameResult { PlayerId = player.Id },
                    Messages = new List<ServiceMessage> { new ServiceMessage { Code = "Cooldown", Message = waitText } }
                };
            }

            var hasAttackItem = player.CurrentAttackPlayerItem != null;
            var positiveGameEvent = await _positiveGameEventService.GetRandomPositiveGameEvent(hasAttackItem);
            if (positiveGameEvent == null)
            {
                return new ServiceResult<GameResult>
                {
                    Messages = new List<ServiceMessage>
                        {
                        new ServiceMessage
                        {
                            Code = "Error",
                            Message = "Something went wrong getting the Positive Game Event.",
                            MessagePriority = MessagePriority.Error
                        }
                    }
                };
            }

            var negativeGameEvent = await _negativeGameEventService.GetRandomNegativeGameEvent();

            var oldLevel = player.GetLevel();

            player.Zeni += positiveGameEvent.Zeni;
            player.Experience += positiveGameEvent.Experience;

            var newLevel = player.GetLevel();

            var levelMessages = new List<ServiceMessage>();
            //Check if we leveled up
            if (oldLevel < newLevel)
            {
                levelMessages = [new ServiceMessage { Code = "LevelUp", Message = $"Congratulations, you arrived at level {newLevel}" }];
            }

            //Consume ki
            var kiMessages =  await ConsumeKi(player);

            var attackMessages = new List<ServiceMessage>();
            var defenseMessages = new List<ServiceMessage>();
            var eventMessages = new List<ServiceMessage>();
            if (negativeGameEvent != null)
            {
                //Check defense consumption
                if (player.CurrentDefensePlayerItem != null)
                {
                    eventMessages.Add(new ServiceMessage { Code = "DefenseWithGear", Message = negativeGameEvent.DefenseWithGearDescription });
                    defenseMessages.AddRange(await ConsumeDefense(player, negativeGameEvent.DefenseLoss));
                }
                else
                {
                    eventMessages.Add(new ServiceMessage { Code = "DefenseWithoutGear", Message = negativeGameEvent.DefenseWithoutGearDescription });

                    //If we have no defense item, consume the defense loss from Health and Attack
                    defenseMessages.AddRange(await ConsumeKi(player, negativeGameEvent.DefenseLoss));
                    defenseMessages.AddRange(await ConsumeAttack(player, negativeGameEvent.DefenseLoss));
                }
            }
           
            else if (positiveGameEvent.Zeni > 500)
            {
                attackMessages.AddRange(await ConsumeAttack(player));
            }

            var warningMessages = GetWarningMessages(player);

            player.LastActionExecutedDateTime = DateTime.UtcNow;

            //Save Player
            await _database.SaveChangesAsync();

            var gameResult = new GameResult
            {
                PlayerId = player.Id,
                PositiveGameEvent = positiveGameEvent,
                NegativeGameEvent = negativeGameEvent,
                EventMessages = eventMessages
            };

            var serviceResult = new ServiceResult<GameResult>
            {
                Data = gameResult
            };

            //Add all the messages to the player
            serviceResult.WithMessages(levelMessages);
            serviceResult.WithMessages(warningMessages);
            serviceResult.WithMessages(kiMessages);
            serviceResult.WithMessages(attackMessages);
            serviceResult.WithMessages(defenseMessages);

            return serviceResult;
        }

        public async Task<ServiceResult<BuyResult>> Buy(int playerId, int itemId)
        {
            var player = await _database.Players.FirstOrDefaultAsync(p => p.Id == playerId);
            if (player == null)
            {
                return new ServiceResult<BuyResult>().PlayerNotFound();
            }

            var item = await _database.Items.FirstOrDefaultAsync(p => p.Id == itemId);
            if (item == null)
            {
                return new ServiceResult<BuyResult>().ItemNotFound();
            }

            if (item.Price > player.Zeni)
            {
                return new ServiceResult<BuyResult>().NotEnoughMoney();
            }

            await _playerItemService.Create(playerId, itemId);

            player.Zeni -= item.Price;

            //SaveChanges
            await _database.SaveChangesAsync();

            var buyResult = new BuyResult
            {
                PlayerId = player.Id,
                ItemId = item.Id,
                ItemName = item.Name,
                ItemDescription = item.Description,
            };
            return new ServiceResult<BuyResult> { Data = buyResult };
        }

        private async Task<IList<ServiceMessage>> ConsumeKi(Player player, int kiLoss = 1)
        {
            if (player.CurrentKiPlayerItem != null && player.CurrentKiPlayerItemId.HasValue)
            {
                player.CurrentKiPlayerItem.RemainingKi -= kiLoss;
                if (player.CurrentKiPlayerItem.RemainingKi <= 0)
                {
                    await _playerItemService.Delete(player.CurrentKiPlayerItemId.Value);

                    //Load a new Ki Item from inventory
                    var newKiItem = player.Inventory
                        .Where(pi => pi.Item.Ki > 0)
                        .MaxBy(pi => pi.Item.Ki);

                    if (newKiItem != null)
                    {
                        player.CurrentKiPlayerItem = newKiItem;
                        player.CurrentKiPlayerItemId = newKiItem.Id;
                        return new List<ServiceMessage>{new ServiceMessage
                        {
                            Code = "ReloadedKi",
                            Message = $"You are low on Ki and eat {newKiItem.Item.Name}. Yummy!"
                        }};
                    }

                    return new List<ServiceMessage>{new ServiceMessage
                    {
                        Code = "NoFood",
                        Message = "You are so hungry. You look into your bag and find ... nothing!",
                        MessagePriority = MessagePriority.Warning
                    }};
                }
            }

            return new List<ServiceMessage>();
        }

        private async Task<IList<ServiceMessage>> ConsumeAttack(Player player, int attackLoss = 1)
        {
            if (player.CurrentAttackPlayerItem != null && player.CurrentAttackPlayerItemId.HasValue)
            {
                var oldAttackItem = player.CurrentAttackPlayerItem;
                player.CurrentAttackPlayerItem.RemainingAttack -= attackLoss;
                if (player.CurrentAttackPlayerItem.RemainingAttack <= 0)
                {
                    await _playerItemService.Delete(player.CurrentAttackPlayerItemId.Value);

                    //Load a new Attack Item from inventory
                    var newAttackItem = player.Inventory
                        .Where(pi => pi.Item.Attack > 0)
                        .MaxBy(pi => pi.Item.Attack);
                    if (newAttackItem != null)
                    {
                        player.CurrentAttackPlayerItem = newAttackItem;
                        player.CurrentAttackPlayerItemId = newAttackItem.Id;
                        return new List<ServiceMessage>{new ServiceMessage
                        {
                            Code = "ReloadedAttack",
                            Message = $"You just broke {oldAttackItem.Item.Name}. No worries, you swiftly equip a new {newAttackItem.Item.Name}. Yeah!",

                        }};
                    }

                    return new List<ServiceMessage>{new ServiceMessage
                    {
                        Code = "NoAttack",
                        Message = $"You just broke {oldAttackItem.Item.Name}. This was your last tool. Idiot!",
                        MessagePriority = MessagePriority.Warning
                    }};
                }
            }
            else if (player.CurrentDefensePlayerItem != null) 
            {
                
                await ConsumeDefense(player);
            }

            return new List<ServiceMessage>();
        }

        private async Task<IList<ServiceMessage>> ConsumeDefense(Player player, int defenseLoss = 1)
        {
            if (player.CurrentDefensePlayerItem != null && player.CurrentDefensePlayerItemId.HasValue)
            {
                var oldDefenseItem = player.CurrentDefensePlayerItem;
                player.CurrentDefensePlayerItem.RemainingDefense -= defenseLoss;
                if (player.CurrentDefensePlayerItem.RemainingDefense <= 0)
                {
                    await _playerItemService.Delete(player.CurrentDefensePlayerItemId.Value);

                    //Load a new Defense Item from inventory
                    var newDefenseItem = player.Inventory
                        .Where(pi => pi.Item.Defense > 0)
                        .MaxBy(pi => pi.Item.Defense);
                    ;
                    if (newDefenseItem != null)
                    {
                        player.CurrentDefensePlayerItem = newDefenseItem;
                        player.CurrentDefensePlayerItemId = newDefenseItem.Id;

                        return new List<ServiceMessage>{new ServiceMessage
                        {
                            Code = "ReloadedDefense",
                            Message = $"Your {oldDefenseItem.Item.Name} is starting to tear from the cracks. No worries, you swiftly put on a newly made {newDefenseItem.Item.Name}."
                        }};
                    }

                    return new List<ServiceMessage>{new ServiceMessage
                    {
                        Code = "NoAttack",
                        Message = $"You just lost {oldDefenseItem.Item.Name}. You continue without protection. Did I just see something move?",
                        MessagePriority = MessagePriority.Warning
                    }};
                }
            }
            else
            {
                //If we don't have defensive gear, just consume more ki in stead.
                await ConsumeKi(player);
            }

            return new List<ServiceMessage>();
        }

        private IList<ServiceMessage> GetWarningMessages(Player player)
        {
            var serviceMessages = new List<ServiceMessage>();

            if (player.CurrentKiPlayerItem == null)
            {
                var infoText = "Training without ki is hard. You need a long time to recover. Consider buying food from the shop.";
                serviceMessages.Add(new ServiceMessage { Code = "NoFood", Message = infoText, MessagePriority = MessagePriority.Warning });
            }
            if (player.CurrentAttackPlayerItem == null)
            {
                var infoText = "Training without tools is hard. You can't keep on training like this! Consider buying tools from the shop.";
                serviceMessages.Add(new ServiceMessage { Code = "NoTools", Message = infoText, MessagePriority = MessagePriority.Warning });
            }
            if (player.CurrentDefensePlayerItem == null)
            {
                var infoText = "Training without gear is hard. You will take more damage! Consider buying gear from the shop.";
                serviceMessages.Add(new ServiceMessage { Code = "NoGear", Message = infoText, MessagePriority = MessagePriority.Warning });
            }

            return serviceMessages;
        }
    }
}
