using TMPro;
using UnityEngine;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Math;
using ZEngine.Unity.Core.Models;

namespace Noho.Unity.Components
{
    public static class Show
    {
        public static void Nice(Transform transform)
        {
            Nice(transform.position, transform);
        }
        public static void Nice(Transform transform, Transform parent)
        {
            Nice(transform.position, parent);
        }
        public static void Nice(Vector3 position, Transform parent)
        {
            Object niceSource = ZSource.Load(NiceController.PrefabPath);

            GameObject go = (GameObject) Object.Instantiate(niceSource, position, Quaternion.identity, parent);
            NiceController nice = go.GetComponent<NiceController>();
            
            nice.Init(ZRandom.EnumValue<NiceController.NiceEnum>());
        }
    }
    public class NiceController : UIMonoBehaviour
    {
        public TMP_Text Text;
        public static ResourcePath PrefabPath = new ResourcePath("UI", "Surgery", "Nice");
        
        public enum NiceEnum
        {
            WOW,
            WOAH,
            NEAT,
            OKAY,
            NICE,
            COOL,
            GNARLY,
            RADICAL,
            BAZINGA
        }

        public void Init(NiceEnum nice)
        {
            Text.text = nice.ToString();
        }
    }
}