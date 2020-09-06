using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Noho.Unity.Components
{
    public class ToolbarBtnController : MonoBehaviour
    {
        public Button Button;

        public Image ToolImage;

        public void OnSelected(UnityAction action)
        {
            Button.onClick.AddListener(action);
        }

        public void RemoveListeners()
        {
            Button.onClick.RemoveAllListeners();
        }
    }
}