using Noho.Messages;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;
using Object = UnityEngine.Object;

namespace Noho.Unity.Scenes.Surgery
{
    public class PatientSectionController : UIMonoBehaviour
    {
        public PatientController Template;
        
        public void OnDestroy()
        {
            UnInit();
        }

        private void UnInit()
        {
            MsgMgr.Instance.UnsubscribeFrom<NewPatientMsg>(OnNewPatient);
        }

        public void Awake()
        {
            Init();
        }

        public void Init()
        {
            MsgMgr.Instance.SubscribeTo<NewPatientMsg>(OnNewPatient);

            if (BrainMain.Instance == null)
            {
                return;
            }

            foreach (int activePatientId in BrainMain.Instance.Context.OperationManager.ActivePatientIds)
            {
                CreatNewPatient(activePatientId);
            }
        }

        private void OnNewPatient(NewPatientMsg message)
        {
            CreatNewPatient(message.NewPatientId);
        }

        private void CreatNewPatient(int newPatientId)
        {
            var newPatient = Object.Instantiate(Template, this.transform, false);
            
            newPatient.Show();
            
            newPatient.Init(newPatientId);
        }
    }
}