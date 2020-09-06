using System.Collections.Generic;
using Noho.Unity.Messages;
using Noho.Unity.Models;
using UnityEngine;
using UnityEngine.Serialization;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes
{
    public partial class TitleMain
    {
        public CanvasGroup MainMenu;
        
        [FormerlySerializedAs("winStack")] 
        public Stack<Win> WinStack = new Stack<Win>();

        private void OnShowWin(ShowWinMsg message)
        {
            PushWin(message.Win);
        }

        private void OnCreateBtnClicked()
        {
            Send.Msg(new ShowWinMsg
            {
                Win = Win.CREATE_WIN,
            });
        }

        private void OnCloseWin(CloseWinMsg message)
        {
            PopWin();
        }

        private void OnSettingsBtnClicked()
        {
            Send.Msg(new ShowWinMsg
            {
                Win = Win.SETTINGS_WIN
            });
        }
        
        public MonoBehaviour GetWin(Win messageWin)
        {
            MonoBehaviour win = null;
            switch (messageWin)
            {
                case Win.SCENE_SELECT:
                    win = SceneSelect;
                    break;
                
                case Win.EPISODE_SELECT:
                    win = EpisodeSelect;
                    break;
                
                case Win.CHARACTER_SELECT:
                    win = CharacterSelect;
                    break;
                
                case Win.SETTINGS_WIN:
                    win = SettingsWin;
                    break;
                
                case Win.CREATE_WIN:
                    win = CreateWin;
                    break;
            }

            return win;
        }
        
        public void PushWin(Win messageWin)
        {
            var win = GetWin(messageWin);

            if (win == null)
            {
                return;
            }

            WinStack.Push(messageWin);
            
            MainMenu.blocksRaycasts = false;
            win.Show();
        }

        private void PopWin()
        {
            if (WinStack.Count == 0)
            {
                return;
            }
            
            Win winType = WinStack.Pop();
            var lastWin = GetWin(winType);
            
            lastWin.Hide();

            if (WinStack.Count == 0)
            {
                MainMenu.blocksRaycasts = true;
            }
        }
    }
}