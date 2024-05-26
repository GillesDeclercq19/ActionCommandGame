namespace ActionCommandGame.Services.Model.Requests
{
    public class ItemRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Ki { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int ActionCooldownSeconds { get; set; }
    }
}
