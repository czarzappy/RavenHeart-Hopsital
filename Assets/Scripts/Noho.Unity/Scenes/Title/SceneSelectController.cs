using Noho.Parsing.Models;
using Noho.Unity.Models;
using UnityEngine;
using ZEngine.Core.Dialogue.Models;
using ZEngine.Core.Utils;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Models;

namespace Noho.Unity.Scenes.Title
{
    public class SceneSelectController : UIMonoBehaviour
    {
        public ScenePanelController Template;

        public Sprite DefaultOperationSprite;
        public Sprite DefaultCutSceneSprite;
        
        public Transform PanelContainer;
        
        public void Awake()
        {
            Init();
        }

        public void Init()
        {
            
            var lines = LineUtil.GetLines(App.Instance.RuntimeData.PlayingEpisodeAsset.text).AsParserLines();
            var scriptItems = App.Instance.ScriptParser.Deserialize(lines);


            int scriptIdx = 0;
            // int number = 1;
            foreach (var item in scriptItems)
            {
                Sprite previewSprite = null;
                switch (item)
                {
                    case CutSceneDef cutSceneDef:
                        foreach (StageAction stageAction in cutSceneDef.Actions)
                        {
                            if (stageAction.Type == StageActionType.BACKGROUND)
                            {
                                ResourcePath resourcePath = new ResourcePath(NohoResourcePack.FOLDER_BG, stageAction.Asset);

                                previewSprite = ZSource.Load<Sprite>(resourcePath);
                                break;
                            }
                        }

                        if (previewSprite == null)
                        {
                            previewSprite = DefaultCutSceneSprite;
                        }
                        break;
                    
                    case OperationDef operationDef:

                        previewSprite = DefaultOperationSprite;
                        break;
                    
                    
                }
                var panel = Instantiate(Template, PanelContainer);
            
                panel.Init($"Scene {scriptIdx + 1}", previewSprite, scriptIdx);
            
                scriptIdx++;
                panel.Show();

                // Only showing first scene for now
                break;
            }
        }
    }
}