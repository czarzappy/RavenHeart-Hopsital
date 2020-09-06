using System;
using Noho.Unity.Models;
using UnityEngine;
using ZEngine.Unity.Core.Attributes;
using ZEngine.Unity.Core.Components;

namespace Noho.Unity.Scenes.Surgery
{
    public class TrayController : UIMonoBehaviour
    {
        public RectTransform TrashDestination;

        private Vector2 mResetPosition;

        public void Start()
        {
            mResetPosition = rectTransform.anchoredPosition;
        }

        public void TakeAwayGunk(GameObject gunk, Action onEndCallback)
        {
            var dumpPos = TrashDestination.anchoredPosition;

            Vector3 displacement = dumpPos - mResetPosition;
            gameObject.InitFiniteTickZAttr<MultiStopTranslateZAttr, MultiStopTranslateZAttr.InitData>(
                new MultiStopTranslateZAttr.InitData
                {
                    Stops = new []
                    {
                        mResetPosition,
                        dumpPos,
                        mResetPosition
                    },
                    Duration = NohoUISettings.TRAY_DUMP_GUNK_DURATION,
                });

            var gunkPos = gunk.transform.position;
            gunk.InitDurationTickZAttr<TranslateZAttr, TranslateZAttr.InitData>(
                new TranslateZAttr.InitData
                {
                    StartPos = gunkPos,
                    EndPos = gunkPos + displacement,
                    OnEndCallback = onEndCallback
                }, NohoUISettings.TRAY_DUMP_GUNK_DURATION);
        }
    }
}