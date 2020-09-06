using Noho.Messages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery
{
    public class VitalController : UIMonoBehaviour
    {
        public TMP_Text VitalNumberText;

        public Slider VitalSlider;
        
        public Image VitalImage;
        public Gradient ColorGradient;

        public TMP_Text VitalCriticalText;
        public TMP_Text VitalWarningText;

        public void Awake()
        {
            Init();
        }

        private int mCurrentPatientId;

        public void Init()
        {
            MsgMgr.Instance.SubscribeTo<CurrentPatientChangedMsg>(OnCurrentPatientChanged);
            MsgMgr.Instance.SubscribeTo<PatientVitalsChangedMsg>(OnPatientVitalsChanged);

            if (BrainMain.Instance == null)
            {
                return;
            }

            var patient = BrainMain.Instance.Context.OperationManager.CurrentPatientManager;
            mCurrentPatientId = patient.PatientId;
            
            UpdateUI(patient.RoundUpVitals, patient.DisplayCurrentVitals, patient.VitalPercentage);
        }

        public void OnDestroy()
        {
            UnInit();
        }

        private void UnInit()
        {
            MsgMgr.Instance.UnsubscribeFrom<CurrentPatientChangedMsg>(OnCurrentPatientChanged);
            MsgMgr.Instance.UnsubscribeFrom<PatientVitalsChangedMsg>(OnPatientVitalsChanged);
        }

        private void OnPatientVitalsChanged(PatientVitalsChangedMsg message)
        {
            if (message.PatientId != mCurrentPatientId)
            {
                return;
            }

            UpdateUI(message.NewVitals, message.NewDisplayVitals, message.NewVitalPercentage);
        }

        public const int WARNING_THRESHOLD = 30;
        public const int CRITICAL_THRESHOLD = 20;
        public void UpdateUI(int newVitals, string newDisplayVitals, float newVitalPercentage)
        {
            VitalNumberText.text = newDisplayVitals;

            float t = newVitalPercentage;
            VitalSlider.value = t;

            if (newVitals < WARNING_THRESHOLD)
            {
                if (newVitals < CRITICAL_THRESHOLD)
                {
                    VitalCriticalText.Show();
                    VitalWarningText.Hide();
                }
                else
                {
                    VitalWarningText.Show();
                    VitalCriticalText.Hide();
                }
            }
            else
            {
                VitalCriticalText.Hide();
                VitalWarningText.Hide();
            }
            
            // if(value)

            Color color = ColorGradient.Evaluate(t);
            VitalImage.color = color;
            VitalNumberText.color = color;
        }

        private void OnCurrentPatientChanged(CurrentPatientChangedMsg message)
        {
            mCurrentPatientId = message.NewPatientId;
            
            var patient = BrainMain.Instance.Context.OperationManager.CurrentPatientManager;
            UpdateUI(patient.RoundUpVitals, patient.DisplayCurrentVitals, patient.VitalPercentage);
        }
    }
}