using System;
using Noho.Messages;
using Noho.Unity.Components;
using Noho.Unity.Messages;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Math;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery.Gunk
{
    [Serializable]
    public class Defib
    {
        public float SpeedRamp;
        public float GoodPercentage = 0.1f;
        public int NumGoodCount = 5;
        public float PauseDuration = 0.5f;

        private int mGoodCount = 0;
        private float mResetTime;
        // private bool mPause = false;

        #region Properties
        public float T => Time.time - mResetTime;

        public float CurrentT => ZMath.Cos0To1(T * (mGoodCount * SpeedRamp + 1));
        public bool IsDone => mGoodCount >= NumGoodCount;

        // public bool IsPaused => mPause;

        #endregion

        public void Reset()
        {
            mResetTime = Time.time + PauseDuration;
        }
        // public void Unpause()
        // {
        //     mPause = false;
        // }

        // public void Pause()
        // {
        //     mPause = true;
        // }

        public void OnSuccess()
        {
            mGoodCount++;
        }

        public void Range(Action tooSoonAction, Action tooLateAction, Action goldieLocksAction)
        {
            if (CurrentT < (.5 - GoodPercentage))
            {
                tooSoonAction();
            }
            else if (CurrentT > (.5 + GoodPercentage))
            {
                tooLateAction();
            }
            else
            {
                goldieLocksAction();
            }
        }
    }
    public class DefibrillatorController : UIMonoBehaviour
    {
        public Color TooSoon;
        public Color TooLate;
        public Color Good;
        
        public Slider Slider;

        public Image InsideImage;

        public Image HandleImage;

        public Image FillImage;

        public Defib Defib = new Defib();

        public void Awake()
        {
            Init();
        }

        public void Init()
        {
            Send.Msg(new DefibrillatorActiveMsg());
        }

        public void Tick()
        {
            // if (Defib.IsPaused)
            // {
            //     return;
            // }
            
            if (Defib.IsDone)
            {
                return;
            }
            
            Slider.value = Defib.CurrentT;
            
            Defib.Range(OnTooSoon, OnTooLate, OnJustRight);

            if (IsInput)
            {
                OnInput();
            }
        }

        private static bool IsInput
        {
            get
            {
                bool isMouseDown = Input.GetMouseButtonDown(0);
                bool isSpaceDown = Input.GetKeyDown(KeyCode.Space);

                if (isSpaceDown)
                {
                      isSpaceDown = true;
                }

                return isMouseDown || isSpaceDown;
            }
        }

        private void OnJustRight()
        {
            SetColor(Good);

            if (IsInput)
            {
                ZBug.Info("DEFIB", $"Just Right! T: {Defib.CurrentT}");
                OnCompression();
            }
        }

        private void OnTooLate()
        {
            SetColor(TooLate);

            if (IsInput)
            {
                ZBug.Info("DEFIB", $"Too Late! T: {Defib.CurrentT}");
            }
        }

        private void OnTooSoon()
        {
            SetColor(TooSoon);

            if (IsInput)
            {
                ZBug.Info("DEFIB", $"Too Soon! T: {Defib.CurrentT}");
            }
        }

        private void OnCompression()
        {
            Earn.Score(this.tag);
            Show.Nice(transform);

            Defib.OnSuccess();

            if (Defib.IsDone)
            {
                Send.Msg(new GunkClearedMsg
                {
                    Gunk = this
                });
            }
        }

        public void OnInput()
        {
            // Defib.Pause();
            Defib.Reset();
                
            // Invoke(nameof(Defib.Unpause), );
        }

        public void SetColor(Color color)
        {
            InsideImage.color = color;
            HandleImage.color = color;
            FillImage.color = color;
        }
    }
}