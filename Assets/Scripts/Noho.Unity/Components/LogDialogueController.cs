using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core.Components;

namespace Noho.Unity.Components
{
    public class LogDialogueController : UIMonoBehaviour
    {
        public TMP_Text DialogueText;
        public TMP_Text SpeakerName;
        public Image DialogueImage;

        public void Init(string speakerName, string dialogue, Color color)
        {
            SpeakerName.text = speakerName;
            DialogueText.text = dialogue;
            DialogueImage.color = color;
        }
    }
}