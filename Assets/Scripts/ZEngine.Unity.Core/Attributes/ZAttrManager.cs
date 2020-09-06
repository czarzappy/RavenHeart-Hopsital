using System;
using System.Collections.Concurrent;
using UnityEngine;
using ZEngine.Core.Collections;
using Object = UnityEngine.Object;

namespace ZEngine.Unity.Core.Attributes
{
    public static class ZAttrManager
    {
        // private static List<ObjectPool<Attribute>> gPools = new List<ObjectPool<Attribute>>();
        private static ConcurrentDictionary<Type, object> gPools = new ConcurrentDictionary<Type, object>();
        
        public static T InitFiniteTickZAttr<T, TIn>(this GameObject gameObject, TIn initData) where T : InitFiniteTickZAttr<TIn> where TIn : struct
        {
            T attribute = gameObject.AddZAttr<T>();
            attribute.Init(initData);

            return attribute;
        }
        
        public static T InitZAttr<T, TIn>(this GameObject gameObject, TIn initData) where T : InitZAttr<TIn>
        {
            T attribute = gameObject.AddZAttr<T>();
            attribute.Init(initData);

            return attribute;
        }
        
        public static T InitDurationTickZAttr<T, TIn>(this GameObject gameObject, TIn initData, float duration) where T : InitDurationTickZAttr<TIn> where TIn : struct
        {
            T attribute = gameObject.AddZAttr<T>();
            attribute.Init(initData, duration);

            return attribute;
        }
        
        public static T AddZAttr<T>(this GameObject gameObject) where T : ZAttr
        {
            return gameObject.AddZAttr<T>(true);
        }
        
        public static T AddZAttr<T>(this GameObject gameObject, bool useExisting) where T : ZAttr
        {
            T found = gameObject.GetComponent<T>();
            if (found != null)
            {
                Object.Destroy(found);
                // return found;
            }
            
            // hope unity handles this memory well
            T newAttr = gameObject.AddComponent<T>();
            
            return newAttr;
        }
        
        public static ObjectPool<T> CreateNewAttributeObjectPool<T>(this GameObject gameObject, Type type) where T : ZAttr
        {
            return new ObjectPool<T>(gameObject.AddZAttr<T>);
        }
        
        public static void RemoveZAttr(this GameObject gameObject, ZAttr attr)
        {
            // hope unity handles this memory well
            Object.Destroy(attr);
            // if (gPools.TryGetValue(attribute.GetType(), out object objPool))
            // {
                // ObjectPool<T> pool = (ObjectPool<T>) objPool;
                
                // ZLog.Info($"Put back attribute: {pool.Count}");
                // pool.PutObject(attribute);
                
                // ZLog.Info($"Destroy attribute: {pool.Count}");
            // }
        }
        
        // public static void PutAttribute<T>(this GameObject gameObject, T attribute) where T : Attribute
        // {
        //     if (gPools.TryGetValue(typeof(T), out object objPool))
        //     {
        //         ObjectPool<T> pool = (ObjectPool<T>) objPool;
        //         
        //         ZLog.Info($"Put back attribute: {pool.Count}");
        //         pool.PutObject(attribute);
        //         
        //         ZLog.Info($"Destroy attribute: {pool.Count}");
        //         Object.Destroy(attribute);
        //     }
        // }
    }
}