using Noho.Unity.Managers;
using UnityEngine.UI;
using ZEngine.Unity.Core.Components;

namespace Noho.Unity.Scenes.Surgery
{
    public class FailWin : UIMonoBehaviour
    {
        public Button TryAgainBtn;
        public Button MainMenuBtn;

        public void Awake()
        {
            Init();
        }

        public void Init()
        {
            TryAgainBtn.onClick.AddListener(OnTryAgainBtnClicked);
            MainMenuBtn.onClick.AddListener(OnMainMenuBtnClicked);
        }

        public void OnTryAgainBtnClicked()
        {
            ZBug.Info("Fail", "Trying again");
            BrainMain.Instance.RestartOperation();
        }

        public void OnMainMenuBtnClicked()
        {
            ZBug.Info("Fail", "Going to Main Menu");
            AppManager.Instance.GoToTitleScene();
        }
    }
}