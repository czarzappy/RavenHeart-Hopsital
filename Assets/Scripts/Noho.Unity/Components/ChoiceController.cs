using System.Collections.Generic;
using System.Linq;
using Noho.Managers;
using Noho.Models;
using Noho.Parsing.Models;
using Noho.Unity.Messages;
using Noho.Unity.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core.Analytics;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Components
{
    public class ChoiceController : MonoBehaviour
    {
        public Button Button;
        public Image ChoiceBackground;
        public TMP_Text ChoiceText;

        public string Expression;

        private StageBranchDef mChoiceDef;

        private int mChoiceIdx;

        
        public void Init(StageBranchDef choiceDef, int choiceIdx)
        {
            mChoiceDef = choiceDef;
            mChoiceIdx = choiceIdx;
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(OnButtonClick);

            bool isValidOption = true;
            
            mActionStack.Clear();
            Enumerable.Reverse(choiceDef.Actions).ForEach(mActionStack.Push);
            
            while (true)
            {
                StageAction action = Next();
                bool done = false;
                switch (action.Type)
                {
                    case StageActionType.DIALOGUE:
                        ChoiceText.text = action.DialogueText.SanitizeProtag();
                        done = true;
                        break;
                    
                    case StageActionType.EXPRESSION:
                        Expression = action.ExpressionType;
                        break;
                    
                    case StageActionType.CHECK_FLAG:

                        if (!App.Instance.RuntimeData.CheckFlag(action.FlagTag, action.FlagValue))
                        {
                            done = true;
                            isValidOption = false;
                        }
                        
                        Enumerable.Reverse(action.TrueBranch)
                            .Cast<StageAction>()
                            .ForEach(mActionStack.Push);
                        
                        break;
                }

                if (done)
                {
                    break;
                }
            }

            if (!isValidOption)
            {
                gameObject.Hide();
            }
        }

        private readonly Stack<StageAction> mActionStack = new Stack<StageAction>();
        public StageAction Next()
        {
            return mActionStack.Pop();
        }

        public void DisableChoice()
        {
            Button.interactable = false;
        }

        public void OnDestroy()
        {
            UnInit();
        }

        public void UnInit()
        {
            Button.onClick.RemoveListener(OnButtonClick);
        }

        public void OnButtonClick()
        {
            ZBug.Info("CHOICE", $"Button has been clicked, choice: {mChoiceDef}");
            
            StageActionManager choiceActionManager = new StageActionManager();
            choiceActionManager.LoadItems(mChoiceDef.Actions);

            ZAnalytics.Instance.OnChoiceSelected(mChoiceIdx, ChoiceText.text, Expression);
            
            Send.Msg(new ChoiceSelectedMsg
            {
                ChoiceIdx = mChoiceIdx,
                SAM = choiceActionManager
            });
        }
    }
}