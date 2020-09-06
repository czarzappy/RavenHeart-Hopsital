using System.IO;

namespace ZEngine.Unity.Core.IO
{
    public static class ZPath
    {
        public static string GetPathWithoutExtension(string path)
        {
            string ext = Path.GetExtension(path);

            int extLen = ext.Length + 1;
            int strLen = path.Length;

            int cutIdx = strLen - extLen;
            string result = path.Remove(cutIdx);

            return result;
        }
    }
}