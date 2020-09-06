using UnityEngine;

namespace ZEngine.Unity.Core.Components
{
    public class UIMonoBehaviour : MonoBehaviour
    {
        private RectTransform mRectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (mRectTransform == null)
                {
                    mRectTransform = GetComponent<RectTransform>();
                }

                return mRectTransform;
            }
        }
    }
}