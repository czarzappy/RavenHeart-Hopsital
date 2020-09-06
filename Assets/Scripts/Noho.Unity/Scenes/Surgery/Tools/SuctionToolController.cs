using UnityEngine;
using ZEngine.Unity.Core.Attributes;

namespace Noho.Unity.Scenes.Surgery.Tools
{
    public class SuctionToolController : MonoBehaviour
    {
        public void Start()
        {
            Init();
        }

        public void Init()
        {
            gameObject.AddZAttr<UIPointerFollowZAttr>();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartSuction();
            }

            if (Input.GetMouseButtonUp(0))
            {
                EndSuction();
            }
        }

        private void StartSuction()
        {
            ZBug.Info("Suction", "ON!");
        }

        private void EndSuction()
        {
            ZBug.Info("Suction", "OFF!");
        }
    }
}