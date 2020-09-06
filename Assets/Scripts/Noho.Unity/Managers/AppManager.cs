using Noho.Configs;
using Noho.Unity.Messages;
using Noho.Unity.Models;
using Noho.Unity.Scenes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Managers
{
    public class AppManager
    {
        private static AppManager gInstance;
        public static AppManager Instance
        {
            get
            {
                if (gInstance == null)
                {
                    gInstance = new AppManager();
                }

                return gInstance;
            }
        }

        public void StartEpisode(TextAsset episodeAsset, bool isIntro = false)
        {
            App.Instance.RuntimeData.PlayingEpisodeAsset = episodeAsset;
            ZBug.Info("TITLE", $"Episode selected: {episodeAsset.name}");
        
            App.Instance.RuntimeData.PlayingEpisodeSceneIdx = 0;
            ZBug.Info("TITLE", "Scene index selected: 0");
            SceneManager.LoadSceneAsync("BrainScene", LoadSceneMode.Single);
        }

        public void GoToCredits()
        {
            SceneManager.LoadScene("CreditScene", LoadSceneMode.Single);
        }

        public void FinishEpisode()
        {
            GoToTitleScene(Win.CHARACTER_SELECT);
        }

        public void GoToTitleScene(Win win = Win.NONE)
        {
            UnityAction<Scene, LoadSceneMode> onSceneLoaded = (scene, mode) =>
            {
                TitleMain titleMain = scene.GetTitleMain();

                if (titleMain != null)
                {
                    titleMain.PushWin(win);
                }
            };
            
            SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
            
            // var value = SceneManager.sceneLoaded;
            void AutoUnSubscriber(Scene scene, LoadSceneMode mode)
            {
                SceneManager.sceneLoaded -= AutoUnSubscriber;
                
                // LoadingAudioListener.enabled = false;
                onSceneLoaded(scene, mode);

                // StartCoroutine(FadeIn());

                // yield return new WaitUntil(() => Math.Abs(BlackImage.color.a - 1f) < Mathf.Epsilon);
            }

            // SceneManager.sceneLoaded = null;
            SceneManager.sceneLoaded += AutoUnSubscriber;
        }

        public void UnlockNextEpisode(string characterDevName) 
        {
            CharacterConfig characterConfig =
                NohoConfigResolver.GetConfig<CharacterConfig>(characterDevName);
            App.Instance.PersistentData.UnlockFirstEpisode(characterConfig);
            
            MsgMgr.Instance.Send(new NewNotifMsg
            {
                Notif = $"Unlocked {characterConfig.RawName} Episodes!"
            });
        }
    }
}
