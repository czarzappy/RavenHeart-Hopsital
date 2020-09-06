using UnityEngine;
using ZEngine.Unity.Core.Models;

namespace ZEngine.Unity.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static void Hide(this GameObject gameObject)
        {
            gameObject.SetActive(false);
        }
        
        public static bool IsHidden(this GameObject gameObject)
        {
            return !gameObject.activeSelf;
        }
        
        public static void Show(this GameObject gameObject)
        {
            gameObject.SetActive(true);
        }
        
        public static void Hide(this Component component)
        {
            component.gameObject.Hide();
        }
        
        public static bool IsHidden(this Component component)
        {
            return component.gameObject.IsHidden();
        }
        
        public static void Show(this Component component)
        {
            component.gameObject.Show();
        }
        public static void Enable(this MonoBehaviour behaviour)
        {
            behaviour.enabled = true;
        }
        
        public static void Disable(this MonoBehaviour behaviour)
        {
            behaviour.enabled = false;
        }
        
        public static void Toggle(this Component component, bool showShould)
        {
            component.gameObject.SetActive(showShould);
        }
        
        public static void Toggle(this Component component)
        {
            component.gameObject.SetActive(!component.gameObject.activeSelf);
        }

        public static T AddChildFromPrefab<T>(this Behaviour behaviour, ResourcePath resourcePath) where T : Behaviour
        {
            Object lineComponent = ZSource.Load<GameObject>(resourcePath);

            T newObj = ((GameObject) Object.Instantiate(lineComponent)).GetComponent<T>();
        
            newObj.transform.parent = behaviour.transform;

            return newObj;
        }

        public static T AddChildFromPrefab<T>(this MonoBehaviour behaviour, ResourcePath resourcePath) where T : Behaviour
        {
            Object lineComponent = ZSource.Load<GameObject>(resourcePath);

            T newObj = ((GameObject) Object.Instantiate(lineComponent)).GetComponent<T>();
        
            newObj.transform.parent = behaviour.transform;

            return newObj;
        }

        public static T InstantiatePrefab<T>(ResourcePath resourcePath) where T : Behaviour
        {
            Object lineComponent = ZSource.Load<GameObject>(resourcePath);

            T newObj = ((GameObject) Object.Instantiate(lineComponent)).GetComponent<T>();

            return newObj;
        }

        public static T AddChildFromPrefab<T>(this Component component, ResourcePath resourcePath) where T : Behaviour
        {
            Object prefab = ZSource.Load<GameObject>(resourcePath);

            T newObj = ((GameObject) Object.Instantiate(prefab)).GetComponent<T>();
        
            newObj.transform.parent = component.transform;

            return newObj;
        }

        public static T AddChildFromPrefab<T>(this GameObject gameObject, ResourcePath resourcePath) where T : Behaviour
        {
            Object lineComponent = ZSource.Load<GameObject>(resourcePath);

            T newObj = ((GameObject) Object.Instantiate(lineComponent)).GetComponent<T>();
        
            newObj.transform.parent = gameObject.transform;

            return newObj;
        }
    }
}