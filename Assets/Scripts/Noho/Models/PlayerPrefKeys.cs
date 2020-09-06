using UnityEngine;

namespace Noho.Models
{
    public static class PlayerPrefKeys
    {
        public static string TEXT_SCROLL_MODE = "TEXT_SCROLL_MODE";
        public static string TEXT_SCROLL_RATE = "TEXT_SCROLL_RATE";
        public static string PLAYER_NAME = "PLAYER_NAME";
        public static string PLAYER_MODEL_TYPE = "PLAYER_MODEL_TYPE";
        public static string CHARACTER_UNLOCKED_EPS_FORMAT = "{0}_UnlockedEpisodes";

        public static string SanitizeProtag(this string s)
        {
            return s.Replace(NohoParsingConstants.CHARACTER_DEV_NAME_PROTAG, PlayerDisplayName);
        }

        public static string PlayerDisplayName
        {
            get
            {
                string playerName = PlayerPrefs.GetString(PLAYER_NAME, "You");

                return $"Dr. {playerName}";
            }
        }
    }
}