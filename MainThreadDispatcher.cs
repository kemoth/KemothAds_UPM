using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KemothStudios.KemothAds
{
    public class MainThreadDispatcher : MonoBehaviour
    {
        private static readonly Queue<Action> _executionQueue = new Queue<Action>();

        private void Update()
        {
            lock (_executionQueue)
            {
                while (_executionQueue.Count > 0)
                {
                    _executionQueue.Dequeue()?.Invoke();
                }
            }
        }

        internal void RunOnMainThread(Action action)
        {
            lock (_executionQueue)
            {
                _executionQueue.Enqueue(()=>StartCoroutine(Execute(action)));
            }
        }

        private IEnumerator Execute(Action action)
        {
            action?.Invoke();
            yield return null;
        }
    }
}
