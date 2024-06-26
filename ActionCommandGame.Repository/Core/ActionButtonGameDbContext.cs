﻿using ActionCommandGame.Model;
using ActionCommandGame.Repository.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore.Design;

namespace ActionCommandGame.Repository.Core
{
    public class ActionButtonGameDbContext : IdentityDbContext
    {
        public ActionButtonGameDbContext(DbContextOptions<ActionButtonGameDbContext> options) : base(options)
        {
        }

        public DbSet<PositiveGameEvent> PositiveGameEvents { get; set; }
        public DbSet<NegativeGameEvent> NegativeGameEvents { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerItem> PlayerItems { get; set; }

        /* uncomment to migrate db
        public class ActionButtonGameDbContextFactory : IDesignTimeDbContextFactory<ActionButtonGameDbContext>
        {
            public ActionButtonGameDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<ActionButtonGameDbContext>();
                optionsBuilder.UseSqlServer("Data Source=GILLES-LAP\\SQLEXPRESS;Initial Catalog=ActionCommandDb;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

                return new ActionButtonGameDbContext(optionsBuilder.Options);
            }
        } */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureRelationships();
            base.OnModelCreating(modelBuilder);
        }

        public async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            GeneratePositiveGameEvents();
            GenerateNegativeGameEvents();
            GenerateAttackItems();
            GenerateDefenseItems();
            GenerateFoodItems();
            GenerateDecorativeItems();

            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var adminName = "Stockey";
            var adminUser = await userManager.FindByNameAsync(adminName);
            if (adminUser == null)
            {
                adminUser = new IdentityUser { UserName = adminName };
                await userManager.CreateAsync(adminUser, "Goku1234+"); // Set the password for the admin user
            }

            // Check if the "Admin" role exists, if not, create it
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Add the "Admin" role to the admin user
            await userManager.AddToRoleAsync(adminUser, "Admin");

            // Add your admin player here and link it to the user

            Players.Add(new Player { Name = "StockeyAdmin", Zeni = 100000, Experience = 10000, UserId = adminUser.Id });

            Players.Add(new Player { Name = "Dev", Zeni = 10000, Experience = 500});
            Players.Add(new Player { Name = "Test", Zeni = 100, Experience = 5 });
            Players.Add(new Player { Name = "Gilles", Zeni = 10, Experience = 50 });

            await SaveChangesAsync();
        }

