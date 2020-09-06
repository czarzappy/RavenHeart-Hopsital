using Noho.Unity.Components;
using Noho.Unity.Managers;
using Noho.Unity.Messages;
using Noho.Unity.Models;
using Noho.Unity.Scenes.Title;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;
using ZEngine.Unity.Core.Scenes;

namespace Noho.Unity.Scenes
{
    public partial class TitleMain : ZMain
    {
        public Button StartButton;
        public Button SettingsButton;
        public Button CreditButton;
        public Button ExitBtn;
        public Button CreateBtn;

        public EpisodeSelectController EpisodeSelect;
        public CharacterSelectWin CharacterSelect;
        public SettingsWin SettingsWin;
        public CreateWin CreateWin;
        public SceneSelectController SceneSelect;
        
        
        public TextAsset IntroEpisode;
        
        // Start is called before the first frame update
        void Start()
        {
            Init();
        }

        private void Init()
        {
            int i = 0;
            ZBug.Info("TITLE", $"Init: {i++}");
            App.Instance.Init();
            ZBug.Info("TITLE", $"Init: {i++}");
            
            // ZLog.Info("Test");
            StartButton.onClick.AddListener(OnStartBtnClicked);
            SettingsButton.onClick.AddListener(OnSettingsBtnClicked);
            CreditButton.onClick.AddListener(OnCreditBtnClicked);
            ExitBtn.onClick.AddListener(OnExitBtnClicked);
            CreateBtn.onClick.AddListener(OnCreateBtnClicked);
            
            MsgMgr.Instance.SubscribeTo<ShowWinMsg>(OnShowWin);
            MsgMgr.Instance.SubscribeTo<CloseWinMsg>(OnCloseWin);
            
            SettingsWin.Hide();
        }

        public void FixedUpdate()
        {
            MsgMgr.Instance.Pump();
        }

        public void OnDestroy()
        {
            MsgMgr.Instance.UnsubscribeFrom<ShowWinMsg>(OnShowWin);
        }

        private void OnExitBtnClicked()
        {
            App.Instance.Quit();
        }

        private void OnStartBtnClicked()
        {
            ZBug.Info("TITLE", "On Start Btn");
            // if (App.Instance.PersistentData.PlayedIntro)
            // {
                Send.Msg(new ShowWinMsg
                {
                    Win = Win.CHARACTER_SELECT
                });
            // }
            // else
            // {
            //     AppManager.Instance.StartEpisode(IntroEpisode, true);
            // }
            // SceneManager.LoadSceneAsync("BrainScene", LoadSceneMode.Single);
            
        }

        private void OnCreditBtnClicked()
        {
            AppManager.Instance.GoToCredits();
        }

        // Update is called once per frame
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || 
                Input.GetKeyDown(KeyCode.Backspace) || 
                Input.GetKeyDown(KeyCode.Delete))
            {
                GoBack();
            }
        }

        private void GoBack()
        {
            PopWin();
        }
    }
}
