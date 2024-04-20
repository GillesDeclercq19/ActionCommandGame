using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ActionCommandGame.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Fuel = table.Column<int>(type: "int", nullable: false),
                    Attack = table.Column<int>(type: "int", nullable: false),
                    Defense = table.Column<int>(type: "int", nullable: false),
                    ActionCooldownSeconds = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NegativeGameEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    DefenseWithGearDescription = table.Column<string>(type: "longtext", nullable: true),
                    DefenseWithoutGearDescription = table.Column<string>(type: "longtext", nullable: true),
                    DefenseLoss = table.Column<int>(type: "int", nullable: false),
                    Probability = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NegativeGameEvents", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PositiveGameEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    Money = table.Column<int>(type: "int", nullable: true),
                    Experience = table.Column<int>(type: "int", nullable: false),
                    Probability = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositiveGameEvents", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PlayerItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    RemainingFuel = table.Column<int>(type: "int", nullable: false),
                    RemainingAttack = table.Column<int>(type: "int", nullable: false),
                    RemainingDefense = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Money = table.Column<int>(type: "int", nullable: false),
                    Experience = table.Column<int>(type: "int", nullable: false),
                    LastActionExecutedDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CurrentFuelPlayerItemId = table.Column<int>(type: "int", nullable: true),
                    CurrentAttackPlayerItemId = table.Column<int>(type: "int", nullable: true),
                    CurrentDefensePlayerItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_PlayerItems_CurrentAttackPlayerItemId",
                        column: x => x.CurrentAttackPlayerItemId,
                        principalTable: "PlayerItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Players_PlayerItems_CurrentDefensePlayerItemId",
                        column: x => x.CurrentDefensePlayerItemId,
                        principalTable: "PlayerItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Players_PlayerItems_CurrentFuelPlayerItemId",
                        column: x => x.CurrentFuelPlayerItemId,
                        principalTable: "PlayerItems",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "ActionCooldownSeconds", "Attack", "Defense", "Description", "Fuel", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 0, 50, 0, null, 0, "Basic Pickaxe", 50 },
                    { 2, 0, 300, 0, null, 0, "Enhanced Pick", 300 },
                    { 3, 0, 500, 0, null, 0, "Turbo Pick", 500 },
                    { 4, 0, 5000, 0, null, 0, "Mithril Warpick", 15000 },
                    { 5, 0, 50, 0, null, 0, "Thor's Hammer", 1000000 },
                    { 6, 0, 0, 20, null, 0, "Torn Clothes", 20 },
                    { 7, 0, 0, 150, null, 0, "Hardened Leather Gear", 200 },
                    { 8, 0, 0, 500, null, 0, "Iron plated Armor", 1000 },
                    { 9, 0, 0, 2000, null, 0, "Rock Shield", 10000 },
                    { 10, 0, 0, 2000, null, 0, "Emerald Shield", 10000 },
                    { 11, 0, 0, 20000, null, 0, "Diamond Shield", 10000 },
                    { 12, 50, 0, 0, null, 4, "Apple", 8 },
                    { 13, 45, 0, 0, null, 5, "Energy Bar", 10 },
                    { 14, 30, 0, 0, null, 30, "Field Rations", 300 },
                    { 15, 25, 0, 0, null, 100, "Abbye cheese", 500 },
                    { 16, 25, 0, 0, null, 100, "Abbye Beer", 500 },
                    { 17, 15, 0, 0, null, 500, "Celestial Burrito", 10000 },
                    { 18, 0, 0, 0, "Does nothing. Do you feel special now?", 0, "Balloon", 10 },
                    { 19, 0, 0, 0, "For those who cannot afford the Crown of Flexing.", 0, "Blue Medal", 100000 },
                    { 20, 0, 0, 0, "Yes, show everyone how much money you are willing to spend on something useless!", 0, "Crown of Flexing", 500000 },
                    { 21, 1, 1000000, 1000000, "This is almost how a GOD must feel.", 1000000, "GOD MODE", 10000000 }
                });

            migrationBuilder.InsertData(
                table: "NegativeGameEvents",
                columns: new[] { "Id", "DefenseLoss", "DefenseWithGearDescription", "DefenseWithoutGearDescription", "Description", "Name", "Probability" },
                values: new object[,]
                {
                    { 1, 2, "Your mining gear allows you and your tools to escape unscathed", "You try to cover your face but the rocks are too heavy. That hurt!", "As you are mining, the cave walls rumble and rocks tumble down on you", "Rockfall", 100 },
                    { 2, 3, "It tries to bite you, but your mining gear keeps the rat's teeth from sinking in.", "It tries to bite you and nicks you in the ankles. It already starts to glow dangerously.", "As you are mining, you feel something scurry between your feet!", "Cave Rat", 50 },
                    { 3, 2, "Your gear grants a safe landing, protecting you and your pickaxe.", "You tumble down the dark hole and take a really bad landing. That hurt!", "As you are mining, the ground suddenly gives way and you fall down into a chasm!", "Sinkhole", 100 },
                    { 4, 3, "Your gear barely covers you from the noxious goop. You are safe.", "The slime covers your hands and arms and starts biting through your flesh. This hurts!", "As you are mining, you uncover a green slime oozing from the cracks!", "Ancient Bacteria", 50 }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "CurrentAttackPlayerItemId", "CurrentDefensePlayerItemId", "CurrentFuelPlayerItemId", "Experience", "LastActionExecutedDateTime", "Money", "Name" },
                values: new object[,]
                {
                    { 1, null, null, null, 0, null, 100, "John Doe" },
                    { 2, null, null, null, 2000, null, 100000, "John Francks" },
                    { 3, null, null, null, 5, null, 500, "Luc Doleman" },
                    { 4, null, null, null, 200, null, 12345, "Emilio Fratilleci" }
                });

            migrationBuilder.InsertData(
                table: "PositiveGameEvents",
                columns: new[] { "Id", "Description", "Experience", "Money", "Name", "Probability" },
                values: new object[,]
                {
                    { 1, null, 0, null, "Nothing but boring rocks", 1000 },
                    { 2, "It slips out of your hands and rolls inside a crack in the floor. It is out of reach.", 0, null, "The biggest Opal you ever saw.", 500 },
                    { 3, null, 0, null, "Sand, dirt and dust", 1000 },
                    { 4, "You hold it to the light and warm it up to reveal secret texts, but it remains empty.", 0, null, "A piece of empty paper", 1000 },
                    { 5, "The water flows around your feet and creates a dirty puddle.", 0, null, "A small water stream", 1000 },
                    { 6, null, 1, 1, "Junk", 2000 },
                    { 7, null, 1, 1, "Murphy's idea bin", 300 },
                    { 8, null, 1, 1, "Donald's book of excuses", 300 },
                    { 9, null, 1, 1, "Children's Treasure Map", 300 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerItems_ItemId",
                table: "PlayerItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerItems_PlayerId",
                table: "PlayerItems",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_CurrentAttackPlayerItemId",
                table: "Players",
                column: "CurrentAttackPlayerItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_CurrentDefensePlayerItemId",
                table: "Players",
                column: "CurrentDefensePlayerItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_CurrentFuelPlayerItemId",
                table: "Players",
                column: "CurrentFuelPlayerItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerItems_Players_PlayerId",
                table: "PlayerItems",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerItems_Items_ItemId",
                table: "PlayerItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerItems_Players_PlayerId",
                table: "PlayerItems");

            migrationBuilder.DropTable(
                name: "NegativeGameEvents");

            migrationBuilder.DropTable(
                name: "PositiveGameEvents");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "PlayerItems");
        }
    }
}
