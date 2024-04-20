using ActionCommandGame.Model;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Repository.Extensions
{
    public static class RelationshipsExtensions
    {
        public static void ConfigureRelationships(this ModelBuilder builder)
        {
            builder.ConfigurePlayerItem();
            builder.ConfigurePlayer();
        }

        private static void ConfigurePlayerItem(this ModelBuilder builder)
        {
            builder.Entity<PlayerItem>()
                .HasOne(a => a.Item)
                .WithMany(u => u.PlayerItems)
                .HasForeignKey(a => a.ItemId);

            builder.Entity<PlayerItem>()
                .HasOne(a => a.Player)
                .WithMany(u => u.Inventory)
                .HasForeignKey(a => a.PlayerId);
        }

        private static void ConfigurePlayer(this ModelBuilder builder)
        {
            builder.Entity<Player>()
                .HasOne(a => a.CurrentFuelPlayerItem)
                .WithMany(u => u.FuelPlayers)
                .HasForeignKey(a => a.CurrentFuelPlayerItemId);

            builder.Entity<Player>()
                .HasOne(a => a.CurrentAttackPlayerItem)
                .WithMany(u => u.AttackPlayers)
                .HasForeignKey(a => a.CurrentAttackPlayerItemId);

            builder.Entity<Player>()
                .HasOne(a => a.CurrentDefensePlayerItem)
                .WithMany(u => u.DefensePlayers)
                .HasForeignKey(a => a.CurrentDefensePlayerItemId);
        } /*
        public static void AddDatabaseContent(this ModelBuilder builder)
        {
            GeneratePositiveGameEvents(builder);
            GenerateNegativeGameEvents(builder);
            GenerateAttackItems(builder);
            GenerateDefenseItems(builder);
            GenerateFoodItems(builder);
            GenerateDecorativeItems(builder);

            // God Mode Item
            builder.Entity<Item>().HasData(new Item
            {
                Id = 21,
                Name = "GOD MODE",
                Description = "This is almost how a GOD must feel.",
                Attack = 1000000,
                Defense = 1000000,
                Fuel = 1000000,
                ActionCooldownSeconds = 1,
                Price = 10000000
            });

            // Players
            builder.Entity<Player>().HasData(
                new Player { Id = 1, Name = "John Doe", Money = 100 },
                new Player { Id = 2, Name = "John Francks", Money = 100000, Experience = 2000 },
                new Player { Id = 3, Name = "Luc Doleman", Money = 500, Experience = 5 },
                new Player { Id = 4, Name = "Emilio Fratilleci", Money = 12345, Experience = 200 }
            );
        }

        private static void GeneratePositiveGameEvents(ModelBuilder builder)
        {
            builder.Entity<PositiveGameEvent>().HasData(
                new PositiveGameEvent { Id = 1, Name = "Nothing but boring rocks", Probability = 1000 },
                new PositiveGameEvent { Id = 2, Name = "The biggest Opal you ever saw.", Description = "It slips out of your hands and rolls inside a crack in the floor. It is out of reach.", Probability = 500 },
                new PositiveGameEvent { Id = 3, Name = "Sand, dirt and dust", Probability = 1000 },
                new PositiveGameEvent { Id = 4, Name = "A piece of empty paper", Description = "You hold it to the light and warm it up to reveal secret texts, but it remains empty.", Probability = 1000 },
                new PositiveGameEvent { Id = 5, Name = "A small water stream", Description = "The water flows around your feet and creates a dirty puddle.", Probability = 1000 },
                new PositiveGameEvent { Id = 6, Name = "Junk", Money = 1, Experience = 1, Probability = 2000 },
                new PositiveGameEvent { Id = 7, Name = "Murphy's idea bin", Money = 1, Experience = 1, Probability = 300 },
                new PositiveGameEvent { Id = 8, Name = "Donald's book of excuses", Money = 1, Experience = 1, Probability = 300 },
                new PositiveGameEvent { Id = 9, Name = "Children's Treasure Map", Money = 1, Experience = 1, Probability = 300 }
            );
        }

        private static void GenerateNegativeGameEvents(ModelBuilder builder)
        {
            builder.Entity<NegativeGameEvent>().HasData(
                new NegativeGameEvent
                {
                    Id = 1, Name = "Rockfall",
                    Description = "As you are mining, the cave walls rumble and rocks tumble down on you",
                    DefenseWithGearDescription = "Your mining gear allows you and your tools to escape unscathed",
                    DefenseWithoutGearDescription =
                        "You try to cover your face but the rocks are too heavy. That hurt!",
                    DefenseLoss = 2, Probability = 100
                },
                new NegativeGameEvent
                {
                    Id = 2, Name = "Cave Rat",
                    Description = "As you are mining, you feel something scurry between your feet!",
                    DefenseWithGearDescription =
                        "It tries to bite you, but your mining gear keeps the rat's teeth from sinking in.",
                    DefenseWithoutGearDescription =
                        "It tries to bite you and nicks you in the ankles. It already starts to glow dangerously.",
                    DefenseLoss = 3, Probability = 50
                },
                new NegativeGameEvent
                {
                    Id = 3, Name = "Sinkhole",
                    Description = "As you are mining, the ground suddenly gives way and you fall down into a chasm!",
                    DefenseWithGearDescription = "Your gear grants a safe landing, protecting you and your pickaxe.",
                    DefenseWithoutGearDescription =
                        "You tumble down the dark hole and take a really bad landing. That hurt!",
                    DefenseLoss = 2, Probability = 100
                },
                new NegativeGameEvent
                {
                    Id = 4, Name = "Ancient Bacteria",
                    Description = "As you are mining, you uncover a green slime oozing from the cracks!",
                    DefenseWithGearDescription = "Your gear barely covers you from the noxious goop. You are safe.",
                    DefenseWithoutGearDescription =
                        "The slime covers your hands and arms and starts biting through your flesh. This hurts!",
                    DefenseLoss = 3, Probability = 50
                }
            );
        }

        private static void GenerateAttackItems(ModelBuilder builder)
        {
            builder.Entity<Item>().HasData(
                new Item { Id = 1, Name = "Basic Pickaxe", Attack = 50, Price = 50 },
                new Item { Id = 2, Name = "Enhanced Pick", Attack = 300, Price = 300 },
                new Item { Id = 3, Name = "Turbo Pick", Attack = 500, Price = 500 },
                new Item { Id = 4, Name = "Mithril Warpick", Attack = 5000, Price = 15000 },
                new Item { Id = 5, Name = "Thor's Hammer", Attack = 50, Price = 1000000 }
            );
        }

        private static void GenerateDefenseItems(ModelBuilder builder)
        {
            builder.Entity<Item>().HasData(
                new Item { Id = 6, Name = "Torn Clothes", Defense = 20, Price = 20 },
                new Item { Id = 7, Name = "Hardened Leather Gear", Defense = 150, Price = 200 },
                new Item { Id = 8, Name = "Iron plated Armor", Defense = 500, Price = 1000 },
                new Item { Id = 9, Name = "Rock Shield", Defense = 2000, Price = 10000 },
                new Item { Id = 10, Name = "Emerald Shield", Defense = 2000, Price = 10000 },
                new Item { Id = 11, Name = "Diamond Shield", Defense = 20000, Price = 10000 }
            );
        }

        private static void GenerateFoodItems(ModelBuilder builder)
        {
            builder.Entity<Item>().HasData(
                new Item { Id = 12, Name = "Apple", ActionCooldownSeconds = 50, Fuel = 4, Price = 8 },
                new Item { Id = 13, Name = "Energy Bar", ActionCooldownSeconds = 45, Fuel = 5, Price = 10 },
                new Item { Id = 14, Name = "Field Rations", ActionCooldownSeconds = 30, Fuel = 30, Price = 300 },
                new Item { Id = 15, Name = "Abbye cheese", ActionCooldownSeconds = 25, Fuel = 100, Price = 500 },
                new Item { Id = 16, Name = "Abbye Beer", ActionCooldownSeconds = 25, Fuel = 100, Price = 500 },
                new Item { Id = 17, Name = "Celestial Burrito", ActionCooldownSeconds = 15, Fuel = 500, Price = 10000 }
            );
        }

        private static void GenerateDecorativeItems(ModelBuilder builder)
        {
            builder.Entity<Item>().HasData(
                new Item
                {
                    Id = 18, Name = "Balloon", Description = "Does nothing. Do you feel special now?", Price = 10
                },
                new Item
                {
                    Id = 19, Name = "Blue Medal", Description = "For those who cannot afford the Crown of Flexing.",
                    Price = 100000
                },
                new Item
                {
                    Id = 20, Name = "Crown of Flexing",
                    Description = "Yes, show everyone how much money you are willing to spend on something useless!",
                    Price = 500000
                }
            );
        } */
    }
}
