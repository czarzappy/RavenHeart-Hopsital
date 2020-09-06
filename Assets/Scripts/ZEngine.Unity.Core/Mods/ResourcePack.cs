using System.Collections.Generic;
using UnityEngine;
using ZEngine.Unity.Core.Models;

namespace ZEngine.Unity.Core.Mods
{
    public abstract class ResourcePack
    {
        public abstract bool IsPresent<T>(ResourcePath resourcePath) where T : Object;

        public abstract T Load<T>(ResourcePath resourcePath) where T : Object;

        public abstract IEnumerable<T> LoadAll<T>(ResourcePath resourcePath) where T : Object;
    }
}