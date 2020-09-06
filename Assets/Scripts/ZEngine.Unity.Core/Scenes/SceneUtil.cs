using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZEngine.Unity.Core.Scenes
{
    public static class SceneUtil
    {
        public static T GetRootComponent<T>(this Scene scene) where T : Object
        {
            foreach (GameObject rootGameObject in scene.GetRootGameObjects())
            {
                var operationContext = rootGameObject.GetComponent<T>();
                if (operationContext == null)
                {
                    continue;
                }

                return operationContext;
            }

            return null;
        }
    }
}