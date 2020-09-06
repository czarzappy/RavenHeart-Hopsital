using Noho.Configs;
using Noho.Unity.Messages;
using Noho.Unity.Models;

namespace Noho.Unity.Managers
{
    public enum CheatType
    {
        UNLOCK_ALL_CHARACTERS
    }
    
    public static class Cheat
    {
        public static void With(CheatType cheatType)
        {
            ZBug.Warn($"APPMANAGER", $"Cheating: {cheatType}");
            switch (cheatType)
            {
                case CheatType.UNLOCK_ALL_CHARACTERS:
                    foreach (CharacterConfig character in 
                        NohoConfigResolver.MasterConfig.Characters)
                    {
                        App.Instance.PersistentData.UnlockFirstEpisode(character);
                    }
                    break;
            }
        }
    }
}