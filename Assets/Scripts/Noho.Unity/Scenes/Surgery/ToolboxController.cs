using System;
using System.Collections.Generic;
using Noho.Models;
using Noho.Unity.Scenes.Surgery.Tools;
using UnityEngine;
using ZEngine.Unity.Core.Extensions;

namespace Noho.Unity.Scenes.Surgery
{
    public class ToolboxController : MonoBehaviour
    {
        private static ToolboxController gInstance;
        public static ToolboxController Instance => gInstance;

        private MonoBehaviour mLastTool;
        public ForcepsToolController Forceps;
        public JuiceController Juice;
        public BandageToolController Bandage;
        public SutureController Suture;
        public ScalpelController Scalpel;
        public SuctionToolController Suction;
        public InjectionToolController Injection;
        
        public NohoConstants.ToolType CurrentToolType;
        
        public void Awake()
        {
            Init();
        }

        private void Init()
        {
            ZBug.Info("TOOLBOX", "INIT");
            gInstance = this;
            // mLastTool = Forceps;
            
            // Juice.Hide();
            EquipTool(CurrentToolType);
        }

        public void OnDestroy()
        {
            if (gInstance == this)
            {
                ZBug.Info("TOOLBOX", "UNINIT");
                gInstance = null;
            }
        }
        
        private readonly List<Tuple<KeyCode, NohoConstants.ToolType>> mDefaultKeyBindings = new List<Tuple<KeyCode, NohoConstants.ToolType>>
        {
            new Tuple<KeyCode, NohoConstants.ToolType>(KeyCode.Alpha1, NohoConstants.ToolType.FORCEPS ),
            new Tuple<KeyCode, NohoConstants.ToolType>(KeyCode.Alpha2, NohoConstants.ToolType.JUICE ),
            new Tuple<KeyCode, NohoConstants.ToolType>(KeyCode.Alpha3, NohoConstants.ToolType.SUTURE ),
            new Tuple<KeyCode, NohoConstants.ToolType>(KeyCode.Alpha4, NohoConstants.ToolType.INJECTION ),
            new Tuple<KeyCode, NohoConstants.ToolType>(KeyCode.Q, NohoConstants.ToolType.SUCTION ),
            new Tuple<KeyCode, NohoConstants.ToolType>(KeyCode.W, NohoConstants.ToolType.BANDAGE ),
            new Tuple<KeyCode, NohoConstants.ToolType>(KeyCode.E, NohoConstants.ToolType.SCALPEL ),
        };

        public void Update()
        {
            foreach (var tuple in mDefaultKeyBindings)
            {
                if (Input.GetKeyDown(tuple.Item1))
                {
                    EquipTool(tuple.Item2);
                }
            }
        }

        public void EquipTool(NohoConstants.ToolType toolType)
        {
            CurrentToolType = toolType;
            switch (toolType)
            {
                case NohoConstants.ToolType.JUICE:
                    Juice.Show();
                    Forceps.Hide();
                    Bandage.Hide();
                    Suture.Hide();
                    Scalpel.Hide();
                    Suction.Hide();
                    Injection.Hide();
                    break;
                
                case NohoConstants.ToolType.FORCEPS:
                    Forceps.Show();
                    Juice.Hide();
                    Bandage.Hide();
                    Suture.Hide();
                    Scalpel.Hide();
                    Suction.Hide();
                    Injection.Hide();
                    break;
                
                case NohoConstants.ToolType.BANDAGE:
                    Bandage.Show();
                    Forceps.Hide();
                    Juice.Hide();
                    Suture.Hide();
                    Scalpel.Hide();
                    Suction.Hide();
                    Injection.Hide();
                    break;
                
                case NohoConstants.ToolType.SUTURE:
                    Suture.Show();
                    Bandage.Hide();
                    Forceps.Hide();
                    Juice.Hide();
                    Scalpel.Hide();
                    Suction.Hide();
                    Injection.Hide();
                    break;
                
                case NohoConstants.ToolType.SCALPEL:
                    Scalpel.Show();
                    Suture.Hide();
                    Bandage.Hide();
                    Forceps.Hide();
                    Juice.Hide();
                    Suction.Hide();
                    Injection.Hide();
                    break;
                
                case NohoConstants.ToolType.SUCTION:
                    Suction.Show();
                    Scalpel.Hide();
                    Suture.Hide();
                    Bandage.Hide();
                    Forceps.Hide();
                    Juice.Hide();
                    Injection.Hide();
                    break;
                
                case NohoConstants.ToolType.INJECTION:
                    Injection.Show();
                    Suction.Hide();
                    Scalpel.Hide();
                    Suture.Hide();
                    Bandage.Hide();
                    Forceps.Hide();
                    Juice.Hide();
                    break;
            }
        }

        public static Func<bool> IsEquippedCB(NohoConstants.ToolType toolType) => () =>
        {
            if (Instance == null)
            {
                return false;
            }
            
            return Instance.IsEquipped(toolType);
        };

        public bool IsEquipped(NohoConstants.ToolType toolType)
        {
            return CurrentToolType == toolType;
        }
    }
}