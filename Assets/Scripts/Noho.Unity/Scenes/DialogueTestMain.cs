using Noho.Unity.Components;
using UnityEngine;

namespace Noho.Unity.Scenes
{
    public class DialogueTestMain : MonoBehaviour
    {
        public DialogueController Dialogue;

        public string[] Phrases;

        public int Index;
    
        // Start is called before the first frame update
        void Start()
        {
            ZBug.Info("DialogueTestMain","Start");
            // InvokeRepeating(nameof(Tick), 0f, 5f);
            ResetInvoke();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ResetInvoke();
            }
        }

        public void ResetInvoke()
        {
        
            CancelInvoke(nameof(Tick));
            InvokeRepeating(nameof(Tick), 0f, 5f);
        }

        void Tick()
        {
            ZBug.Info("DialogueTestMain","Tick");
            string phrase = Phrases[Index];
            Dialogue.HandleDialogue("test", phrase);

            Index = (Index + 1) % Phrases.Length;
        }
    }
}
