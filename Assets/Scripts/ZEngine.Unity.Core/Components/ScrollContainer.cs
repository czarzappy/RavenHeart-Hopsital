using System;
using System.Collections;
using UnityEngine;
using ZEngine.Unity.Core.Extensions;

namespace ZEngine.Unity.Core.Components
{
    public abstract class ScrollContainer<TItem, TInitData> : UIMonoBehaviour where TItem : ScrollItem<TInitData>
    {
        public TItem TemplateItem;
        public Transform ContentParent;

        public void OnEnable()
        {
            TemplateItem.Hide();
            StartCoroutine(Init());
        }

        public void OnDestroy()
        {
            UnInit();
        }

        public abstract IEnumerator Init();
        public abstract void UnInit();

        public void ClearChildren()
        {
            for (int childIdx = 0; childIdx < ContentParent.childCount; childIdx++)
            {
                var child = ContentParent.GetChild(childIdx);
                
                Destroy(child.gameObject);
            }
        }

        public IEnumerator AddNewItem(Func<TInitData> initDataFactory)
        {
            TItem item = Instantiate(TemplateItem, ContentParent);
            
            item.Show();
            item.Init(initDataFactory());
            
            yield break;
        }
    }
}