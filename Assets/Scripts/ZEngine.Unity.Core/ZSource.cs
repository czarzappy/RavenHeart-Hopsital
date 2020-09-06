using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core.Models;
using ZEngine.Unity.Core.Mods;
using Object = UnityEngine.Object;

namespace ZEngine.Unity.Core
{
    public static partial class ZSource
    {
        private static string BAD_SPRITE_PATH = "BadSprite";
        private static string BAD_AUDIO_PATH = "BadSFX";
        // private 
        
        private static List<ResourcePack> ResourcePacks = new List<ResourcePack>();

        public static void AddResourcePack(ResourcePack resourcePack)
        {
            ResourcePacks.Add(resourcePack);
        }
        
        public static Object Load(params string[] path)
        {
            return Resources.Load(string.Join(ResourcePath.UNITY_RESOURCE_PATH_SEPARATOR, path));
        }
        
        public static Object Load(ResourcePath resourcePath)
        {
            return Resources.Load(resourcePath.UnityPath);
        }


        public static ResourceRequest LoadAsync<T>(params string[] resourcePath) where T : Object
        {
            if (resourcePath == null)
            {
                return LoadBadAsync<T>();
            }

            return LoadAsync<T>(new ResourcePath(resourcePath));
        }

        public static T Load<T>(params string[] resourcePath) where T : Object
        {
            if (resourcePath == null)
            {
                return LoadBad<T>();
            }

            return Load<T>(new ResourcePath(resourcePath));
        }

        public static ResourceRequest LoadAsync<T>(ResourcePath resourcePath) where T : Object
        {
            if (resourcePath.Length <= 0)
            {
                return null;
            }

            // var finalPath = string.Join(RESOURCE_PATH_SEPARATOR, resourcePath.Path);

            ResourceRequest request = Resources.LoadAsync<T>(resourcePath.UnityPath);

            if (request != null)
            {
                return request;
            }

            return null;

            // TODO: Handle async resource pack loading
            // foreach (ResourcePack resourcePack in ResourcePacks)
            // {
            //     asset = resourcePack.Load<T>(resourcePath);
            //
            //     if (asset != null)
            //     {
            //         return asset;
            //     }
            // }
            //
            //
            // ZBug.Error("ZSOURCE", $"No type: {typeof(T)} at resource path: {resourcePath}");
            // return LoadBad<T>();
        }

        public static T Load<T>(ResourcePath resourcePath) where T : Object
        {
            if (resourcePath.Length <= 0)
            {
                return LoadBad<T>();
            }

            // var finalPath = string.Join(RESOURCE_PATH_SEPARATOR, resourcePath.Path);

            T asset = Resources.Load<T>(resourcePath.UnityPath);

            if (asset != null)
            {
                return asset;
            }

            foreach (ResourcePack resourcePack in ResourcePacks)
            {
                asset = resourcePack.Load<T>(resourcePath);

                if (asset != null)
                {
                    return asset;
                }
            }
            
            
            ZBug.Error("ZSOURCE", $"No type: {typeof(T)} at resource path: {resourcePath}");
            return LoadBad<T>();
        }

        public static bool IsPresent<T>(params string[] resourcePath) where T : Object
        {
            if (resourcePath == null)
            {
                ZBug.Warn("ZSOURCE", "Attempted to check if invalid resource path is present");
                return false;
            }

            return IsPresent<T>(new ResourcePath(resourcePath));
        }

        public static bool IsPresent<T>(ResourcePath resourcePath) where T : Object
        {
            if (resourcePath.Length <= 0)
            {
                return false;
            }

            T asset = Resources.Load<T>(resourcePath.UnityPath);

            if (asset != null)
            {
                return true;
            }

            foreach (ResourcePack resourcePack in ResourcePacks)
            {
                if (resourcePack.IsPresent<T>(resourcePath))
                {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<T> LoadAll<T>(params string[] resourcePath) where T : Object
        {
            if (resourcePath == null)
            {
                yield break;
            }

            foreach (T result in LoadAll<T>(new ResourcePath(resourcePath)))
            {
                yield return result;
            }
        }

        public static IEnumerable<T> LoadAll<T>(ResourcePath resourcePath) where T : Object
        {
            var results = Resources.LoadAll<T>(resourcePath.UnityPath);

            bool loadedNone = true;
            foreach (T result in results)
            {
                loadedNone = true;
                yield return result;
            }

            foreach (ResourcePack resourcePack in ResourcePacks)
            {
                foreach (T result in resourcePack.LoadAll<T>(resourcePath))
                {
                    loadedNone = true;
                    yield return result;
                }
            }

            if (loadedNone)
            {
                ZBug.Error("ZSOURCE", $"Found no assets of type: {typeof(T)} to load at path: {resourcePath}");
            }
        }
    }
}