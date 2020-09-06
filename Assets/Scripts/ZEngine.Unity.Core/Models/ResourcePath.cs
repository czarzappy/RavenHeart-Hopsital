using System;

namespace ZEngine.Unity.Core.Models
{
    [Serializable]
    public struct ResourcePath
    {
        public const string UNITY_RESOURCE_PATH_SEPARATOR = "/";
        
        public string[] Path;

        public ResourcePath(params string[] path)
        {
            Path = path;
        }
        public string this[int i] => Path[i];

        public string UnityPath => string.Join(UNITY_RESOURCE_PATH_SEPARATOR, Path);

        public int Length
        {
            get
            {
                if (Path == null)
                {
                    return -1;
                }

                return Path.Length;
            }
        }

        public override string ToString()
        {
            return string.Join("->", Path);
        }

        public static ResourcePath Combine(params string[] path)
        {
            return new ResourcePath(path);
        }

        public static ResourcePath Combine(string arg0, string arg1, ResourcePath resourcePath)
        {
            int len = resourcePath.Length;
            if (len < 0)
            {
                ZBug.Warn("RESOURCEPATH","Attempting to combine invalid resource path");
                return default;
            }
            
            string[] args = new string[2 + len];

            int idx = 0;
            args[idx++] = arg0;
            args[idx++] = arg1;

            for (int i = 0; i < len; i++)
            {
                args[idx++] = resourcePath[i];
            }
            
            return new ResourcePath(args);
        }
    }
}