using UnityEngine;

namespace ZEngine.Unity.Core.Extensions
{
    public static class StringExtensions
    {
        public static void CopyToClipboard(this string s)
        {
            TextEditor te = new TextEditor
            {
                text = s
            };
            te.SelectAll();
            te.Copy();
        }
    }
}