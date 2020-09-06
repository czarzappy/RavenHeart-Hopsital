using System.Collections.Generic;
using System.IO;
using Noho.Extensions;
using Noho.Messages;
using Noho.Models;
using Noho.Parsing.Models;
using Noho.Parsing.Parsers;
using Noho.Unity.Components;
using Noho.Unity.Messages;
using Noho.Unity.Models;
using Noho.Unity.Scenes.Surgery;
using Noho.Unity.Scenes.Surgery.Gunk;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ZEngine.Core.Dialogue.Models;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes
{
    public partial class OperationMain : MonoBehaviour, IPointerDownHandler
    {
        public TextAsset SceneData;
        
        public GunkController GunkController;
        public StagePosController StagePosController;

        public DialogueController DialogueController;

        public ToolboxController ToolboxController;
        public ToolbarController ToolbarController;

        public Button SaveButton;

        [FormerlySerializedAs("ScrollController")] public OperationBackgroundController OperationBackgroundController;

        public Image Anatomy;

        public bool mFailed = false;
        public bool mLoading = false;
        public FailWin FailWin;

        public VitalController VitalController;

        public AudioSource VO;

        private string mLastCharacterDevName;
        
        private bool mOnlyDialoguePhases = true;

        // private bool IgnoreFirstInput;

        public string SurgeryScene;

        // prevent user input for progressing actions when phase is already fully progressed
        private bool mExpectingUserInput = false;

        private string mNextSurgeryScene;

        private float mLastTickTime;
        public bool HandleCallbacks => mNextSurgeryScene == SurgeryScene;
        
        // public 
        
        public void Awake()
        {
            if (BrainMain.Instance == null)
            {
                TextReader reader = new StringReader(SceneData.text);
                
                SceneDef sceneDef = SceneConvert.FromTextReader(reader);
            
                SaveButton.onClick.AddListener(Save);

                Init(sceneDef);
                
                // GunkController.guni
                
                // Init(sceneDef.Name);
            }
            else
            {
                SaveButton.Hide();
            }
        }
        
        public void Init(SceneDef sceneDef)
        {
            mLoading = false;
            mLastTickTime = Time.time;
            mExpectingUserInput = false;
            // mSurgeryScene = SceneManager.GetActiveScene().name;
            SurgeryScene = sceneDef.Name;
            mNextSurgeryScene = SurgeryScene;
            // handle event registering
            MsgMgr.Instance.SubscribeTo<ClearedAllGunkMsg>(OnClearedAllGunk);
            MsgMgr.Instance.SubscribeTo<NewStageActionLoadedMsg>(OnNewStageActionLoaded);
            MsgMgr.Instance.SubscribeTo<PhaseSetupCompleted>(OnPhaseSetupCompleted);
            MsgMgr.Instance.SubscribeTo<LoadNewSurgerySceneMsg>(OnLoadNewSurgeryScene);
            MsgMgr.Instance.SubscribeTo<OperationFailedMsg>(OnOperationFailed);
            MsgMgr.Instance.SubscribeTo<ChoiceSelectedMsg>(OnChoiceSelected);

            
            Load(sceneDef);
            // handle UI bindings
            GunkController.Init();
            
            StagePosController.Init();
            
            DialogueController.Init(StagePosController);
            
            FailWin.Hide();

            // ResourcePath path = new ResourcePath("Surgery", "Scenes", SceneName);
            // TextAsset textAsset = ZSource.Load<TextAsset>(path);

            
            // last thing
            // start the pump going
            if (BrainMain.Instance != null)
            {
                ZBug.Info($"[Init] {this}, move to next action");
                BrainMain.Instance.Context.OperationManager.MoveNextAction();

                InvokeRepeating(nameof(Tick), NohoUISettings.OPERATION_TICK_DELAY, NohoUISettings.OPERATION_TICK_DURATION);
            }
        }

        private void OnChoiceSelected(ChoiceSelectedMsg message)
        {
            // BrainMain.Instance.Context.OperationManager.CurrentPatientManager.PhaseManager.SceneManager.AddSAM(message.SAM);

            // first action is the protag's response
            // BrainMain.Instance.Context.OperationManager.CurrentPatientManager.PhaseManager.SceneManager.MoveNextAction();
            
            // second is the follow up
            // BrainMain.Instance.Context.OperationManager.CurrentPatientManager.PhaseManager.SceneManager.MoveNextAction();
        }
        
        

        private void OnOperationFailed(OperationFailedMsg message)
        {
            mFailed = true;
            FailWin.Show();
        }

        public string SurgerySceneName
        {
            get
            {
                if (SceneData == null)
                {
                    return SurgeryScene;
                }

                return SceneData.name;
            }
        }

        public void OnDestroy()
        {
            UnInit();
        }

        private void UnInit()
        {
            MsgMgr.Instance.UnsubscribeFrom<ClearedAllGunkMsg>(OnClearedAllGunk);
            MsgMgr.Instance.UnsubscribeFrom<NewStageActionLoadedMsg>(OnNewStageActionLoaded);
            MsgMgr.Instance.UnsubscribeFrom<PhaseSetupCompleted>(OnPhaseSetupCompleted);
            MsgMgr.Instance.UnsubscribeFrom<LoadNewSurgerySceneMsg>(OnLoadNewSurgeryScene);
            MsgMgr.Instance.UnsubscribeFrom<OperationFailedMsg>(OnOperationFailed);
            MsgMgr.Instance.UnsubscribeFrom<PhaseCompletedMsg>(OnPhaseCompleted);
            MsgMgr.Instance.UnsubscribeFrom<ChoiceSelectedMsg>(OnChoiceSelected);
            
            DialogueController.UnInit();
            
            if (BrainMain.Instance != null)
            {
                CancelInvoke();
            }
        }

        private void OnPhaseCompleted(PhaseCompletedMsg message)
        {
            mLoading = true;
        }

        private void OnLoadNewSurgeryScene(LoadNewSurgerySceneMsg message)
        {
            mNextSurgeryScene = message.SceneName;
        }

        private void OnPhaseSetupCompleted(PhaseSetupCompleted message)
        {
            if (!HandleCallbacks)
            {
                return;
            }
            
            if (mOnlyDialoguePhases)
            {
                ResetAndMoveToNextPhase();
            }
            else
            {
                ZBug.Warn($"Restricting user action input for phase progression");
                DialogueController.Hide();
                StagePosController.Hide();
                
                ToolboxController.Show();
                ToolbarController.Show();
                GunkController.EnableRaycasts();
            }
        }

        private void OnClearedAllGunk(ClearedAllGunkMsg message)
        {
            ZBug.Info("OPERATION", "Cleared all gunk");
            if (!HandleCallbacks)
            {
                ZBug.Warn("OPERATION", "Not handling callbacks right now");
                return;
            }
            
            ResetAndMoveToNextPhase();
        }

        private void ResetAndMoveToNextPhase()
        {
            if (!HandleCallbacks)
            {
                return;
            }
            
            GunkController.FirstGunkTip = true;
            ZBug.Info("OPERATION", $"[ResetAndMoveToNextPhase] Reset and move to next phase");
            mOnlyDialoguePhases = true;
            mExpectingUserInput = false;

            StartCoroutine(BrainMain.Instance.Context.OperationManager.MoveNext());
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ZBug.Info("POINTER", "Down");
            // ZBug.Info($"[OnPointerDown] Move to next action: {eventData.clickTime}, time: {Time.time}");
            HandleUserInput();
        }

        public void Tick()
        {
            float currentTickTime = Time.time;
            if (mExpectingUserInput || mFailed || mLoading)
            {
                mLastTickTime = currentTickTime;
                return;
            }

            float deltaTime = currentTickTime - mLastTickTime;

            BrainMain.Instance.Context.OperationManager.Tick(deltaTime);

            mLastTickTime = currentTickTime;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HandleUserInput();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                ZBug.Info("SCORE", "Keydown!");
                Send.Msg(new EarnScoreMsg
                {
                    ScoreDelta = 100,
                });
            }
            
            
            if (mExpectingUserInput || mFailed || mLoading)
            {
                return;
            }

            GunkController.Tick();
            // if (!mExpectingUserInput)
            // {
            //     if (Input.GetKeyDown(KeyCode.RightArrow))
            //     {
            //         ZBug.Info("OPERATION", "Shifting to right patient");
            //         BrainMain.Instance.Context.OperationManager.ShiftActivePatient(1);
            //     }
            //
            //     if (Input.GetKeyDown(KeyCode.LeftArrow))
            //     {
            //         ZBug.Info("OPERATION", "Shifting to left patient");
            //         BrainMain.Instance.Context.OperationManager.ShiftActivePatient(-1);
            //     }
            // }
        }

        public void HandleUserInput()
        {
            if (!mExpectingUserInput)
            {
                ZBug.Warn("OPERATION", "Not accepting user input now");
                return;
            }

            mExpectingUserInput = false;
            
            ZBug.Info("OPERATION", "Handling user input");
            BrainMain.Instance.Context.OperationManager.CurrentPatientManager.MoveNextAction();
        }
        
        private void OnNewStageActionLoaded(NewStageActionLoadedMsg message)
        {
            if (!HandleCallbacks)
            {
                return;
            }
            
            OnNextActionRequest(message.NewStageAction);
        }
        
        private void OnNextActionRequest(StageAction stageAction)
        {
            ZBug.Info("Operation", $"Next stage action: {stageAction}");
            string characterDevName = stageAction.CharacterDevName;
            if (characterDevName == null)
            {
                characterDevName = mLastCharacterDevName;
            }
            else
            {
                mLastCharacterDevName = characterDevName;
            }

            bool shouldBlock = false;
            switch (stageAction.Type)
            {
                case StageActionType.VO:
                    VO.clip = ZSource.Load<AudioClip>("VO", stageAction.Asset);
                    VO.Play();
                    break;
                
                case StageActionType.DIALOGUE:
                    
                    ToolboxController.Hide();
                    ToolbarController.Hide();
                    GunkController.DisableRaycasts();
                    
                    // Debug.LogWarning($"[DIALOGUE] {characterDevName} - {stageAction.DialogueText}");
                    // DialogueController.HandleDialogue(characterDevName, stageAction.DialogueText);

                    switch (characterDevName)
                    {
                        case NohoParsingConstants.CHARACTER_DEV_NAME_PATIENT:
                        case NohoParsingConstants.CHARACTER_DEV_NAME_NARRATOR:
                            break;
                        
                        default:
                            
                            StagePosController.Show();
                            StagePosController.HandleMoves(new List<CharacterMove>
                            {
                                new CharacterMove
                                {
                                    CharacterDevName = characterDevName,
                                    NewStagePos = 1,
                                }
                            });
                            break;
                    }
                    
                    DialogueController.Show();
                    DialogueController.HandleDialogue(characterDevName, stageAction.DialogueText);
                    shouldBlock = true;

                    mExpectingUserInput = true;
                    break;
                
                case StageActionType.GUNK_INIT:
                    mOnlyDialoguePhases = false;
                    // isClearingGunk = true;
                    ZBug.Info($"Adding gunk: {stageAction.GunkType} - {stageAction.GunkAmount}");
                    Send.Msg(new InitGunkMsg
                    {
                        InitData = new []
                        {
                            new GunkInitData
                            {
                                GunkTag = stageAction.GunkType,
                                Amount = stageAction.GunkAmount
                            }
                        }
                    });
                    DialogueController.Hide();
                    StagePosController.Hide();
                    break;
                
                case StageActionType.GUNK_INDICES:
                    mOnlyDialoguePhases = false;

                    int[] indices = stageAction.CleanGunkIndices();
                    ZBug.Info("OPERATION", $"Adding gunk at indices: {stageAction.GunkType} {string.Join(",", indices)}");
                    Send.Msg(new InitGunkIndicesMsg
                    {
                        InitData = new []
                        {
                            new GunkInitIndicesData
                            {
                                GunkTag = stageAction.GunkType,
                                Indices = indices
                            }
                        }
                    });
                    DialogueController.Hide();
                    StagePosController.Hide();
                    break;
                    
                case StageActionType.CHOICE:

                    // TODO: Handle choices in operations
                    // DialogueController.HandleChoices(stageAction.Choices, stageAction.NumberOfChoicesToSelect);
                    // shouldBlock = true;
                    break;
                
                default:
                    mOnlyDialoguePhases = false;
                    ZBug.Error("OPERATION", $"Unhandled stage action: {stageAction.Type}");
                    shouldBlock = true;
                    break;
            }
            
            if (shouldBlock)
            {
                ZBug.Warn("OPERATION", $"Blocked on action");
                return;
            }
            
            ZBug.Info("OPERATION", $"[OnNextActionRequest] Moving to next, last action was non-blocking");
            BrainMain.Instance.Context.OperationManager.MoveNextAction();
        }
    }
}