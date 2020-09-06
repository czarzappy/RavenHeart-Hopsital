using System;
using System.Collections.Generic;
using Noho.Configs;
using Noho.Factories;
using Noho.Models;
using UnityEngine;
using ZEngine.Core.Dialogue.Parsers;
using ZEngine.Unity.Core.Analytics;
using ZEngine.Unity.Steam;

namespace Noho.Unity.Models
{
    public class App
    {
        public readonly RuntimeData RuntimeData = new RuntimeData();
        public PersistentData PersistentData = new PersistentData();
        
        public IScriptParser ScriptParser = ScriptParserFactory.DefaultParser();

        private static App gInstance;

        public static App Instance
        {
            get
            {
                if (gInstance == null)
                {
                    gInstance = new App();
                }
                
                return gInstance;
            }
        }

        public App()
        {
        }

        public bool mInited = false;

        public void Init()
        {
            if (mInited)
            {
                return;
            }

            mInited = true;
            
            RuntimeData.Reset();
            PersistentData.Reset();

            ZAnalytics.Instance.Ping();
            ZAnalytics.Instance.OnGameStarted();

            try
            {

#if UNITY_STANDALONE && !UNITY_EDITOR
#if DEMO_BUILD
            ZBug.Info("APP", "Starting demo build");
            ZSteamClient.Init(NohoConstants.STEAM_DEMO_APP_ID);
#else // DEMO_BUILD
            ZBug.Info("APP", "Starting real build");
            ZSteamClient.Init(NohoConstants.STEAM_APP_ID);
#endif // DEMO_BUILD
#else // UNITY_STANDALONE && !UNITY_EDITOR
                ZBug.Info("APP", "Starting non-standalone build");
#endif // UNITY_STANDALONE && !UNITY_EDITOR
            }
            catch (Exception e)
            {
                ZBug.Ex(e);
            }
            
        }

        public void Tick()
        {
            
        }

        public void Quit()
        {
            ZBug.Info("APP", "Quiting!");
            if (ZSteamClient.IsValid)
            {
                ZBug.Info("APP", "Steam shutdown");
                ZSteamClient.Shutdown();
            }
            else
            {
                ZBug.Info("APP", "Unity App Quit");
                Application.Quit();
            }
        }
    }

    public class PersistentData
    {
        private Dictionary<string, int> CharacterLove = new Dictionary<string, int>();
        private Dictionary<string, int> CharacterEpisodes = new Dictionary<string, int>();
        
        public void UnlockNextEpisode(CharacterConfig characterConfig)
        {
            int unlockedEpisode = GetUnlockedCharacterEpisode(characterConfig);

            CharacterEpisodes[characterConfig.DevName] = unlockedEpisode + 1;
        }
        public void UnlockFirstEpisode(CharacterConfig characterConfig)
        {
            if (characterConfig.FirstEpisodeUnlocked)
            {
                return;
            }
            
            if (!CharacterEpisodes.TryGetValue(characterConfig.DevName, out int currentEpisode))
            {
                currentEpisode = 0;
            }
            
            CharacterEpisodes[characterConfig.DevName] = Mathf.Max(1, currentEpisode);
        }
        
        public int GetUnlockedCharacterEpisode(CharacterConfig config)
        {
            if(!CharacterEpisodes.TryGetValue(config.DevName, out int unlockedEpisode))
            {
                unlockedEpisode = 0;
            }
            // string key = string.Format(PlayerPrefKeys.CHARACTER_UNLOCKED_EPS_FORMAT, config.DevName);

            // if(App.Instance.PersistentData.CharacterEpisodes)
            //     int unlockedEpisode = PlayerPrefs.GetInt(key, 0);
            //
            // if (unlockedEpisode == 0)
            // {
            //     unlockedEpisode = 1;
            // }

            if (config.FirstEpisodeUnlocked)
            {
                unlockedEpisode += 1;
            }

            return unlockedEpisode;
        }

        public void IncrementLove(string characterDevName, int loveDelta)
        {
            if (!CharacterLove.ContainsKey(characterDevName))
            {
                CharacterLove[characterDevName] = 0;
            }

            CharacterLove[characterDevName] += loveDelta;
        }
        public int GetCurrentLoveBy(string characterDevName)
        {
            if (CharacterLove.TryGetValue(characterDevName, out int value))
            {
                return value;
            }

            return 0;
        }

        public void Reset()
        {
            CharacterLove.Clear();
            CharacterEpisodes.Clear();
        }
    }

    public class RuntimeData
    {
        public TextAsset PlayingEpisodeAsset;
        public int PlayingEpisodeSceneIdx;
        
        private Dictionary<string, bool> mFlags = new Dictionary<string, bool>();
        
        #region Properties
        public NohoConstants.ProtagBaseModel ProtagSelection
        {
            get
            {
                var value = PlayerPrefs.GetInt(PlayerPrefKeys.PLAYER_MODEL_TYPE, (int) NohoConstants.ProtagBaseModel.F);
                
                return (NohoConstants.ProtagBaseModel) value;
            }
        }
        #endregion

        public void Reset()
        {
            PlayingEpisodeAsset = null;
            PlayingEpisodeSceneIdx = 0;
            // ProtagSelection = NohoConstants.ProtagBaseModel.A;
            
            mFlags.Clear();
        }

        public bool CheckFlag(string stageActionFlagTag, bool stageActionFlagValue)
        {
            if (mFlags.TryGetValue(stageActionFlagTag, out bool result))
            {
                return result == stageActionFlagValue;
            }

            return false;
        }

        public void SetFlag(string stageActionFlagTag, bool stageActionFlagValue)
        {
            ZBug.Info("APP", $"Flag {stageActionFlagTag} set to value: {stageActionFlagValue}");

            mFlags[stageActionFlagTag] = stageActionFlagValue;
        }
    }
}