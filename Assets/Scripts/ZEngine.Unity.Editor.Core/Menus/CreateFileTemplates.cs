using System.IO;
using UnityEditor;
using UnityEngine;

namespace ZEngine.Unity.Editor.Core.Menus
{
    public static class CreateFileTemplates
    {
        public const string DEFAULT_FILE_NAME = "file";
        public const string XML_TEMPLATE_FILE = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
        public const string JSON_TEMPLATE_FILE = "{\n\t\n}";
        
        [MenuItem("Assets/ZCreate/Text File (.txt)")]
        public static void ExecuteTxt()
        {
            string filePath = Path.Combine(ZSelection.activeAssetPath, $"{DEFAULT_FILE_NAME}.txt");
            ZAssetDatabase.WriteAllText(filePath, "");
        }
        
        [MenuItem("Assets/ZCreate/XML (.xml)")]
        public static void ExecuteXml()
        {
            string filePath = Path.Combine(ZSelection.activeAssetPath, $"{DEFAULT_FILE_NAME}.xml");
            ZAssetDatabase.WriteAllText(filePath, XML_TEMPLATE_FILE);
        }
        
        [MenuItem("Assets/ZCreate/JSON (.json)")]
        public static void ExecuteJson()
        {
            string filePath = Path.Combine(ZSelection.activeAssetPath, $"{DEFAULT_FILE_NAME}.json");
            ZAssetDatabase.WriteAllText(filePath, JSON_TEMPLATE_FILE);
        }
    }
}