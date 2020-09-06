using Noho.Messages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery
{
    public class PatientController : UIMonoBehaviour
    {
        public Image PatientImage;

        public TMP_Text PatientName;
        public TMP_Text PatientVitals;

        private int mPatientId;
        
        public Gradient ColorGradient;

        public void Init(int patientId)
        {
            mPatientId = patientId;
            
            var patient = BrainMain.Instance.Context.OperationManager.GetPatient(patientId);
            
            // one time set
            PatientName.text = patient.PatientDef.Name;

            UpdateUI(patient.DisplayCurrentVitals, patient.VitalPercentage);
            
            MsgMgr.Instance.SubscribeTo<PatientVitalsChangedMsg>(OnPatientVitalsChanged);
        }

        public void OnDestroy()
        {
            UnInit();
        }

        private void UnInit()
        {
            MsgMgr.Instance.UnsubscribeFrom<PatientVitalsChangedMsg>(OnPatientVitalsChanged);
        }

        private void OnPatientVitalsChanged(PatientVitalsChangedMsg message)
        {
            if (message.PatientId != mPatientId)
            {
                return;
            }

            UpdateUI(message.NewDisplayVitals, message.NewVitalPercentage);
        }

        public void UpdateUI(string newDisplayVitals, float newVitalPercentage)
        {
            
            PatientVitals.text = newDisplayVitals;
            
            float t = newVitalPercentage;
            var color = ColorGradient.Evaluate(t);
            
            SetColor(color);
        }

        public void SetColor(Color color)
        {
            PatientImage.color = color;
        }
    }
}