using Noho.Messages;
using Noho.Models;
using Noho.Unity.Messages;
using UnityEngine.SceneManagement;
using ZEngine.Unity.Core.Messaging;
using ZEngine.Unity.Core.Scenes;

namespace Noho.Unity.Scenes
{
    public partial class BrainMain
    {
        private void OnPostOpComplete(PostOpCompleteMsg message)
        {
            LoadNextScriptItem();
        }
        private void OnLoadPostOpScene(LoadPostOpSceneMsg message)
        {
            ZBug.Info("BRAIN", $"Loading new surgery scene: {message.PostOpDef}");
            // mCurrentSurgeryScene = message.SceneName;
            // if (mOverlayScene == null)
            // {
            //     // fade out
            //     StartCoroutine(FadeOutThenRun(() =>
            //     {
            //         // fade in
            //         ZBug.Info("BRAIN", "No overlay, directly loading surgery scene");
            //         LoadSurgeryScene(message.SceneName);
            //         
            //     }));
            // }
            // else
            // {
            //     // fade out
            //     ZBug.Info("BRAIN", $"Found overlay: {mOverlayScene} while loading surgery scene");
            //     StartCoroutine(KillOverlayScene(mOverlayScene, operation =>
            //     {
            //         // fade in
            //         LoadSurgeryScene(message.SceneName);
            //     }));
            // }

            LoadOverlayScene(() => LoadPostOpScene(message.PostOpDef));
        }
        
        private void LoadPostOpScene(PostOpDef postOpDef)
        {
            void OnPureDialogueSceneLoaded(Scene scene, LoadSceneMode mode)
            {
                SceneManager.SetActiveScene(scene);
                mCurrentOverlaySceneType = SceneType.POSTOP;

                PostOpMain main = scene.GetRootComponent<PostOpMain>();

                main.Init();
            }
            
            LoadOverlayScene(NohoConstants.SceneNames.POST_OP_SCENE, OnPureDialogueSceneLoaded);
        }
    }
}