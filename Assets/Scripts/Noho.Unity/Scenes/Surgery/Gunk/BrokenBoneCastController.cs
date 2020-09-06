using System.Linq;
using Noho.Messages;
using Noho.Unity.Messages;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery.Gunk
{
    public class BrokenBoneCastController : AIDU
    {
        public BandagePointsController[] BandagePoints;

        private bool mIsDone = false;

        public override void Init()
        {
            ZBug.Info("BROKENBONECAST", "Init");
            MsgMgr.Instance.SubscribeTo<BandagePointsCompleteMsg>(OnBandagePointsComplete);
            
            foreach (BandagePointsController bandagePointsController in BandagePoints)
            {
                bandagePointsController.Init(this.tag);
            }
            
        }

        public override void UnInit()
        {
            ZBug.Info("BROKENBONECAST", "UnInit");
            MsgMgr.Instance.UnsubscribeFrom<BandagePointsCompleteMsg>(OnBandagePointsComplete);
            
            foreach (BandagePointsController bandagePointsController in BandagePoints)
            {
                bandagePointsController.UnInit();
            }
        }

        private void OnBandagePointsComplete(BandagePointsCompleteMsg msg)
        {
            if (mIsDone)
            {
                return;
            }

            if (!BandagePoints.Contains(msg.Points))
            {
                return;
            }
            
            foreach (var points in BandagePoints)
            {
                if (!points.IsComplete)
                {
                    return;
                }
            }

            mIsDone = true;
            Send.Msg(new GunkClearedMsg
            {
                Gunk = this,
                KeepAfterClear = true
            });
        }
    }
}