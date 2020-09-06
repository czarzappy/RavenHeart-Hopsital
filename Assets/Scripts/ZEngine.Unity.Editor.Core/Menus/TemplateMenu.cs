using UnityEditor;

namespace ZEngine.Unity.Editor.Core.Menus
{
    public static class TemplateMenuItem
    {
        public const string PATH = "Assets/Template MenuItem";
        
        [MenuItem(PATH)]
        public static void Execute()
        {
            
        }

        [MenuItem(PATH, true)]
        public static bool Validate()
        {
            return true;
        }
    }
}