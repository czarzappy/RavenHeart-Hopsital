using System.IO;
using UnityEditor;

namespace ZEngine.Unity.Editor.Core
{
    public static class ZAssetDatabase
    {
        public static void WriteAllText(string filepath, string data)
        {
            File.WriteAllText(filepath, data);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void WriteStream(string filePath, Stream stream)
        {
            using (FileStream destStream = File.Create(filePath))
            {
                stream.CopyTo(destStream);
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}