using System;
using Noho.Messages;
using Noho.Models;
using UnityEngine;
using ZEngine.Unity.Core.Attributes;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery.Gunk
{
    public class HemorrhageController : MonoBehaviour
    {
        public Vector3 StartPos;
        public bool IsCleared => mIsCleared;

        public void OnEnable()
        {
            Init();
        }

        private Func<bool> IsTool;

        public void Init()
        {
            ZBug.Info("GUNK", "HEMO INIT");
            mIsCleared = false;
            StartPos = this.transform.position;
            IsTool = ToolboxController.IsEquippedCB(NohoConstants.ToolType.SUCTION);
        }

        public void Reset()
        {
            mIsCleared = false;
            this.transform.position = StartPos;
            this.gameObject.RemoveZAttr(mSuctionAttr);
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // ZBug.Info();
                if (IsTool())
                {
                    StartSuction();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                StopSuction();
            }
        }

        private void StopSuction()
        {
            this.gameObject.RemoveZAttr(mSuctionAttr);
            mIsSuctionOn = false;
        }

        private ZAttr mSuctionAttr;

        private bool mIsSuctionOn;
        private void StartSuction()
        {
            ZBug.Info("GUNK", "Hemorrhage start suction");
            mIsSuctionOn = true;
            mSuctionAttr = this.gameObject.InitZAttr<GravitateAttr, GravitateAttr.InitData>(new GravitateAttr.InitData
            {
                GoalMarker = ToolboxController.Instance.Suction.transform,
                ForceCoeff = .04f,
                RangeOfInfluence = -1,
            });
        }

        private bool mIsCleared;
        public void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case NohoConstants.GOTags.SUCTION:
                    ZBug.Info("GUNK", $"Entering SUCTION!");
                    if (mIsCleared || !mIsSuctionOn)
                    {
                        break;
                    }

                    mIsSuctionOn = false;
                    mIsCleared = true;
                    Send.Msg(new GunkClearedMsg
                    {
                        Gunk = this
                    });
                    transform.position = StartPos;
                    break;
            }
        }

        public void OnTriggerStay2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case NohoConstants.GOTags.SUCTION:
                    // ZBug.Info($"Entering SUCTION!");
                    if (mIsCleared || !mIsSuctionOn)
                    {
                        break;
                    }

                    mIsSuctionOn = false;
                    mIsCleared = true;
                    Send.Msg(new GunkClearedMsg
                    {
                        Gunk = this
                    });
                    transform.position = StartPos;
                    break;
            }
        }
    }
}