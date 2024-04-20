using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActionCommandGame.Model;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Extensions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Settings;
using ActionCommandGame.Ui.ConsoleApp.ConsoleWriters;

namespace ActionCommandGame.Ui.ConsoleApp
{
    public class Game
    {
        private readonly AppSettings _settings;
        private readonly IGameService _gameService;
        private readonly IPlayerService _playerService;
        private readonly IItemService _itemService;
        private readonly IPlayerItemService _playerItemService;

        public Game(
            AppSettings settings,
            IGameService gameService,
            IPlayerService playerService,
            IItemService itemService,
            IPlayerItemService playerItemService)
        {
            _settings = settings;
            _gameService = gameService;
            _playerService = playerService;
            _itemService = itemService;
            _playerItemService = playerItemService;
        }

        public void Start()
        {
            Console.OutputEncoding = Encoding.UTF8;
            ConsoleBlockWriter.Write(_settings.GameName, 4 , ConsoleColor.Blue);
            ConsoleWriter.WriteText($"Play your game. Try typing \"help\" or \"{_settings.ActionCommand}\"", ConsoleColor.Yellow);

            //Get the player from somewhere
            var currentPlayerId = 1;

            while (true)
            {
                ConsoleWriter.WriteText($"{_settings.CommandPromptText} ", ConsoleColor.DarkGray, false);

                string command = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(command))
                {
                    continue;
                }

                if (CheckCommand(command, new[] { "exit", "quit", "stop" }))
                {
                    break;
                }

                if (CheckCommand(command, new[] { _settings.ActionCommand }))
                {
                    PerformAction(currentPlayerId);

                    ShowStats(currentPlayerId);
                }

                if (CheckCommand(command, new[] { "shop", "store" }))
                {
                    ShowShop();
                }

                if (CheckCommand(command, new[] { "buy", "purchase", "get" }))
                {
                    var itemId = GetIdParameterFromCommand(command);

                    if (!itemId.HasValue)
                    {
                        ConsoleWriter.WriteText("I have no idea what you mean. I have tagged every item with a number. Please give me that number.", ConsoleColor.Red);
                        continue;
                    }

                    Buy(currentPlayerId, itemId.Value);
                }

                if (CheckCommand(command, new[] { "bal", "balance", "money", "xp", "level", "statistics", "stats", "stat", "info" }))
                {
                    ShowStats(currentPlayerId);
                }

                if (CheckCommand(command, new[] { "leaderboard", "lead", "top", "rank", "ranking" }))
                {
                    var players = _playerService.Find().OrderByDescending(p => p.Experience).ToList();
                    ShowLeaderboard(players, currentPlayerId);
                }

                if (CheckCommand(command, new[] { "inventory", "inv", "bag", "backpack" }))
                {
                    var inventory = _playerItemService.Find(currentPlayerId);
                    ShowInventory(inventory);
                }

                if (CheckCommand(command, new[] { "?", "help", "h", "commands" }))
                {
                    ShowHelp();
                }
            }

            ConsoleWriter.WriteText("Thank you for playing.", ConsoleColor.Yellow);
            Console.ReadLine();
        }

        private static void ShowItem(Item item)
        {
            ConsoleWriter.WriteText($"\t[{item.Id}] {item.Name} €{item.Price}", ConsoleColor.White);
            if (!string.IsNullOrWhiteSpace(item.Description))
            {
                ConsoleWriter.WriteText($"\t\t{item.Description}");
            }
            if (item.Fuel > 0)
            {
                ConsoleWriter.WriteText("\t\tFuel: ", ConsoleColor.White, false);
                ConsoleWriter.WriteText($"{item.Fuel}");
            }
            if (item.Attack > 0)
            {
                ConsoleWriter.WriteText("\t\tAttack: ", ConsoleColor.White, false);
                ConsoleWriter.WriteText($"{item.Attack}");
            }
            if (item.Defense > 0)
            {
                ConsoleWriter.WriteText("\t\tDefense: ", ConsoleColor.White, false);
                ConsoleWriter.WriteText($"{item.Defense}");
            }
            if (item.ActionCooldownSeconds > 0)
            {
                ConsoleWriter.WriteText("\t\tCooldown seconds: ", ConsoleColor.White, false);
                ConsoleWriter.WriteText($"{item.ActionCooldownSeconds}"); 
            }
        }

