using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ZEngine.Unity.Editor.Core
{
    public class ZSelection
    {
        public static string activeAssetPath
        {
            get
            {
                var activeObject = Selection.activeObject;
            
                string assetPath = AssetDatabase.GetAssetPath(activeObject);

                return assetPath;
            }
        }

        public static IEnumerable<TextAsset> TextAssets()
        {
            return Selection.objects.Select((o => o as TextAsset));;
        }

        public static bool HasTexAssetsSelected()
        {
            if (Selection.objects.Length == 0)
            {
                return false;
            }

            return Selection.objects.All((o => o is TextAsset));
        }
    }
}