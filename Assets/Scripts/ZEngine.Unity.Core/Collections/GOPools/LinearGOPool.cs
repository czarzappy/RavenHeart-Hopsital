using System.Collections.Generic;
using UnityEngine;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Models;

namespace ZEngine.Unity.Core.Collections.GOPools
{
    public class LinearGOPool : MonoBehaviour
    {
        public PoolSettings Settings;

        public void Init(PoolSettings settings)
        {
            this.Settings = settings;
        }

        public List<Behaviour> TrackedItems = new List<Behaviour>();
        public List<Behaviour> VisibleItems = new List<Behaviour>();
        public Queue<Behaviour> QueuedItems = new Queue<Behaviour>();

        public int CurrentQueueSize;
        // private int mIndex;
        
        public T Get<T>() where T : Behaviour
        {
            CurrentQueueSize = QueuedItems.Count;
            // if (settings.HasMax)
            // {
            //     if (currentSize >= settings.Overflow.HardCap)
            //     {
            //         mIndex = mIndex % currentSize;
            //     }
            //
            //     if (currentSize >= settings.Overflow.SoftCap)
            //     {
            //         
            //     }
            // }
            
            T nextItem;
            
            if (CurrentQueueSize > 0)
            {
                nextItem = (T) QueuedItems.Dequeue();
            }
            else
            {
                nextItem = New<T>();
            }
            
            nextItem.Show();
            VisibleItems.Add(nextItem);

            return nextItem;
        }

        private T New<T>() where T : Behaviour
        {
            T newItem = ObjectExtensions.InstantiatePrefab<T>(Settings.PrefabResourcePath);

            newItem.transform.SetParent(Settings.ParentTransform, true);
            TrackedItems.Add(newItem);

            return newItem;
        }

        public void ReturnAll()
        {
            foreach (var item in VisibleItems)
            {
                ReturnToQueue(item);
            }
            
            VisibleItems.Clear();
        }

        public void Return<T>(T returnItem) where T : Behaviour
        {
            int indx = VisibleItems.FindIndex(behaviour => behaviour == returnItem);
            if (indx == -1)
            {
                return;
            }

            ReturnToQueue(returnItem);
            
            VisibleItems.RemoveAt(indx);
        }

        private void ReturnToQueue<T>(T item) where T : Behaviour
        {
            item.Hide();
            QueuedItems.Enqueue(item);
        }
    }
}