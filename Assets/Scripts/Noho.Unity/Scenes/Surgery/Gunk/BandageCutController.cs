using Noho.Configs;
using Noho.Messages;
using Noho.Models;
using Noho.Unity.Messages;
using Noho.Unity.Models;
using UnityEngine;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery.Gunk
{
    public class BandageCutController : MonoBehaviour
    {
        public int NumberOfSuturesRequired;
        public int NumberOfJuiceRequired;

        public Transform Stitches;

        public void Awake()
        {
            Init();
        }
        
        public void Init()
        {
            MsgMgr.Instance.SubscribeTo<SutureFinishedMessage>(OnSutureFinishedMessage);
            
            NumberOfSuturesRequired = 2;
            
            Stitches.Hide();

            mCurrentPhase = Phase.SUTURE;
        }

        public Phase mCurrentPhase;
        
        public enum Phase
        {
            SUTURE,
            BANDAGE,
            MEDIGEL
        }
        private void OnSutureFinishedMessage(SutureFinishedMessage message)
        {
            if (mCurrentPhase == Phase.SUTURE && sutureCount > NumberOfSuturesRequired)
            {
                Send.Msg(new ClearSuturesMsg());
                // Send.Msg(new ClearedGunkMsg());

                Stitches.Show();

                mCurrentPhase = Phase.BANDAGE;
                Earn.Score(this.tag);
                
                Send.Msg(new StartNewProcedureStepMsg
                {
                    Step = ProcedureStep.STEP_2,
                    Tag = this.tag
                });
            }
        }
        

        public void OnDestroy()
        {
            MsgMgr.Instance.UnsubscribeFrom<SutureFinishedMessage>(OnSutureFinishedMessage);
        }

        public int sutureCount = 0;
        public int juiceCount = 0;
        private void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case NohoConstants.GOTags.SUTURE:
                    if (mCurrentPhase == Phase.SUTURE)
                    {
                        sutureCount++;
                    }
                    
                    // Debug.Log($"large cut trigger enter suture: {sutureCount}");
                    break;
                
                case NohoConstants.GOTags.JUICE:

                    if (mCurrentPhase == Phase.MEDIGEL)
                    {
                        HandleJuice();
                    }

                    break;
                
                case NohoConstants.GOTags.BANDAGE:
                    
                    if (mCurrentPhase == Phase.BANDAGE)
                    {
                        var bandage = GunkUtil.BandageCloneAndShow(other.gameObject, this.transform);
                        
                        mCurrentPhase = Phase.MEDIGEL;
                        
                        Earn.Score(this.tag);
                        Send.Msg(new StartNewProcedureStepMsg
                        {
                            Step = ProcedureStep.STEP_3,
                            Tag = this.tag
                        });
                    }
                    
                    break;
            }
        }

        public void OnTriggerStay2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case NohoConstants.GOTags.JUICE:
                    if (mCurrentPhase == Phase.MEDIGEL)
                    {
                        HandleJuice();
                    }
                    break;
            }
        }

        public void HandleJuice()
        {
            juiceCount++;

            if (juiceCount > NumberOfJuiceRequired)
            {
                Send.Msg(new GunkClearedMsg
                {
                    Gunk = this
                });
            }
        }

        // private void OnTriggerExit2D(Collider2D other)
        // {
            // Debug.Log("large cut trigger exit");
        // }
    }
}