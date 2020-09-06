using System;
using System.Collections;
using Noho.Messages;
using Noho.Models;
using Noho.Parsing.Models;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using ZEngine.Unity.Core.Messaging;
using ZEngine.Unity.Core.Scenes;

namespace Noho.Unity.Scenes
{
    public partial class BrainMain
    {

        private void OnCutSceneCompleted(CutSceneCompletedMsg message)
        {
            LoadNextScriptItem();
        }
        private void OnLoadNewCutScene(LoadNewCutSceneMsg message)
        {
            CutSceneDef cutSceneDef = message.NewCutScene;

            var updateCutsceneInPlace = LoadNewCutsceneInPlaceFactory(cutSceneDef);

            // Overlay scene is a cutscene, can update in place
            if (mCurrentOverlaySceneType == SceneType.CUTSCENE ||
                mOverlayScene == NohoConstants.SceneNames.DIALOGUE_SCENE)
            {
                StartCoroutine(updateCutsceneInPlace());
                return;
            }

            // if (mOverlayScene == null)
            // {
            //     // fade out
            //     StartCoroutine(FadeOutThenRun(() =>
            //     {
            //         // fade in
            //         ZBug.Info("BRAIN", $"Directly loading dialogue scene");
            //         LoadPureDialogueScene(cutSceneDef);
            //     }));
            //     return;
            // }
            // else
            // {
            //     // Stop existing overlay
            //     // fade out
            //     ZBug.Info("BRAIN", $"Found overlay: {mOverlayScene} while loading dialogue scene scene");
            //     StartCoroutine(KillOverlayScene(mOverlayScene, operation =>
            //     {
            //         // fade in
            //         LoadPureDialogueScene(cutSceneDef);
            //     }));
            // }
            LoadOverlayScene(() => LoadPureDialogueScene(cutSceneDef));
        }
        
        /// <summary>
        /// Fade out and fade in
        /// </summary>
        /// <param name="cutSceneDef"></param>
        /// <returns></returns>
        private Func<IEnumerator> LoadNewCutsceneInPlaceFactory(CutSceneDef cutSceneDef)
        {
            IEnumerator LoadNewCutsceneInPlace()
            {
                yield return FadeOut();
                
                ZBug.Info("BRAIN", $"Dialogue scene already loaded");
                mCurrentOverlaySceneType = SceneType.CUTSCENE;
                        
                Context.SceneManager.LoadItem(cutSceneDef);
                Send.Msg(new CutSceneLoadedMsg
                {
                    CutScene = cutSceneDef
                });
                
                yield return FadeIn();
            }

            return LoadNewCutsceneInPlace;
        }

        private void LoadPureDialogueScene(CutSceneDef cutSceneDef)
        {
            void OnPureDialogueSceneLoaded(Scene scene, LoadSceneMode mode)
            {
                SceneManager.SetActiveScene(scene);
                mCurrentOverlaySceneType = SceneType.CUTSCENE;

                PureDialogueMain pureDialogueMain = scene.GetRootComponent<PureDialogueMain>();

                pureDialogueMain.Init();

                Context.SceneManager.LoadItem(cutSceneDef);

                Send.Msg(new CutSceneLoadedMsg {CutScene = cutSceneDef});
            }
            
            LoadOverlayScene(NohoConstants.SceneNames.DIALOGUE_SCENE, OnPureDialogueSceneLoaded);
        }
    }
}