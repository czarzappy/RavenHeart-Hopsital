using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core.Components;

namespace Noho.Unity.Components
{
    public class URLButtonController : UIMonoBehaviour
    {
        public Button Button;

        public string URL;

        public void Awake()
        {
            Init();
        }

        private void Init()
        {
            Button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            Application.OpenURL(URL);
        }
    }
}