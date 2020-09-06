using Noho.Unity.Messages;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Components
{
    public class SettingsWin : MonoBehaviour
    {
        public Button BackButton;
        
        // Start is called before the first frame update
        void Start()
        {
            BackButton.onClick.AddListener(OnBackButtonClick);
        }

        private void OnBackButtonClick()
        {
            Send.Msg(new CloseWinMsg
            {
                
            });
        }
    }
}
