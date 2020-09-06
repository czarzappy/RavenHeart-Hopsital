using System;
using Noho.Unity.Components;
using Noho.Unity.Messages;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Title
{
    public class CreateWin : UIMonoBehaviour
    {
        public Button BGButton;
        [FormerlySerializedAs("SFXButton")] 
        public Button BGMButton;
        public Button SurgeryButton;
        public Button BackBtn;

        public Transform BGPanel;
        public Transform BGMPanel;

        public void Awake()
        {
            Init();
        }

        private void Init()
        {
            BGButton.onClick.AddListener(OnBGButtonClick);
            BGMButton.onClick.AddListener(OnBGMButtonClick);
            BackBtn.onClick.AddListener(OnBackBtnClick);
        }

        private void OnBackBtnClick()
        {
            Send.Msg(new CloseWinMsg
            {
                
            });
        }

        public Transform CurrentPanel;
        private void OnBGButtonClick()
        {
            Show(BGPanel);
        }

        private void Show(Transform panel)
        {
            if (CurrentPanel == panel)
            {
                return;
            }
            
            CurrentPanel?.Hide();
            CurrentPanel = panel;
            
            panel.Show();
        }

        private void OnBGMButtonClick()
        {
            Show(BGMPanel);
        }
    }
}