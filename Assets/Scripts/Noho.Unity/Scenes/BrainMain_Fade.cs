using System;
using System.Collections;
using Noho.Unity.Models;
using UnityEngine;
using ZEngine.Unity.Core.Attributes;
using ZEngine.Unity.Core.Extensions;

namespace Noho.Unity.Scenes
{
    public partial class BrainMain
    {
        private IEnumerator FadeOutThenRun(Action action)
        {
            yield return FadeOut();

            action();
        }
        
        private IEnumerator FadeOut()
        {
            if (IsSkipping)
            {
                yield break;
            }
            
            LoadingCamera.enabled = true;
            
            ZBug.Info("BRAIN", "Starting Fade Out");
            BlackImage.gameObject.InitDurationTickZAttr<ColorFadeZAttr, ColorFadeZAttr.InitData>(new ColorFadeZAttr.InitData
            {
                BlendType = ColorFadeZAttr.BlendType.START_END,
                StartColor = BlackImage.color.NoAlpha(),
                EndColor = BlackImage.color.FullAlpha(),
                SourceType = ColorFadeZAttr.SourceType.IMAGE,
                Image = BlackImage,
            }, NohoUISettings.BRAIN_FADE_OUT);

            yield return new WaitUntil(IsFadedOut);
            ZBug.Info("BRAIN", "Fade Out Complete");
        }
        
        private IEnumerator FadeIn()
        {
            if (IsSkipping)
            {
                yield break;
            }
            
            yield return new WaitForSeconds(NohoUISettings.BRAIN_FADE_IN_DELAY);
            
            ZBug.Info("BRAIN", "Starting Fade In");
            BlackImage.gameObject.InitDurationTickZAttr<ColorFadeZAttr, ColorFadeZAttr.InitData>(new ColorFadeZAttr.InitData
            {
                BlendType = ColorFadeZAttr.BlendType.START_END,
                StartColor = BlackImage.color.FullAlpha(),
                EndColor = BlackImage.color.NoAlpha(),
                SourceType = ColorFadeZAttr.SourceType.IMAGE,
                Image = BlackImage,
            }, NohoUISettings.BRAIN_FADE_IN);

            yield return new WaitUntil(IsFadedIn);
            ZBug.Info("BRAIN", "Fade In Complete");
            
            LoadingCamera.enabled = false;
        }

        private bool IsFadedIn()
        {
            return Math.Abs(BlackImage.color.a - 0f) < Mathf.Epsilon;
        }

        private bool IsFadedOut()
        {
            return Math.Abs(BlackImage.color.a - 1f) < Mathf.Epsilon;
        }
    }
}