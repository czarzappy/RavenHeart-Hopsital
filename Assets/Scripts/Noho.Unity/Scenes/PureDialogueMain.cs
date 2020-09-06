using System.Collections.Generic;
using System.Linq;
using Noho.Configs;
using Noho.Managers;
using Noho.Messages;
using Noho.Parsing.Models;
using Noho.Unity.Components;
using Noho.Unity.Managers;
using Noho.Unity.Messages;
using Noho.Unity.Models;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using ZEngine.Core.Dialogue.Models;
using ZEngine.Core.Utils;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;
using Image = UnityEngine.UI.Image;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Noho.Unity.Scenes
{
    public class PureDialogueMain : MonoBehaviour, IPointerClickHandler
    {
        public TextAsset FileAsset;

        public StagePosController StagePosController;
        // public Image CharacterImage;

        public DialogueController DialogueController;
        
        public Image BackgroundImage;
        public AudioSource BackgroundMusic;
        public AudioSource VO;

        public Transform SceneIntroContainer;
        public TMP_Text SceneTitle;
        public TMP_Text SceneDescription;

        private Context mContext;
        
        public void Start()
        {
            if (!HasInit)
            {
                Init();
            }
        }

        private string mSurgeryScene;

        public bool HasInit = false;
        public void Init()
        {
            mAcceptUserInput = false;
            mSurgeryScene = SceneManager.GetActiveScene().name;
            NextSurgeryScene = mSurgeryScene;
            HasInit = true;
            // OnNewScene();
            MsgMgr.Instance.SubscribeTo<CutSceneLoadedMsg>(OnCutSceneLoaded);
            MsgMgr.Instance.SubscribeTo<NewStageActionLoadedMsg>(OnNewStageActionLoaded);
            MsgMgr.Instance.SubscribeTo<LoadNewSurgerySceneMsg>(OnLoadNewSurgeryScene);
            MsgMgr.Instance.SubscribeTo<ChoiceSelectedMsg>(OnChoiceSelected);
            
            if (BrainMain.Instance == null)
            {
                mContext = new Context();
                mContext.Init();

                var lines = LineUtil.GetLines(FileAsset.text).AsParserLines();
                
                var scenes = App.Instance.ScriptParser.Deserialize<CutSceneDef>(lines);

                mContext.ScriptManager.LoadItems(scenes);
                // BrainMain.Instance.Context.SceneManager.LoadScenes(scenes.ToList());
                
                mContext.ScriptManager.MoveNextScriptItem();
            }
            else
            {
                mContext = BrainMain.Instance.Context;
            }
            
            StagePosController.Init();
            
            DialogueController.Init(StagePosController);
        }

        public void OnSkipClick()
        {
            BrainMain.Instance.HandleSceneSkip(false);
        }

        private void OnChoiceSelected(ChoiceSelectedMsg message)
        {
            mContext.SceneManager.AddSAM(message.SAM);

            // first action is the protag's response
            mContext.SceneManager.MoveNextAction();
            
            // second is the follow up
            mContext.SceneManager.MoveNextAction();
        }

        public void OnDestroy()
        {
            UnInit();
        }
        private string NextSurgeryScene;
        public bool HandleCallbacks
        {
            get { return NextSurgeryScene == mSurgeryScene; }
        }
        private void OnLoadNewSurgeryScene(LoadNewSurgerySceneMsg message)
        {
            NextSurgeryScene = message.SceneName;
        }

        public void UnInit()
        {
            MsgMgr.Instance.UnsubscribeFrom<CutSceneLoadedMsg>(OnCutSceneLoaded);
            MsgMgr.Instance.UnsubscribeFrom<NewStageActionLoadedMsg>(OnNewStageActionLoaded);
            MsgMgr.Instance.UnsubscribeFrom<LoadNewSurgerySceneMsg>(OnLoadNewSurgeryScene);
            
            MsgMgr.Instance.UnsubscribeFrom<ChoiceSelectedMsg>(OnChoiceSelected);
            
            DialogueController.UnInit();
        }

        public void OnCutSceneLoaded(CutSceneLoadedMsg msg)
        {
            if (!HandleCallbacks)
            {
                return;
            }
            
            CutSceneDef scene = msg.CutScene;
            // CutSceneDef scene = Context.SceneManager.NextScene();
            
            ZBug.Info("PURE-DIALOGUE",$"[NEW-SCENE] {scene.SceneTitle} - {scene.SceneDescription}");
            
            // setup the stage
            SceneTitle.text = scene.SceneTitle;
            SceneDescription.text = scene.SceneDescription;
            
            SceneIntroContainer.Show();
            
            StagePosController.Init();
            
            DialogueController.Init(StagePosController);
            
            // Start the pump
            mContext.SceneManager.MoveNextAction();
        }

        private bool mAcceptUserInput;
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.IsScrolling () || eventData.dragging) 
            {    
                return;
            }
            
            OnUserInput();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnUserInput();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // Send.Msg(new );
            }
        }

        private void OnUserInput()
        {
            if (!mAcceptUserInput)
            {
                return;
            }
            
            mContext.SceneManager.MoveNextAction();
        }
        
        private void OnNewStageActionLoaded(NewStageActionLoadedMsg message)
        {
            if (!HandleCallbacks)
            {
                return;
            }
            
            OnNextActionRequest(message.NewStageAction);
        }

        private string mLastSpeakingCharacterDevName;
        private string mLastExpressingCharacterDevName;
        private string mDefaultLoveFocus;

        private void OnNextActionRequest(StageAction stageAction)
        {
            ZBug.Info("Dialogue", $"New stage action: {stageAction}");
            SceneIntroContainer.Hide();

            bool shouldBlock = false;
            switch (stageAction.Type)
            {
                case StageActionType.CHOICE:

                    DialogueController.HandleChoices(stageAction.Choices, stageAction.NumberOfChoicesToSelect);
                    shouldBlock = true;
                    mAcceptUserInput = false;
                    break;
                
                case StageActionType.BREAK:

                    mContext.SceneManager.PopSam();

                    break;
                
                case StageActionType.DIALOGUE:

                    string speakingCharacterDevName = Current(ref mLastSpeakingCharacterDevName, stageAction.CharacterDevName);
                    
                    DialogueController.HandleDialogue(speakingCharacterDevName, stageAction.DialogueText);

                    shouldBlock = true;
                    mAcceptUserInput = true;
                    break;
                
                case StageActionType.MOVE:
                    mLastSpeakingCharacterDevName = stageAction.CharacterMoves.Last().CharacterDevName;
                    StagePosController.HandleMoves(stageAction.CharacterMoves);
                    break;
                    
                case StageActionType.EXPRESSION:
                    string expressingCharacterDevName = Current(ref mLastExpressingCharacterDevName, stageAction.CharacterDevName);
                    
                    StagePosController.HandleExpression(expressingCharacterDevName, stageAction.ExpressionType);
                    break;
                    
                case StageActionType.NEW_CHARACTER_STORYLINE:
                    AppManager.Instance.UnlockNextEpisode(stageAction.CharacterDevName);
                    break;
                
                case StageActionType.BACKGROUND:
                    BackgroundImage.sprite = ZSource.Load<Sprite>(NohoResourcePack.FOLDER_BG, stageAction.Asset);
                    break;
                
                case StageActionType.BACKGROUND_MUSIC:
                    BackgroundMusic.clip = ZSource.Load<AudioClip>(NohoResourcePack.FOLDER_BGM, stageAction.Asset);
                    BackgroundMusic.Play();
                    break;
                
                case StageActionType.VO:
                    VO.clip = ZSource.Load<AudioClip>("VO", stageAction.Asset);
                    VO.Play();
                    break;
                
                case StageActionType.LOVE_FOCUS:
                    ZBug.Info($"love focus: {stageAction.CharacterDevName}");

                    mDefaultLoveFocus = stageAction.CharacterDevName;
                    break;

                case StageActionType.LOVE_DELTA:
                {
                    var currentLoveFocus = stageAction.CharacterDevName ?? mDefaultLoveFocus;
                    
                    App.Instance.PersistentData.IncrementLove(currentLoveFocus, stageAction.LoveAmount);
                    
                    Send.Msg(new CharacterLoveChangedMsg
                    {
                        CharacterDevName = currentLoveFocus,
                        LoveDelta = stageAction.LoveAmount
                    });
                    
                    ZBug.Info($"love: {currentLoveFocus}, {stageAction.LoveAmount}");

                    break;
                }

                case StageActionType.LOVE_MIN:
                {
                    var currentLoveFocus = stageAction.CharacterDevName ?? mDefaultLoveFocus;
                    
                    if (App.Instance.PersistentData.GetCurrentLoveBy(currentLoveFocus) < stageAction.LoveAmount)
                    {
                        ZBug.Info("Not enough love, skipping");
                        break;
                    }

                    
                    ZBug.Info("Enough love!");
                    
                    StageActionManager sam = new StageActionManager();

                    var actions = stageAction.TrueBranch.Select((o => (StageAction) o)).ToList();
                    
                    sam.LoadItems(actions);
                    mContext.SceneManager.AddSAM(sam);
                    
                    break;
                }
                
                case StageActionType.SET_FLAG:

                    App.Instance.RuntimeData.SetFlag(stageAction.FlagTag, stageAction.FlagValue);
                    
                    break;

                case StageActionType.CHECK_FLAG:
                {
                    if (!App.Instance.RuntimeData.CheckFlag(stageAction.FlagTag, stageAction.FlagValue))
                    {
                        ZBug.Info("PURE-DIALOGUE", $"Flag {stageAction.FlagTag} not value: {stageAction.FlagValue}");
                        break;
                    }

                    StageActionManager sam = new StageActionManager();

                    var actions = stageAction.TrueBranch.Select(o => (StageAction) o).ToList();
                    
                    sam.LoadItems(actions);
                    mContext.SceneManager.AddSAM(sam);
                    
                    break;
                }
                    
                default:
                    ZBug.Error("PURE-DIALOGUE",$"Unhandled stage action: {stageAction.Type}");
                    break;
            }

            if (shouldBlock)
            {
                return;
            }

            mContext.SceneManager.MoveNextAction();
        }

        private string Current(ref string last, string next)
        {
            string result = next;
            if (result == null)
            {
                result = last;
            }
            else
            {
                last = result;
            }

            return result;
        }
    }
}