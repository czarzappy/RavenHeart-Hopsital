using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Noho.Unity.Scenes
{
    public partial class BrainMain
    {
        private static readonly LoadSceneParameters OVERLAY_SCENE_LOAD_PARAMETERS = new LoadSceneParameters(LoadSceneMode.Additive);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sceneName">Name of a Unity scene</param>
        /// <param name="onSceneUnloaded"></param>
        /// <returns></returns>
        private IEnumerator KillOverlayScene(string sceneName, Action<AsyncOperation> onSceneUnloaded)
        {
            yield return FadeOut();
            
            ZBug.Warn($"Unloading scene: {sceneName}");
            int sceneCount = SceneManager.sceneCount;

            bool found = false;
            for (int sceneIdx = 0; sceneIdx < sceneCount; sceneIdx++)
            {
                Scene scene = SceneManager.GetSceneAt(sceneIdx);
                if (scene.name == sceneName)
                {
                    var operation = SceneManager.UnloadSceneAsync(sceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

                    if (operation == null)
                    {
                        ZBug.Error($"Attempting to unloaded improper scene: {sceneName}");
                    }
                    else
                    {
                        void AutoUnSubscriber(AsyncOperation asyncOperation)
                        {
                            operation.completed -= AutoUnSubscriber;
                            LoadingAudioListener.enabled = true;
                            onSceneUnloaded(asyncOperation);
                        }
                    
                        operation.completed += AutoUnSubscriber;
                        found = true;
                        break;
                    }
                }
            }

            if (!found)
            {
                LoadingAudioListener.enabled = true;
                onSceneUnloaded(null);
            }
        }

        /// <summary>
        /// Loads overlay scene then fades in
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="onSceneLoaded"></param>
        /// <returns></returns>
        private void LoadOverlayScene(string sceneName, UnityAction<Scene, LoadSceneMode> onSceneLoaded)
        {
            mOverlayScene = sceneName;
            ZBug.Info($"Loading scene: {sceneName}");
            SceneManager.LoadScene(sceneName, OVERLAY_SCENE_LOAD_PARAMETERS);

            // var value = SceneManager.sceneLoaded;
            void AutoUnSubscriber(Scene scene, LoadSceneMode mode)
            {
                SceneManager.sceneLoaded -= AutoUnSubscriber;
                
                LoadingAudioListener.enabled = false;
                onSceneLoaded(scene, mode);

                StartCoroutine(FadeIn());

                // yield return new WaitUntil(() => Math.Abs(BlackImage.color.a - 1f) < Mathf.Epsilon);
            }

            // SceneManager.sceneLoaded = null;
            SceneManager.sceneLoaded += AutoUnSubscriber;
        }
        

        public void LoadOverlayScene(Action loadOverlaySceneFactory)
        {
            // No existing overlay scene, can fade out and then load and fade in
            if (mOverlayScene == null)
            {
                // fade out
                StartCoroutine(FadeOutThenRun(() =>
                {
                    // fade in
                    ZBug.Info("BRAIN", "No overlay, directly loading surgery scene");
                    // LoadSurgeryScene(message.SceneName);
                    loadOverlaySceneFactory();
                }));
            }
            else
            {
                // Existing overlay scene, can fade out, kill, then load and fade in
                // fade out
                ZBug.Info("BRAIN", $"Found overlay: {mOverlayScene} while loading surgery scene");
                StartCoroutine(KillOverlayScene(mOverlayScene, operation =>
                {
                    // fade in
                    // LoadSurgeryScene(message.SceneName);
                    loadOverlaySceneFactory();
                }));
            }
        }
    }
}