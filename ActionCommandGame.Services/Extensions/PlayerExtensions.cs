using ActionCommandGame.Model;
using ActionCommandGame.Services.Helpers;

namespace ActionCommandGame.Services.Extensions
{
    public static class PlayerExtensions
    {
        public static int GetLevel(this Player player)
        {
            return PlayerLevelHelper.GetLevelFromExperience(player.Experience);
        }

        public static int GetExperienceForNextLevel(this Player player)
        {
            return PlayerLevelHelper.GetExperienceForNextLevel(player.Experience);
        }

        public static int GetLevelFromExperience(this Player player)
        {
            return PlayerLevelHelper.GetLevelFromExperience(player.Experience);
        }

        public static int GetRemainingExperienceUntilNextLevel(this Player player)
        {
            return PlayerLevelHelper.GetRemainingExperienceUntilNextLevel(player.Experience);
        }
    }
}
