using Noho.Managers;

namespace Noho.Unity
{
    public class Context
    {
        public readonly SceneManager SceneManager = new SceneManager();
        public readonly OperationManager OperationManager = new OperationManager();
        public readonly ScriptManager ScriptManager = new ScriptManager();

        public void Init()
        {
            
            SceneManager.Init();


            ScriptManager.Init();
        }
    }
}