        private void GeneratePositiveGameEvents()
        {
            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Training with Master Roshi",
                Description = "You undergo intense training sessions with Master Roshi, honing your martial arts skills and increasing your power level.",
                Zeni = 10,
                Probability = 1000
            });

            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Dragon Ball Search",
                Description = "You find a Dragon Ball hidden in a remote location, bringing you one step closer to making a wish.",
                Zeni = 20,
                Experience = 8,
                Probability = 950
            });

            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Encounter with Friendly Saiyan",
                Description = "You encounter a friendly Saiyan warrior who offers to spar with you, helping you improve your combat techniques.",
                Zeni = 25,
                Experience = 12,
                Probability = 900
            });

            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Discovering the Hyperbolic Time Chamber",
                Description = "You stumble upon the Hyperbolic Time Chamber and train inside, mastering new techniques and abilities.",
                Zeni = 30,
                Experience = 18,
                Probability = 850
            });

            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Sensei's Wisdom",
                Description = "You receive wise counsel from Master Korin, gaining valuable insights into unlocking your hidden potential.",
                Zeni = 50,
                Experience = 25,
                Probability = 800
            });

            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Gathering Senzu Beans",
                Description = "You find a field of Senzu Beans and collect them, ensuring you have plenty of healing supplies for your journey.",
                Zeni = 75,
                Experience = 30,
                Probability = 750
            });

            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "A Visit to Kami's Lookout",
                Description = "You visit Kami's Lookout and receive training from Mr. Popo, enhancing your spiritual strength and focus.",
                Probability = 700
            });

            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Uncovering Ancient Saiyan Artifact",
                Description = "You unearth an ancient Saiyan artifact that amplifies your Saiyan powers, boosting your combat abilities.",
                Zeni = 120,
                Experience = 75,
                Probability = 650
            });

            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Assistance from Capsule Corp",
                Description = "You receive technological support from Capsule Corp, upgrading your equipment for better performance in battles.",
                Zeni = 140,
                Experience = 85,
                Probability = 600
            });

            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Befriending a Namekian Elder",
                Description = "You befriend a wise Namekian elder who teaches you ancient Namekian techniques, expanding your repertoire of abilities.",
                Zeni = 180,
                Experience = 95,
                Probability = 550
            });

            
            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Fusion Training with Goku",
                Description = "You train with Goku to master the Fusion technique, allowing you to fuse with a partner for increased strength in battles.",
                Zeni = 200,
                Experience = 100,
                Probability = 500
            });

            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Gathering Energy for Spirit Bomb",
                Description = "You gather energy from across the universe to create a Spirit Bomb, a powerful weapon against evil forces.",
                Zeni = 250,
                Experience = 150,
                Probability = 450
            });

            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Unlocking Potential with Elder Kai",
                Description = "You undergo a ritual with Elder Kai that unlocks your hidden potential, pushing your power level beyond its limits.",
                Zeni = 300,
                Experience = 175,
                Probability = 400
            });

            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Mastering Instant Transmission",
                Description = "You master the Instant Transmission technique, allowing you to teleport instantly to any location in the universe.",
                Zeni = 325,
                Experience = 200,
                Probability = 350
            });

            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Attaining Super Saiyan Transformation",
                Description = "You achieve the legendary Super Saiyan transformation, unlocking immense power and strength.",
                Zeni = 350,
                Experience = 225,
                Probability = 300
            });

            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Evolving into Super Saiyan Blue",
                Description = "You evolve your Super Saiyan form into Super Saiyan Blue, reaching a new pinnacle of Saiyan power.",
                Zeni = 375,
                Experience = 250,
                Probability = 250
            });

            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Unlocking Ultra Instinct",
                Description = "You unlock the Ultra Instinct form, achieving a state of heightened awareness and reflexes in battle.",
                Zeni = 400,
                Experience = 300,
                Probability = 200
            });

            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Learning Kaio-Ken Technique",
                Description = "You learn the Kaio-Ken technique, allowing you to multiply your power for short bursts of incredible strength.",
                Zeni = 420,
                Experience = 310,
                Probability = 150
            });

            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Mastering Kamehameha Wave",
                Description = "You master the Kamehameha wave, unleashing devastating energy blasts with precision and control.",
                Zeni = 450,
                Experience = 340,
                Probability = 100
            });

            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Achieving Perfect Ultra Instinct",
                Description = "You achieve Perfect Ultra Instinct, transcending mortal limits and attaining godlike power and speed.",
                Zeni = 500,
                Experience = 450,
                Probability = 50
            });
        }

        public void GenerateNegativeGameEvents()
        {
            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Power Level Drain",
                Description = "You encounter a powerful enemy who drains your power level, leaving you weakened and vulnerable.",
                DefenseWithGearDescription = "Your protective gear shields you from some of the drain, but you still feel weakened.",
                DefenseWithoutGearDescription = "Unable to defend against the drain, you feel your energy slipping away rapidly.",
                DefenseLoss = 2,
                Probability = 80
            });

            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Mysterious Energy Barrier",
                Description = "You come across a barrier of mysterious energy that blocks your path, preventing you from progressing.",
                DefenseWithGearDescription = "Your gear helps you analyze the barrier, but you're still unable to bypass it.",
                DefenseWithoutGearDescription = "Lacking the necessary tools, you're unable to overcome the barrier and are forced to retreat.",
                DefenseLoss = 1,
                Probability = 70
            });

            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Gravity Distortion",
                Description = "You encounter a region with distorted gravity, making movement and combat difficult.",
                DefenseWithGearDescription = "Your gear provides some resistance to the gravity distortion, but it's still challenging to navigate.",
                DefenseWithoutGearDescription = "Without specialized gear, you struggle to move under the extreme gravitational forces.",
                DefenseLoss = 2,
                Probability = 60
            });

            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Temporal Anomaly",
                Description = "You stumble into a temporal anomaly, causing time to fluctuate unpredictably in the area.",
                DefenseWithGearDescription = "Your gear offers some protection against the temporal fluctuations, but time still behaves erratically.",
                DefenseWithoutGearDescription = "Caught without protection, you experience disorienting shifts in time, making it difficult to act effectively.",
                DefenseLoss = 3,
                Probability = 50
            });

            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Mystic Curse",
                Description = "You're cursed by a mysterious entity, causing your abilities to falter and misfire.",
                DefenseWithGearDescription = "Your gear mitigates some of the curse's effects, but you still struggle to perform at your best.",
                DefenseWithoutGearDescription = "Lacking any protection, you suffer the full brunt of the curse, leaving you weakened and vulnerable.",
                DefenseLoss = 3,
                Probability = 40
            });

            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Dark Energy Surge",
                Description = "A surge of dark energy sweeps through the area, disrupting your senses and draining your strength.",
                DefenseWithGearDescription = "Your gear provides partial protection, but you still feel the effects of the dark energy surge.",
                DefenseWithoutGearDescription = "Without any protective measures, you're overwhelmed by the dark energy, leaving you drained and disoriented.",
                DefenseLoss = 2,
                Probability = 50
            });
        }

        private void GenerateAttackItems()
        {
            Items.Add(new Item { Name = "Enraged Fist", Description = "You punch harder", Attack = 25, Price = 25 });
            Items.Add(new Item { Name = "Power Pole", Description = "Goku's favorite tool as a child", Attack = 50, Price = 50 });
            Items.Add(new Item { Name = "Launch's Gun", Description = "The accuracy is insane", Attack = 100, Price = 150 });
            Items.Add(new Item { Name = "Z-Sword", Description = "Can you wield it?", Attack = 300, Price = 300 });
            Items.Add(new Item { Name = "Kienzan (Destructo Disc)", Description = "Basic Ki Blast", Attack = 500, Price = 500 });
            Items.Add(new Item { Name = "Supreme Kamehameha", Description = "Strongest Kamehameha there is", Attack = 5000, Price = 15000 });
        }

        private void GenerateDefenseItems()
        {
            Items.Add(new Item { Name = "Turtle School Gi", Description = "Cheap but durable", Defense = 20, Price = 20 });
            Items.Add(new Item { Name = "Namekian Regeneration Band", Description = "Minor cuts will heal", Defense = 150, Price = 200 });
            Items.Add(new Item { Name = "Saiyan Battle Armor", Description = "A Saiyan's most trusted defense", Defense = 500, Price = 1000 });
            Items.Add(new Item { Name = "Frieza's Armor", Description = "Only Goku could pierce through it", Defense = 2000, Price = 10000 });
            Items.Add(new Item { Name = "Android Barrier Device", Description = "Don't get it broken or it will explode your enemy and you", Defense = 2000, Price = 10000 });
            Items.Add(new Item { Name = "Mystic Robe", Description = "ᓵ¡ᔑᓭℸ", Defense = 20000, Price = 10000 });
        }

        private void GenerateFoodItems()
        {
            Items.Add(new Item { Name = "Senzu Bean", Description = "Extra-small energy boost", ActionCooldownSeconds = 50, Ki = 4, Price = 8 });
            Items.Add(new Item { Name = "Dino Meat", Description = "Small energy boost", ActionCooldownSeconds = 45, Ki = 10, Price = 15 });
            Items.Add(new Item { Name = "Kami's Sacred Water", Description = "Medium energy boost", ActionCooldownSeconds = 30, Ki = 30, Price = 300 });
            Items.Add(new Item { Name = "Sacred Fruit of the Tree of Might", Description = "High energy boost", ActionCooldownSeconds = 25, Ki = 100, Price = 500 });
            Items.Add(new Item { Name = "King Yemma's Enchanted Rice Cake", Description = "Large energy boost", ActionCooldownSeconds = 20, Ki = 250, Price = 800 });
            Items.Add(new Item { Name = "Ramen", Description = "Ultimate energy boost", ActionCooldownSeconds = 15, Ki = 500, Price = 10000 });

        }

        private void GenerateDecorativeItems()
        {
            Items.Add(new Item
            {
                Name = "Dragon Ball", 
                Description = "You bought a Dragon Ball from AliExpress! This will definitely do something, right?", 
                Price = 10
            });
            Items.Add(new Item
            {
                Name = "Super Saiyan", 
                Description = "You thought it was that easy to achieve it, huh?", 
                Price = 100000
            });
            Items.Add(new Item
            {
                Name = "Time Machine",
                Description = "All that money spend, just to die to Cell again",
                Price = 500000
            });
            Items.Add(new Item
            {
                Name = "Dragon Radar", 
                Description = "A device that locates the Dragon Balls. Collect them all!", 
                Price = 1000
            });
            Items.Add(new Item
            {
                Name = "Capsule Corp Capsule",
                Description = "Compact storage device from Capsule Corporation. Convenient!",
                Price = 500
            });
            Items.Add(new Item
            {
                Name = "Flying Nimbus",
                Description = "The magical cloud that Goku used to travel. Beware of falling off!",
                Price = 1500
            });
        } 
    } 
}
