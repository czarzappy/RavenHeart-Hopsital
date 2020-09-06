using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ZEngine.Unity.Core
{
    public static partial class ZSource
    {
        private static bool GetBadAssetPath<T>(out string resourcePath) where T : Object
        {
            resourcePath = null;
            Type type = typeof(T);

            if (type == typeof(Sprite))
            {
                resourcePath = BAD_SPRITE_PATH;
            }
            else if (type == typeof(AudioClip))
            {
                resourcePath = BAD_AUDIO_PATH;
            }
            else
            {
                ZBug.Error("ZSOURCE",$"Unable to get bad asset path for unknown type: {type}");
                return false;
            }

            return true;
        }
        
        private static ResourceRequest LoadBadAsync<T>() where T : Object
        {
            if (!GetBadAssetPath<T>(out string badAssetPath))
            {
                return null;
            }

            return Resources.LoadAsync<T>(badAssetPath);
        }
        
        private static T LoadBad<T>() where T : Object
        {
            if (!GetBadAssetPath<T>(out string badAssetPath))
            {
                return null;
            }
            
            T asset = Resources.Load<T>(badAssetPath);

            if (asset == null)
            {
                ZBug.Error("ZSOURCE",$"No Bad Asset at path: {badAssetPath}");
            }
            return asset;
        }
    }
}