using Noho.Messages;
using Noho.Parsing.Models;
using Noho.Unity.Managers;
using Noho.Unity.Messages;
using Noho.Unity.Models;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ZEngine.Core.Dialogue.Models;
using ZEngine.Core.Utils;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes
{
    public partial class BrainMain : MonoBehaviour
    {
        private enum SceneType
        {
            NONE,
            CUTSCENE,
            SURGERY,
            POSTOP
        }
        
        public static BrainMain Instance { get; private set; }
        
        public Context Context;
        public Image BlackImage;
        public Camera LoadingCamera;
        public TextAsset FileAsset;

        public AudioSource OperationMusic;
        public AudioListener LoadingAudioListener;


        // private string mLastLoadedScene;
        private string mLastLoadedSurgeryScene;
        private string mOverlayScene;
        private SceneType mCurrentOverlaySceneType = SceneType.NONE;

        #region Properties

        private bool IsSkipping => App.Instance.RuntimeData.PlayingEpisodeSceneIdx > 0;

        #endregion

        public void Start()
        {
            Init();
        }

        public void OnDestroy()
        {
            UnInit();
        }

        private void UnInit()
        {
            MsgMgr.Instance.UnsubscribeFrom<LoadNewSurgerySceneMsg>(OnLoadNewSurgerySurgeryScene);
            MsgMgr.Instance.UnsubscribeFrom<LoadNewCutSceneMsg>(OnLoadNewCutScene);
            MsgMgr.Instance.UnsubscribeFrom<LoadNewOperationSceneMsg>(OnLoadNewOperationScene);
            MsgMgr.Instance.UnsubscribeFrom<CutSceneCompletedMsg>(OnCutSceneCompleted);
            MsgMgr.Instance.UnsubscribeFrom<OperationCompletedMsg>(OnOperationCompleted);
            MsgMgr.Instance.UnsubscribeFrom<OperationLoadedMsg>(OnOperationLoaded);
            MsgMgr.Instance.UnsubscribeFrom<PostOpCompleteMsg>(OnPostOpComplete);
            MsgMgr.Instance.UnsubscribeFrom<LoadPostOpSceneMsg>(OnLoadPostOpScene);
        }

        private void Init()
        {
            ZBug.Info("BRAIN", "Init");
            
            Instance = this;
            
            Context = new Context();
            Context.Init();
            
            MsgMgr.Instance.SubscribeTo<LoadNewSurgerySceneMsg>(OnLoadNewSurgerySurgeryScene);
            MsgMgr.Instance.SubscribeTo<LoadNewCutSceneMsg>(OnLoadNewCutScene);
            MsgMgr.Instance.SubscribeTo<LoadNewOperationSceneMsg>(OnLoadNewOperationScene);
            MsgMgr.Instance.SubscribeTo<CutSceneCompletedMsg>(OnCutSceneCompleted);
            MsgMgr.Instance.SubscribeTo<OperationCompletedMsg>(OnOperationCompleted);
            MsgMgr.Instance.SubscribeTo<OperationLoadedMsg>(OnOperationLoaded);
            MsgMgr.Instance.SubscribeTo<PostOpCompleteMsg>(OnPostOpComplete);
            MsgMgr.Instance.SubscribeTo<LoadPostOpSceneMsg>(OnLoadPostOpScene);

            TextAsset playing = App.Instance.RuntimeData.PlayingEpisodeAsset;

            if (playing == null)
            {
                playing = FileAsset;

                App.Instance.RuntimeData.PlayingEpisodeAsset = FileAsset;
            }


            if (playing == null)
            {
                ZBug.Warn("BRAIN", "No episode defined to play");
                return;
            }
            
            ZBug.Info("BRAIN", $"Playing Episode: {playing.name}");
            var lines = LineUtil.GetLines(playing.text).AsParserLines();
            var scriptItems = App.Instance.ScriptParser.Deserialize(lines);

            Context.ScriptManager.LoadItems(scriptItems);
                
            ZBug.Info("BRAIN", $"Jumping forward to scene: {App.Instance.RuntimeData.PlayingEpisodeSceneIdx}");

            while (IsSkipping)
            {
                ZBug.Warn("BRAIN", "Skipping!");
                LoadNextScriptItem();
                App.Instance.RuntimeData.PlayingEpisodeSceneIdx--;
            }
            
            // Actually starts the scene
            LoadNextScriptItem();
        }

        public void FixedUpdate()
        {
            MsgMgr.Instance.Pump();
        }

        public void Update()
        {
            if (Input.GetKey(KeyCode.Escape) ||
                Input.GetKey("escape"))
            {
                AppManager.Instance.GoToTitleScene();
                // App.Instance.Quit();
            }
            
            bool isShift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            
            if (Input.GetKeyDown(KeyCode.Return))
            {
                HandleSceneSkip(isShift);
            }
        }

        public void HandleSceneSkip(bool shift)
        {
            switch (Context.ScriptManager.CurrentItem)
            {
                case OperationDef _:

                    if (shift)
                    {
                        StartCoroutine(Context.OperationManager.MoveNext());
                        break;
                    }
                    
                    goto default;
                    
                default:
            
                    LoadNextScriptItem();
                    break;
            }
        }

        public void LoadNextScriptItem()
        {
            ZBug.Warn("BRAIN", "loading next script item");
            OperationMusic?.Stop();
            if (!Context.ScriptManager.MoveNextScriptItem())
            {
                ZBug.Warn("BRAIN", $"Script done!");

                AppManager.Instance.FinishEpisode();
            }
        }
    }
}