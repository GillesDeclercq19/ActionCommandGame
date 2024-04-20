namespace ActionCommandGame.Services.Model.Requests
{
    public class PositiveGameEventRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Money { get; set; }
        public int Experience { get; set; }
        public int Probability { get; set; }
    }
}
