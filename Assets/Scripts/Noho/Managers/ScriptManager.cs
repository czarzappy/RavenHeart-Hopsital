using System.Collections.Generic;
using System.Linq;
using Noho.Messages;
using Noho.Models;
using Noho.Parsing.Models;
using ZEngine.Unity.Core.Analytics;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Managers
{
    public class ScriptManager
    {
        private int mScriptItemIdx = -1;
        public List<object> ScriptItems;

        public void Init()
        {
        }

        public void LoadItems(IEnumerable<object> scriptItems)
        {
            ScriptItems = scriptItems.ToList();
        }

        public object CurrentItem => ScriptItems[mScriptItemIdx];

        public bool MoveNext()
        {
            if (mScriptItemIdx + 1 >= ScriptItems.Count)
            {
                return false;
            }
            
            mScriptItemIdx++;
            return true;
        }

        public bool MoveNextScriptItem()
        {
            while (true)
            {
                if (!MoveNext())
                {
                    ZBug.Warn("SCRIPT", $"Script complete! last item: {CurrentItem}, type: {CurrentItem.GetType()}, idx: {mScriptItemIdx}");
                    return false;
                }

                ZAnalytics.Instance.OnScriptItemStarted(mScriptItemIdx);
                
                ZBug.Info("SCRIPT", $"Current script item: {CurrentItem}, type: {CurrentItem.GetType()}, idx: {mScriptItemIdx}");

                switch (CurrentItem)
                {
                    case CutSceneDef cutSceneDef:
                        ZBug.Info("SCRIPT", $"Starting new cutscene: {cutSceneDef}");
                        Send.Msg(new LoadNewCutSceneMsg {NewCutScene = cutSceneDef});
                        break;

                    case OperationDef operationDef:
                        ZBug.Info("SCRIPT", $"Starting new operation: {operationDef}");
                        ZBug.Info("SCRIPT", $"Adding Post op def at idx: {mScriptItemIdx + 1}");
                        ScriptItems.Insert(mScriptItemIdx + 1, new PostOpDef());
                        Send.Msg(new LoadNewOperationSceneMsg {NewOperationScene = operationDef});
                        break;

                    case BriefingDef briefingDef:
                        ZBug.Info("SCRIPT", $"Would have tried to load briefing: {briefingDef}");
                        // skip known unhandled types
                        continue;
                    
                    case PostOpDef postOpDef:
                        ZBug.Info("SCRIPT", $"Starting new post op");
                        Send.Msg(new LoadPostOpSceneMsg{PostOpDef = postOpDef});
                        break;

                    default:
                        ZBug.Warn("SCRIPT", $"Unhandled item type: {CurrentItem}, {CurrentItem?.GetType()}");
                        return false;
                }

                return true;
            }
        }
    }
}