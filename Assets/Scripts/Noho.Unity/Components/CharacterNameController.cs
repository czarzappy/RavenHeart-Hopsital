using UnityEngine;

namespace Noho.Unity.Components
{
    public class CharacterNameController : MonoBehaviour
    {
        public Vector3 pos;

        public void Init()
        {
            // CharacterNameContainer.Hide();
        }

        public void SetName(string characterDevName)
        {
            
        }

        public void Update()
        {
            pos = transform.position;
        }
    }
}