        private static void ShowPlayerItem(PlayerItem playerItem)
        {
            ConsoleWriter.WriteText($"\t{playerItem.Item.Name}", ConsoleColor.White);
            if (!string.IsNullOrWhiteSpace(playerItem.Item.Description))
            {
                ConsoleWriter.WriteText($"\t\t{playerItem.Item.Description}");
            }
            if (playerItem.Item.Fuel > 0)
            {
                ConsoleWriter.WriteText($"\t\tFuel: {playerItem.RemainingFuel}/{playerItem.Item.Fuel}");
            }
            if (playerItem.Item.Attack > 0)
            {
                ConsoleWriter.WriteText($"\t\tAttack: {playerItem.RemainingAttack}/{playerItem.Item.Attack}");
            }
            if (playerItem.Item.Defense > 0)
            {
                ConsoleWriter.WriteText($"\t\tDefense: {playerItem.RemainingDefense}/{playerItem.Item.Defense}");
            }
            if (playerItem.Item.ActionCooldownSeconds > 0)
            {
                ConsoleWriter.WriteText($"\t\tCooldown seconds: {playerItem.Item.ActionCooldownSeconds}");
            }
        }

        private static bool CheckCommand(string command, IList<string> matchingCommands)
        {
            return matchingCommands.Any(c => command.ToLower().StartsWith(c.ToLower()));
        }

       
        public void ShowStats(int playerId)
        {
            var player = _playerService.Get(playerId);

            //Check food consumption
            if (player.CurrentFuelPlayerItem != null)
            {
                ConsoleWriter.WriteText($"[{player.CurrentFuelPlayerItem.Item.Name}] ", ConsoleColor.Yellow, false);
                ConsoleWriter.WriteText($"{player.CurrentFuelPlayerItem.RemainingFuel}/{player.CurrentFuelPlayerItem.Item.Fuel}  ", null, false);
            }
            else
            {
                ConsoleWriter.WriteText("[Food] ", ConsoleColor.Red, false);
                ConsoleWriter.WriteText("nothing ", null, false);
            }

            //Check attack consumption
            if (player.CurrentAttackPlayerItem != null)
            {
                ConsoleWriter.WriteText($"[{player.CurrentAttackPlayerItem.Item.Name}] ", ConsoleColor.Yellow, false);
                ConsoleWriter.WriteText($"{player.CurrentAttackPlayerItem.RemainingAttack}/{player.CurrentAttackPlayerItem.Item.Attack}  ", null, false);
            }
            else
            {
                ConsoleWriter.WriteText("[Attack] ", ConsoleColor.Red, false);
                ConsoleWriter.WriteText("nothing ", null, false);
            }

            //Check defense consumption
            if (player.CurrentDefensePlayerItem != null)
            {
                ConsoleWriter.WriteText($"[{player.CurrentDefensePlayerItem.Item.Name}] ", ConsoleColor.Yellow, false);
                ConsoleWriter.WriteText($"{player.CurrentDefensePlayerItem.RemainingDefense}/{player.CurrentDefensePlayerItem.Item.Defense}  ", null, false);
            }
            else
            {
                ConsoleWriter.WriteText("[Defense] ", ConsoleColor.Red, false);
                ConsoleWriter.WriteText("nothing ", null, false);
            }

            ConsoleWriter.WriteText("[Money] ", ConsoleColor.Yellow, false);
            ConsoleWriter.WriteText($"€{player.Money}  ", null, false);
            ConsoleWriter.WriteText("[Level] ", ConsoleColor.Yellow, false);
            ConsoleWriter.WriteText($"{player.GetLevel()} ({player.Experience}/{player.GetExperienceForNextLevel()})  ", null, false);

            ConsoleWriter.WriteText();
            ConsoleWriter.WriteText();
        }

        private void ShowLeaderboard(IList<Player> players, int currentPlayerId)
        {
            foreach (var player in players)
            {
                var text = $"\tLevel {player.GetLevel()} {player.Name} ({player.Experience})";
                if (player.Id == currentPlayerId)
                {
                    ConsoleWriter.WriteText(text, ConsoleColor.Yellow);
                }
                else
                {
                    ConsoleWriter.WriteText(text);
                }
            }
        }

