using System.Collections.Generic;
using Noho.Messages;
using Noho.Parsing.Models;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Managers
{
    public class SceneManager
    {
        // private StageActionManager mStageActionManager;
        //
        // public StageActionManager StageActionManager => mStageActionManager;

        private readonly Stack<StageActionManager> mSAMStack = new Stack<StageActionManager>();

        public void Init()
        {
        }

        public void LoadItem(CutSceneDef newCutScene)
        {
            ZBug.Info("SCENE", $"Loading new cutscene: {newCutScene}");

            var sam = new StageActionManager();
            sam.LoadItems(newCutScene.Actions);
            
            mSAMStack.Clear();
            mSAMStack.Push(sam);
        }

        public void AddSAM(StageActionManager sam)
        {
            mSAMStack.Push(sam);
        }
        
        public void PopSam()
        {
            mSAMStack.Pop();
        }
        
        public void MoveNextAction()
        {
            if (mSAMStack.Count == 0)
            {
                ZBug.Warn("SCENE", "Cutscene complete");
                Send.Msg(new CutSceneCompletedMsg());
                return;
            }
            
            while (!mSAMStack.Peek().MoveNextAction())
            {
                PopSam();
                
                if (mSAMStack.Count == 0)
                {
                    ZBug.Warn("SCENE", "Cutscene complete");
                    Send.Msg(new CutSceneCompletedMsg());
                    return;
                }
                else
                {
                    ZBug.Warn("SCENE", "Finished choice");
                    
                }
            }
        }
    }
}
