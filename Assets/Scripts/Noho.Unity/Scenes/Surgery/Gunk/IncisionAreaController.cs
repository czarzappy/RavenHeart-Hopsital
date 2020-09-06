using System.Collections.Generic;
using Noho.Configs;
using Noho.Messages;
using Noho.Unity.Messages;
using Noho.Unity.Models;
using UnityEngine;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery.Gunk
{
    public class IncisionAreaController : MonoBehaviour
    {
        public List<IncisionSpotController> IncisionSpots;

        public int NumCutSpots = 0;
        private int NumSpotsGelled = 0;

        public void Start()
        {
            if (!mInit)
            {
                Init(this.tag);
            }
        }

        private bool mInit = false;

        private string mParentTag;

        public void Init(string parentTag)
        {
            mInit = true;

            mParentTag = parentTag;
            
            MsgMgr.Instance.SubscribeTo<SpotGelledMsg>(OnSpotGelledMsg);
            MsgMgr.Instance.SubscribeTo<CutSpotMsg>(OnCutSpotMsg);
        }

        private void OnSpotGelledMsg(SpotGelledMsg message)
        {
            // make sure the cleared spot is ours
            if (!IncisionSpots.Contains(message.CutSpot))
            {
                return;
            }

            NumSpotsGelled++;

            if (NumSpotsGelled >= IncisionSpots.Count)
            {
                Send.Msg(new EarnScoreMsg
                {
                    ScoreDelta = NohoConfigResolver.GetConfig<GunkConfig>(mParentTag).PartialScore
                });
                Send.Msg(new StartNewProcedureStepMsg
                {
                    Tag = mParentTag,
                    Step = ProcedureStep.STEP_2
                });
            }
        }

        private void OnCutSpotMsg(CutSpotMsg message)
        {
            // make sure the cleared spot is ours
            if (!IncisionSpots.Contains(message.CutSpot))
            {
                return;
            }
            
            NumCutSpots++;

            if (NumCutSpots >= IncisionSpots.Count)
            {
                Send.Msg(new GunkClearedMsg
                {
                    Gunk = this,
                });
            }
        }

        public void OnDisable()
        {
            MsgMgr.Instance.UnsubscribeFrom<CutSpotMsg>(OnCutSpotMsg);
        }
    }
}