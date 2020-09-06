using UnityEngine;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Models;
using ZEngine.Unity.Core.Mods;

namespace ZEngine.Unity.Editor.Core.Validation
{
    public static class Validate
    {
        public static void AudioClip(string scriptName, ResourcePath audioClipAsset)
        {
            if (!ZSource.IsPresent<AudioClip>(audioClipAsset))
            {
                ZBug.Error($"[Script: {scriptName}] Missing Audio Clip, path: {audioClipAsset}");
            }
        }
    }
}