using Noho.Configs;
using Noho.Messages;
using Noho.Parsing.Models;
using Noho.Unity.Components;
using Noho.Unity.Messages;
using Noho.Unity.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;
using ZEngine.Unity.Core.Models;

namespace Noho.Unity.Scenes.Surgery
{
    public class ToolTipController : UIMonoBehaviour
    {
        public CanvasGroup CanvasGroup;
        
        public TMP_Text HelpText;
        public TMP_Text SpeakerText;

        public SpeakerBubbleController SpeakerBubbleController;
        public void Awake()
        {
            Init();
        }

        public string OperationSpeakerDevName;
        
        
        private void UnInit()
        {
            MsgMgr.Instance.UnsubscribeFrom<NewToolTipMsg>(OnNewToolTip);
            MsgMgr.Instance.UnsubscribeFrom<StartNewProcedureStepMsg>(OnStartNewProcedureStep);
            MsgMgr.Instance.UnsubscribeFrom<GunkTotallyClearedMsg>(OnGunkTotallyCleared);
        }

        private void Init()
        {
            MsgMgr.Instance.SubscribeTo<NewToolTipMsg>(OnNewToolTip);
            MsgMgr.Instance.SubscribeTo<StartNewProcedureStepMsg>(OnStartNewProcedureStep);
            MsgMgr.Instance.SubscribeTo<GunkTotallyClearedMsg>(OnGunkTotallyCleared);


            if (BrainMain.Instance == null)
            {
                return;
            }
            
            OperationSpeakerDevName = BrainMain.Instance.Context.OperationManager.OperationDef.ToolTipSpeakerDevName;

            if (OperationSpeakerDevName == null)
            {
                SpeakerText.text = "TIP";
            }
            else
            {
                SpeakerText.text = $"{OperationSpeakerDevName}";
                
                CharacterConfig characterConfig = NohoConfigResolver.GetConfig<CharacterConfig>(OperationSpeakerDevName);

                if (characterConfig == null)
                {
                    ZBug.Warn("TOOLTIP", $"Attempted to get character config for unknown character: {OperationSpeakerDevName}");
                }
                else
                {
                    SpeakerBubbleController.Init(characterConfig);
                }
            }

            CanvasGroup.alpha = 0;
        }

        private void OnGunkTotallyCleared(GunkTotallyClearedMsg message)
        {
            if (message.GunkTag == CurrentGunkTag)
            {
                // Cleared the gunk that is being showed now

                LoadNextTooltip(message.NextGunkTag, ProcedureStep.STEP_1);
            }
        }

        private void OnStartNewProcedureStep(StartNewProcedureStepMsg message)
        {
            var nextGunkTag = message.Tag;
            LoadNextTooltip(nextGunkTag, message.Step);
        }

        private void LoadNextTooltip(string nextGunkTag, ProcedureStep step)
        {
            ToolTipConfig toolTipConfig = NohoConfigResolver.GetConfig<ToolTipConfig>(nextGunkTag);

            if (toolTipConfig == null)
            {
                return;
            }

            var speakerDef = toolTipConfig.GetSpeaker(OperationSpeakerDevName);

            ToolTipProcedureStepsDef currentStepDef = null;
            foreach (ToolTipProcedureStepsDef stepsDef in speakerDef.Steps)
            {
                if(stepsDef.StepId == (int) step)
                {
                    currentStepDef = stepsDef;
                }
            }

            if (currentStepDef == null)
            {
                ZBug.Warn("TOOLTIP", $"Unknown step: {step} for tag: {nextGunkTag}");
                return;
            }

            CurrentGunkTag = nextGunkTag;
            
            Send.Msg(new NewToolTipMsg
            {
                ToolTip = currentStepDef.Dialogue
            });
        }

        public string CurrentGunkTag;

        private void OnNewToolTip(NewToolTipMsg message)
        {
            CanvasGroup.alpha = 1;
            HelpText.text = message.ToolTip;
        }

        public void OnDestroy()
        {
            UnInit();
        }
    }
}