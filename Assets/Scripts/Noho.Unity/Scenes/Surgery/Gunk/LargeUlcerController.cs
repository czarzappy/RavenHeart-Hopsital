using Noho.Configs;
using Noho.Messages;
using Noho.Models;
using Noho.Unity.Attributes;
using Noho.Unity.Components;
using Noho.Unity.Messages;
using Noho.Unity.Models;
using UnityEngine;
using ZEngine.Unity.Core.Attributes;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery.Gunk
{
    public class LargeUlcerController : UIMonoBehaviour
    {
        public IncisionAreaController IncisionArea;
        public GameObject Gunk;
        // public Image Patch;
        
        public void Start()
        {
            Init();
        }

        public enum LargeUlcerPhase : byte
        {
            INCISION = 0,
            REMOVE = 1,
            PATCH = 2,
            MEDICINE = 3,
            CLEARED
        }

        public LargeUlcerPhase Phase;

        private GoalGunkDragAttr mPatchGoalDragAttr;
        private TrayGunkDragAttr mGunkDragAttr;

        // public Vector3 PatchGoalPos;

        private void Init()
        {
            ZBug.Info("GUNK", "[LARGE-ULCER] Transitioning to incision phase");
            Phase = LargeUlcerPhase.INCISION;

            // PatchGoalPos = Patch.gameObject.GetComponent<RectTransform>().position;
            
            MsgMgr.Instance.SubscribeTo<GunkClearedMsg>(OnGunkCleared);
            
            IncisionArea.Init(this.tag);
            
            // Patch.Hide();
        }

        public void OnDestroy()
        {
            UnInit();
        }

        private void UnInit()
        {
            MsgMgr.Instance.UnsubscribeFrom<GunkClearedMsg>(OnGunkCleared);
        }

        private void OnGunkCleared(GunkClearedMsg message)
        {
            if (message.Gunk.GetInstanceID() == IncisionArea.GetInstanceID())
            {
                if (Phase <= LargeUlcerPhase.INCISION)
                {
                    Show.Nice(message.Gunk.transform, transform);
                    
                    MsgMgr.Instance.UnsubscribeFrom<GunkClearedMsg>(OnGunkCleared);
                    ZBug.Info("GUNK", "[LARGE-ULCER] Transitioning to Remove gunk phase");
                    Phase = LargeUlcerPhase.REMOVE;

                    mGunkDragAttr = Gunk.InitZAttr<TrayGunkDragAttr, TrayGunkDragAttr.InitData>(new TrayGunkDragAttr.InitData
                    {
                        OnTriggerEnterUnknown2D = null,
                        OnGunkCleared = OnGunkCleared
                    });
                    
                    Send.Msg(new EarnScoreMsg
                    {
                        ScoreDelta = NohoConfigResolver.GetConfig<GunkConfig>(this.tag).PartialScore
                    });
                    Send.Msg(new StartNewProcedureStepMsg
                    {
                        Tag = this.tag,
                        Step = ProcedureStep.STEP_3
                    });
                }
            }
        }

        private void OnGunkCleared()
        {
            Gunk.RemoveZAttr(mGunkDragAttr);
            ZBug.Info("GUNK", "[LARGE-ULCER] Transitioning to patch phase");
            Phase = LargeUlcerPhase.PATCH;
            
            Send.Msg(new EarnScoreMsg
            {
                ScoreDelta = NohoConfigResolver.GetConfig<GunkConfig>(this.tag).PartialScore
            });
            Send.Msg(new StartNewProcedureStepMsg
            {
                Tag = this.tag,
                Step = ProcedureStep.STEP_4
            });
            

            // Patch.Show();
            
            // Send.Msg(new FragmentClearedMsg
            // {
            //     Fragment = Patch.gameObject
            // });
            
            // mPatchGoalDragAttr = Patch.gameObject.InitZAttr<GoalGunkDragAttr, GoalGunkDragAttr.InitData>(new GoalGunkDragAttr.InitData
            // {
            //     GoalPos = PatchGoalPos,
            //     OnGoalPlaced = OnPatchGoalPlaced
            // });
        }

        private void OnPatchGoalPlaced()
        {
            // Patch.gameObject.RemoveZAttr(mPatchGoalDragAttr);
            ZBug.Info("GUNK", "[LARGE-ULCER] Transitioning to medicine phase");
            Phase = LargeUlcerPhase.MEDICINE;
            
            Send.Msg(new EarnScoreMsg
            {
                ScoreDelta = NohoConfigResolver.GetConfig<GunkConfig>(this.tag).PartialScore
            });
            Send.Msg(new StartNewProcedureStepMsg
            {
                Tag = this.tag,
                Step = ProcedureStep.STEP_5
            });
            
            // Send.Msg(new FragmentPlacedMsg
            // {
            //     Fragment = Patch.gameObject
            // });
        }
        
        public void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case NohoConstants.GOTags.JUICE:
                    switch (Phase)
                    {
                        case LargeUlcerPhase.MEDICINE:
                            OnJuiced();
                            
                            
                            break;
                    }
                    break;
                
                case NohoConstants.GOTags.BANDAGE:

                    switch (Phase)
                    {
                        case LargeUlcerPhase.PATCH:
                            var bandage = GunkUtil.BandageCloneAndShow(other.gameObject, this.transform);
                            
                            OnPatchGoalPlaced();
                            break;
                    }

                    break;
            }
        }

        private void OnJuiced()
        {
            Phase = LargeUlcerPhase.CLEARED;
            // Patch.gameObject.InitDurationTickZAttr<ColorFadeZAttr, ColorFadeZAttr.InitData>(new ColorFadeZAttr.InitData
            // {
            //     SourceType = ColorFadeZAttr.SourceType.IMAGE,
            //     Image = Patch,
            //     StartColor = Patch.color,
            //     EndColor = Color.green,
            // },  NohoUISettings.INCISION_JUICE_DURATION);
            //
            // Patch.transform.SetParent(this.transform, true);
            
            Send.Msg(new GunkClearedMsg
            {
                Gunk = this
            });
        }
    }
}