using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using ZEngine.Unity.Core.IO;
using ZEngine.Unity.Core.Models;

namespace ZEngine.Unity.Editor.Core.Menus
{
    public static class CreateColorPalette
    {
        private const string PATH = "Assets/Create/ColorPalette From File";

        [MenuItem(PATH)]
        public static void CreatePalette()
        {
            var textAssets = Selection.objects.Select((o => o as TextAsset));

            foreach (TextAsset textAsset in textAssets)
            {
                string path = AssetDatabase.GetAssetPath(textAsset);
                ZBug.Info($"Path: {path}");
                
                
                ColorPalette colorPalette = ScriptableObject.CreateInstance<ColorPalette>();
                
                List<Color> colors = new List<Color>();
                StringReader sr = new StringReader(textAsset.text);

                string line;
                
                // textAsset.name

                while ((line = sr.ReadLine()) != null)
                {
                    
                    if (!ColorUtility.TryParseHtmlString(line, out Color color))
                    {
                        ZBug.Error($"Failed to parse line: {line}");
                        break;
                    }
                    
                    ZBug.Info($"Color: {color}");
                    colors.Add(color);
                }

                colorPalette.Colors = colors.ToArray();

                string cleanPath = ZPath.GetPathWithoutExtension(path);

                string newPath = $"{cleanPath}.asset";
                ZBug.Info($"Writing to path: {newPath}");
                
                AssetDatabase.CreateAsset(colorPalette, newPath);
                AssetDatabase.Refresh();
                
                AssetDatabase.SaveAssets();
            }
        }

        [MenuItem(PATH, true)]
        public static bool CreatePalette_Validate()
        {
            return ZSelection.HasTexAssetsSelected();
        }
    }
}