        private void ShowInventory(IList<PlayerItem> playerItems)
        {
            foreach (var playerItem in playerItems)
            {
                ShowPlayerItem(playerItem);
            }
        }

        private void ShowHelp()
        {
            ConsoleWriter.WriteText($"\t{_settings.ActionCommand}: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("Do something");

            ConsoleWriter.WriteText($"\tshop: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("See the shop items");

            ConsoleWriter.WriteText($"\tbuy 1: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("Buy item number 1 from the shop");

            ConsoleWriter.WriteText($"\tinventory: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("Shows your inventory");

            ConsoleWriter.WriteText($"\tstats: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("See your statistics");

            ConsoleWriter.WriteText($"\tleaderboard: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("See the leaderboard");

            ConsoleWriter.WriteText($"\tquit: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("Quit the game");

            ConsoleWriter.WriteText($"\thelp: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("Well, this one is self explanatory, isn't it? Because you just used it?");

        }

        private void ShowShop()
        {
            ConsoleWriter.WriteText("Available Shop Items", ConsoleColor.Green);
            var shopItems = _itemService.Find();
            foreach (var item in shopItems)
            {
                ShowItem(item);
            }
            ConsoleWriter.WriteText();
        }

        private void PerformAction(int playerId)
        {
            var result = _gameService.PerformAction(playerId);

            if (result.IsSuccess)
            {
                var player = result.Data.Player;
                var positiveGameEvent = result.Data.PositiveGameEvent;
                var negativeGameEvent = result.Data.NegativeGameEvent;

                if (player.LastActionExecutedDateTime != null)
                {
                    if (positiveGameEvent != null)
                    {
                        ConsoleWriter.WriteText($"{string.Format(_settings.ActionText, player.Name)} ",
                            ConsoleColor.Green, false);
                        ConsoleWriter.WriteText(positiveGameEvent.Name, ConsoleColor.White);
                        if (!string.IsNullOrWhiteSpace(positiveGameEvent.Description))
                        {
                            ConsoleWriter.WriteText(positiveGameEvent.Description);
                        }
                        if (positiveGameEvent.Money > 0)
                        {
                            ConsoleWriter.WriteText($"€{positiveGameEvent.Money}", ConsoleColor.Yellow, false);
                            ConsoleWriter.WriteText(" has been added to your account.");
                        }
                    }

                    if (negativeGameEvent != null)
                    {
                        ConsoleWriter.WriteText(negativeGameEvent.Name, ConsoleColor.Blue);
                        if (!string.IsNullOrWhiteSpace(negativeGameEvent.Description))
                        {
                            ConsoleWriter.WriteText(negativeGameEvent.Description);
                        }
                        ConsoleWriter.WriteMessages(result.Data.NegativeGameEventMessages);
                    }

                    ConsoleWriter.WriteMessages(result.Messages);
                }
                else
                {
                    ConsoleWriter.WriteText("Last action execution time is null.", ConsoleColor.Red);
                }
            }
            else
            {
                ConsoleWriter.WriteMessages(result.Messages);
            }

            ConsoleWriter.WriteText();
        }


        private void Buy(int playerId, int itemId)
        {
            var result = _gameService.Buy(playerId, itemId);

            if (result.IsSuccess)
            {
                ConsoleWriter.WriteText($"You bought {result.Data.Item.Name} for €{result.Data.Item.Price}");
                ConsoleWriter.WriteText($"Thank you for shopping. Your current balance is €{result.Data.Player.Money}.");

                //Check if there are info and warning messages
                var nonErrorMessages =
                    result.Messages.Where(m => m.MessagePriority == MessagePriority.Error).ToList();
                ConsoleWriter.WriteMessages(nonErrorMessages);
            }
            else
            {
                var errorMessages = result.Messages.Where(m => m.MessagePriority == MessagePriority.Error)
                    .ToList();
                ConsoleWriter.WriteMessages(errorMessages);
            }

            Console.WriteLine();
        }

        private int? GetIdParameterFromCommand(string command)
        {
            var commandParts = command.Split(" ");
            if (commandParts.Length == 1)
            {
                return null;
            }
            var idPart = commandParts[1];

            int.TryParse(idPart, out var itemId);
            
            return itemId;
        }
    }
}
