using System.Collections.Generic;
using Noho.Parsing.Factories;
using Noho.Parsing.Models;
using Noho.Unity.Messages;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Components
{
    public class ChoiceContainerController : MonoBehaviour
    {
        public ChoiceController Template;

        public LayoutGroup LayoutGroup;
        
        public List<ChoiceController> Choices = new List<ChoiceController>();

        public void Init()
        {
            MsgMgr.Instance.SubscribeTo<SAMCompletedMsg>(OnSAMCompleted);
            MsgMgr.Instance.SubscribeTo<ChoiceSelectedMsg>(OnChoiceSelected);
        }

        public void OnDestroy()
        {
            UnInit();
        }

        public void UnInit()
        {
            MsgMgr.Instance.UnsubscribeFrom<SAMCompletedMsg>(OnSAMCompleted);
            MsgMgr.Instance.UnsubscribeFrom<ChoiceSelectedMsg>(OnChoiceSelected);
        }

        private void OnSAMCompleted(SAMCompletedMsg message)
        {
            // if (message.SAM)
            // {
            //     
            // }
        }


        public void Update()
        {
            // if (Input.GetKeyDown(KeyCode.N))
            // {
            //     AddChoice();
            // }
        }

        public void UpdateChoices()
        {
            
        }

        public int NumberOfChoicesToSelect;

        public static StageBranchDef Copy(StageBranchDef stageBranchDef)
        {
            var result = new StageBranchDef();

            foreach (var action in stageBranchDef.Actions)
            {
                result.Actions.Add(action);
            }

            return result;
        }
        public void Load(List<StageBranchDef> choiceDefs, int numberOfChoicesToSelect)
        {
            NumberOfChoicesToSelect = numberOfChoicesToSelect;
            int missingCount = choiceDefs.Count - Choices.Count;
            ZBug.Info("CHOICES", $"Handling choices, missing: {missingCount}");
            foreach (ChoiceController choice in Choices)
            {
                choice.Hide();
            }

            while (missingCount > 0)
            {
                AddChoice();
                missingCount--;
            }

            for (int choiceIdx = 0; choiceIdx < choiceDefs.Count; choiceIdx++)
            {
                // copy branch and add new choice action
                // Don't mutate original branch
                StageBranchDef choiceDef = Copy(choiceDefs[choiceIdx]);

                // if there are more choices to select, generate a new action that is a subset of the remaining choices
                if (numberOfChoicesToSelect > 1)
                {
                    var subChoice = new ActionChoiceDef
                    {
                        NumberOfChoicesToSelect = numberOfChoicesToSelect - 1,
                    };

                    for (int otherChoiceIdx = 0; otherChoiceIdx < choiceDefs.Count; otherChoiceIdx++)
                    {
                        if (otherChoiceIdx == choiceIdx)
                        {
                            continue;
                        }
                        StageBranchDef otherChoiceDef = choiceDefs[otherChoiceIdx];

                        subChoice.Choices.Add(otherChoiceDef);
                    }
                    
                    choiceDef.Actions.Add(StageActionFactory.NewChoicesAction(subChoice));
                }

                var choiceUI = Choices[choiceIdx];
                choiceUI.Show();
                choiceUI.Init(choiceDef, choiceIdx);
            }
        }

        private void OnChoiceSelected(ChoiceSelectedMsg message)
        {
            var test = transform.GetChild(message.ChoiceIdx).GetComponent<ChoiceController>();
            test.DisableChoice();

            // throw new NotImplementedException();
        }

        private void AddChoice()
        {
            ChoiceController choice = Instantiate(Template, LayoutGroup.transform, true);

            choice.Hide();
            Choices.Add(choice);
        }
    }
}