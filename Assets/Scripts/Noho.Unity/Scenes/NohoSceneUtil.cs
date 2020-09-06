using UnityEngine.SceneManagement;
using ZEngine.Unity.Core.Scenes;

namespace Noho.Unity.Scenes
{
    public static class NohoSceneUtil
    {
        public static OperationMain GetSurgerySceneOperation(this Scene scene)
        {
            return scene.GetRootComponent<OperationMain>();
        }
        
        public static TitleMain GetTitleMain(this Scene scene)
        {
            return scene.GetRootComponent<TitleMain>();
        }
        
        public static PureDialogueMain GetDialogueMain(this Scene scene)
        {
            return scene.GetRootComponent<PureDialogueMain>();
        }
    }
}