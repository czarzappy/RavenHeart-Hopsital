using System;
using System.Collections.Generic;
using System.Linq;
using Noho.Messages;
using Noho.Unity.Components;
using Noho.Unity.Messages;
using UnityEngine;
using UnityEngine.Events;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery.Gunk
{
    public class BandagePointsController : UIMonoBehaviour
    {
        public BandagePointController[] Points;
        public bool IsComplete;
        private string ParentTag;

        public Vector3 MidPoint
        {
            get
            {
                Vector3 result = new Vector3();
                foreach (var point in Points)
                {
                    result += point.transform.position;
                }

                return result / Points.Length;
            }
        }

        public void Init(string parentTag)
        {
            ParentTag = parentTag;
            ZBug.Info("BANDAGEPOINTS", "Init");

            MsgMgr.Instance.SubscribeTo<BandageCoveredMsg>(OnBandageCovered);
            
            foreach (var point in Points)
            {
                point.Init();
            }
        }

        public void UnInit()
        {
            ZBug.Info("BANDAGEPOINTS", "UnInit");
            MsgMgr.Instance.UnsubscribeFrom<BandageCoveredMsg>(OnBandageCovered);
        }

        private void OnBandageCovered(BandageCoveredMsg message)
        {
            if (!Points.Contains(message.Point))
            {
                ZBug.Warn("BANDAGEPOINTS", $"{this.name} Not my point: {message.Point.name}");
                return;
            }

            if (IsComplete)
            {
                ZBug.Warn("BANDAGEPOINTS", $"Already completed");
                return;
            }

            int pointIdx = 0;
            foreach (var point in Points)
            {
                if (!point.IsCovered)
                {
                    ZBug.Warn("BANDAGEPOINTS", $"Point no covered, idx: {pointIdx}");
                    return;
                }

                pointIdx++;
            }

            ZBug.Info("BANDAGEPOINTS", "Completed!");
            GunkUtil.BandageCloneAndShow(message.Bandage, this.transform);

            IsComplete = true;

            Earn.Score(ParentTag);
            Show.Nice(MidPoint, transform);
            
            Send.Msg(new BandagePointsCompleteMsg
            {
                Points = this
            });
        }

        private void OnExit(BandagePointController arg0)
        {
        }
    }
}