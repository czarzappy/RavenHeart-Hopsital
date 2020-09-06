using System;
using System.IO;
using Noho.Messages;
using Noho.Models;
using Noho.Parsing.Models;
using Noho.Parsing.Parsers;
using Noho.Unity.Factories;
using Noho.Unity.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes
{
    public partial class BrainMain
    {
        private OperationDef mCurrentOperationDef;
        private void OnOperationCompleted(OperationCompletedMsg message)
        {
            ZBug.Info("BRAIN", "Operation Completed");
            LoadNextScriptItem();
        }
        
        private void OnOperationLoaded(OperationLoadedMsg message)
        {
            OperationMusic.clip = ZSource.Load<AudioClip>(NohoResourcePack.FOLDER_BGM, message.Operation.BackgroundMusicPath);
            OperationMusic.Play();
        }

        private void OnLoadNewOperationScene(LoadNewOperationSceneMsg message)
        {
            ZBug.Info("BRAIN", "Loading new operation");
            mCurrentOperationDef = message.NewOperationScene;
            Context.OperationManager.LoadItem(mCurrentOperationDef);
            
            Context.OperationManager.MoveNextPhase();
        }
        
        // private string mCurrentSurgeryScene;

        private void OnLoadNewSurgerySurgeryScene(LoadNewSurgerySceneMsg message)
        {
            ZBug.Info("BRAIN", $"Loading new surgery scene: {message.SceneName}");
            // mCurrentSurgeryScene = message.SceneName;
            mCurrentOverlaySceneType = SceneType.SURGERY;
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

            LoadOverlayScene(() => LoadSurgeryScene(message.SceneName));
        }

        // Loads surgery scene and fades in
        private void LoadSurgeryScene(string surgerySceneName)
        {
            ZBug.Info("BRAIN",$"Loading surgery scene: {surgerySceneName}");
            
            void OnSurgerySceneLoaded(Scene scene, LoadSceneMode mode)
            {
                if (scene.name != NohoConstants.SceneNames.SURGERY_SCENE)
                {
                    return;
                }

                SceneManager.SetActiveScene(scene);
                
                ZBug.Info("BRAIN", $"Loaded surgery scene: {surgerySceneName}");
                OperationMain operationMain = scene.GetSurgerySceneOperation();

                if (operationMain != null)
                {
                    SceneDef sceneDef = SceneDefFactory.FromResources(surgerySceneName);
                    
                    operationMain.Init(sceneDef);
                }
            }

            LoadOverlayScene(NohoConstants.SceneNames.SURGERY_SCENE, OnSurgerySceneLoaded);
        }

        public void RestartOperation()
        {
            ZBug.Info("Brain", "Restarting operation");
            Send.Msg(new LoadNewOperationSceneMsg
            {
                NewOperationScene = mCurrentOperationDef
            });
        }
    }
}