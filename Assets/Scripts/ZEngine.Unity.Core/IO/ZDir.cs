using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ZEngine.Unity.Core.IO
{
    public static class ZDir
    {
        public static void CreateDirIfNotExists(string path)
        {
            if (Directory.Exists(path))
            {
                return;
            }

            Directory.CreateDirectory(path);
        }

        public static IEnumerable<string> GetFiles(string path)
        {
            if (!Directory.Exists(path))
            {
                yield break;
            }
                
            foreach (string file in Directory.GetFiles(path))
            {
                yield return file;
            }
        }

        public static IEnumerable<string> GetDirs(string path)
        {
            if (!Directory.Exists(path))
            {
                yield break;
            }
            
            foreach (var chars in Directory.GetDirectories(path))
            {
                yield return chars;
            }
        }

        public static IEnumerable<string> GetFiles(string path, string searchPattern)
        {
            if (!Directory.Exists(path))
            {
                yield break;
            }
            
            foreach (var script in Directory.GetFiles(path, searchPattern))
            {
                yield return script;
            }
        }
    }
}