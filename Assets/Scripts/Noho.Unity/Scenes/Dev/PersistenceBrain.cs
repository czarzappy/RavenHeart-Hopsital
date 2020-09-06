using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZEngine.Unity.Core.Components;

namespace Noho.Unity.Scenes.Dev
{
    public class PersistenceBrain : UIMonoBehaviour
    {
        public void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoad;
        }

        public void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SceneManager.LoadScene("PersistedScene", LoadSceneMode.Additive);

            }
            
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SceneManager.LoadScene("PersistedScene", LoadSceneMode.Additive);
            }
        }

        public Scene mLastScene;
        private void OnSceneLoad(Scene arg0, LoadSceneMode loadSceneMode)
        {
            if (mLastScene != null)
            {
                // SceneManager.Unl
            }
            
            mLastScene = arg0;
            // arg0.
        }
    }
}