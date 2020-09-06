using Noho.Models;
using Noho.Unity.Messages;
using Noho.Unity.Models;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core.Attributes;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery.Gunk
{
    public class IncisionSpotController : MonoBehaviour
    {
        public enum IncisionState
        {
            FRESH,
            JUICED,
            CUT,
        }

        public IncisionState State = IncisionState.FRESH;

        public Image SpotImage;

        public Color JuicedColor;

        public void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case NohoConstants.GOTags.JUICE:
                    switch (State)
                    {
                        case IncisionState.FRESH:

                            OnJuiced();
                            
                            
                            break;
                    }
                    break;
                
                case NohoConstants.GOTags.SCALPEL:
                    switch (State)
                    {
                        case IncisionState.JUICED:
                            OnCut();
                            break;
                    }
                    break;
                // default:
                //     
                //     Debug.Log($"Unknown collision: {other.gameObject}, tag: {other.gameObject.tag}");
                //     break;
            }
        }

        private void OnCut()
        {
            // Debug.Log("CUTTING INCISION");
            State = IncisionState.CUT;

            gameObject.InitDurationTickZAttr<ColorFadeZAttr, ColorFadeZAttr.InitData>(new ColorFadeZAttr.InitData
            {
                SourceType = ColorFadeZAttr.SourceType.IMAGE,
                Image = SpotImage,
                
                BlendType = ColorFadeZAttr.BlendType.START_END,
                StartColor = SpotImage.material.color.FullAlpha(),
                EndColor = SpotImage.material.color.NoAlpha(),
            },  NohoUISettings.INCISION_CUT_DURATION);
            
            Send.Msg(new CutSpotMsg
            {
                CutSpot = this
            });
        }

        private void OnJuiced()
        {
            // Debug.Log("JUICING INCISION");
            State = IncisionState.JUICED;
            
            gameObject.InitDurationTickZAttr<ColorFadeZAttr, ColorFadeZAttr.InitData>(new ColorFadeZAttr.InitData
            {
                SourceType = ColorFadeZAttr.SourceType.IMAGE,
                Image = SpotImage,
                
                BlendType = ColorFadeZAttr.BlendType.START_END,
                StartColor = SpotImage.material.color,
                EndColor = Color.green,
            },  NohoUISettings.INCISION_JUICE_DURATION);
            
            
            Send.Msg(new SpotGelledMsg
            {
                CutSpot = this
            });
        }
    }
}