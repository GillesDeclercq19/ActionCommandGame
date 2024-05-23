using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.UI.Mvc.Models
{
    public class PlayModel
    {
        public GameResult? GameResults { get; set; }
        public PlayerResult? PlayerResult { get; set; }
    }
}
