using Noho.Unity.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Noho.Unity.Scenes
{
    public class CreditMain : MonoBehaviour
    {
        public Button TitleBtn;

        // Start is called before the first frame update
        void Start()
        {
            TitleBtn.onClick.AddListener(OnTitleBtnClicked);
        }

        private void OnTitleBtnClicked()
        {
            AppManager.Instance.GoToTitleScene();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
