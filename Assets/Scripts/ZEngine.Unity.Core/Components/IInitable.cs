using System;
using UnityEngine;

namespace ZEngine.Unity.Core.Components
{
    public interface IInitable
    {
        void Init();
    }
    
    public interface IUnInitable
    {
        void UnInit();
    }

    public abstract class AIDU : MonoBehaviour, IInitable, IUnInitable
    {
        public abstract void Init();
        public abstract void UnInit();

        public void Awake()
        {
            ZBug.Info($"AIDU {this}", "Awake");
            Init();
        }

        public void OnDestroy()
        {
            ZBug.Info($"AIDU {this}", "OnDestroy");
            UnInit();
        }
    }
}