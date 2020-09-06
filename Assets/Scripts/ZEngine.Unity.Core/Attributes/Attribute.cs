using System;
using UnityEngine;
using ZEngine.Unity.Core.Components;

namespace ZEngine.Unity.Core.Attributes
{
    [Serializable]
    public abstract class InitFiniteTickZAttr<TIn> : FiniteTickZAttr where TIn : struct
    {
        public TIn initData;

        public void Awake()
        {
            if (!initData.Equals(default(TIn)))
            {
                Init();
            }
        }

        public void Init(TIn initData)
        {
            this.initData = initData;

            Init();
        }
    }
    
    [Serializable]
    public abstract class InitDurationTickZAttr<TIn> : FiniteTickZAttr where TIn : struct
    {
        public TIn initData;

        public float Duration;

        public override float T => ElapsedTime / Duration;

        public new bool IsDone => ElapsedTime >= Duration;
        
        public void Awake()
        {
            if (!initData.Equals(default(TIn)))
            {
                Init();
            }
        }
        
        public void Init(TIn initData, float duration)
        {
            this.initData = initData;
            Duration = duration;

            Init();
        }
    }
    
    [Serializable]
    public abstract class InitZAttr<TIn> : ZAttr
    {
        /// <summary>
        /// Public for debugging purposes
        /// </summary>
        public TIn initData;
        
        public void Init(TIn initData)
        {
            this.initData = initData;

            Init();
        }
    }

    [Serializable]
    public abstract class FiniteTickZAttr : ZAttr
    {
        public void ForceEnd()
        {
            float t = 1f;
            Tick(t);

            if (t >= 1)
            {
                IsDone = true;
            }
        }
        
        public abstract float T { get; }

        public Action OnEnd;

        public void Update()
        {
            if (IsDone)
            {
                OnEnd?.Invoke();
                gameObject.RemoveZAttr(this);
                return;
            }

            float t = T;
            
            Tick(t);

            if (t >= 1)
            {
                IsDone = true;
            }
        }

        public abstract void Tick(float t);
    }

    [Serializable]
    public abstract class ZAttr : UIMonoBehaviour
    {
        private bool mIsDone;

        protected bool IsDone
        {
            get => mIsDone;
            set => mIsDone = value;
        }
        
        /// <summary>
        /// Denotes time when attribute was last initialized
        /// In seconds since the start of the game
        /// </summary>
        protected float InitTime;
        
        /// <summary>
        /// Denotes the time, in seconds, since the attribute was last initialized
        /// </summary>
        protected float ElapsedTime => Time.time - InitTime;

        public void Init()
        {
            InitTime = Time.time;
            PostInit();
        }
        protected abstract void PostInit();
    }
}