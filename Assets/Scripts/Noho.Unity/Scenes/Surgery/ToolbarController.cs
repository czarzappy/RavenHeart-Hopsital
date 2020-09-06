using Noho.Models;
using Noho.Unity.Components;
using UnityEngine;

namespace Noho.Unity.Scenes.Surgery
{
    public class ToolbarController : MonoBehaviour
    {
        public ToolboxController ToolboxController;
        
        public ToolbarBtnController Forceps;
        public ToolbarBtnController Juice;
        public ToolbarBtnController Bandage;
        public ToolbarBtnController Suture;
        public ToolbarBtnController Scalpel;
        public ToolbarBtnController Suction;
        public ToolbarBtnController Injection;

        // Start is called before the first frame update
        void Start()
        {
            Init();
        }

        public void Init()
        {
            Forceps.OnSelected(() => ToolboxController.EquipTool(NohoConstants.ToolType.FORCEPS));
            Juice.OnSelected(() => ToolboxController.EquipTool(NohoConstants.ToolType.JUICE));
            Bandage.OnSelected(() => ToolboxController.EquipTool(NohoConstants.ToolType.BANDAGE));
            Suture.OnSelected(() => ToolboxController.EquipTool(NohoConstants.ToolType.SUTURE));
            Scalpel.OnSelected(() => ToolboxController.EquipTool(NohoConstants.ToolType.SCALPEL));
            Suction.OnSelected(() => ToolboxController.EquipTool(NohoConstants.ToolType.SUCTION));
            Injection.OnSelected(() => ToolboxController.EquipTool(NohoConstants.ToolType.INJECTION));
        }

        public void OnDestroy()
        {
            UnInit();
        }

        public void UnInit()
        {
            Forceps.RemoveListeners();
            Juice.RemoveListeners();
            Bandage.RemoveListeners();
            Suture.RemoveListeners();
            Scalpel.RemoveListeners();
            Suction.RemoveListeners();
            Injection.RemoveListeners();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
