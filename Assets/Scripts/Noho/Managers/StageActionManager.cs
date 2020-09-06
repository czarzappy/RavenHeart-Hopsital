using System.Collections.Generic;
using Noho.Messages;
using Noho.Parsing.Models;
using ZEngine.Core.Dialogue.Models;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Managers
{
    public class StageActionManager
    {
        private List<StageAction>.Enumerator mActions; 

        public void LoadItems(List<StageAction> currentSceneActions)
        {
            mActions = currentSceneActions.GetEnumerator();
        }

        public bool MoveNextAction()
        {
            ZBug.Info("STAGE ACTION", "Moving to next action");
            if (mActions.MoveNext())
            {
                Send.Msg(new NewStageActionLoadedMsg
                {
                    NewStageAction = mActions.Current
                });

                return true;
            }
            else
            {
                ZBug.Warn("STAGE ACTION", "Complete!");
                return false;
            }
        }
    }
}