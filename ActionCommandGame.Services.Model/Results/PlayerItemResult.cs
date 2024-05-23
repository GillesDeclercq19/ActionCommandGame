namespace ActionCommandGame.Services.Model.Results
{
    public class PlayerItemResult
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int RemainingKi { get; set; }
        public int RemainingAttack { get; set; }
        public int RemainingDefense { get; set; }
    }
}
