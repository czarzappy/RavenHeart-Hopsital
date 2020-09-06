using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core.Attributes;
using ZEngine.Unity.Core.Extensions;

namespace ZEngine.Unity.Core.Components
{
    public class ExampleAttributeComponent : MonoBehaviour
    {
        // Transforms to act as start and end markers for the journey.

        // Time when the movement started.

        public Transform StartMarker;
        public Transform EndMarker;

        public Image Image;
        void Start()
        {
            Image = GetComponent<Image>();

            gameObject.InitDurationTickZAttr<TranslateZAttr, TranslateZAttr.InitData>(new TranslateZAttr.InitData
            {
                StartPos = StartMarker.position,
                EndPos = EndMarker.position,
            }, 10f);

            // gameObject.InitFiniteTickZAttr<ImageFadeInZAttr, ImageFadeInZAttr.InitData>(new ImageFadeInZAttr.InitData
            // {
            //     Image = Image,
            //     Duration = 10f,
            // });
        }
    }
}