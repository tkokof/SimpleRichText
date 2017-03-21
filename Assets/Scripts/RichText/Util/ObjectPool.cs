// desc simple game object pool implementation
// maintainer hugoyu

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RichText
{

    public class ObjectPool
    {

        public ObjectPool(Action<GameObject> actionOnSpawn, Action<GameObject> actionOnRelease)
        {
            m_actionOnSpawn = actionOnSpawn;
            m_actionOnRelease = actionOnRelease;
        }

        public GameObject Spawn(GameObject prefab)
        {
            GameObject element;
            if (m_stack.Count == 0)
            {
                element = UnityEngine.Object.Instantiate<GameObject>(prefab);
            }
            else
            {
                element = m_stack.Pop();
            }

            if (m_actionOnSpawn != null)
            {
                m_actionOnSpawn(element);
            }

            return element;
        }

        public void Release(GameObject element)
        {
            if (m_stack.Count > 0 && ReferenceEquals(m_stack.Peek(), element))
            {
                Debug.LogError("[ObjectPool]Internal error. Trying to destroy object that is already released to pool.");
            }

            if (m_actionOnRelease != null)
            {
                m_actionOnRelease(element);
            }

            m_stack.Push(element);
        }

        public void Clear()
        {
            var iter = m_stack.GetEnumerator();
            while (iter.MoveNext())
            {
                var current = iter.Current;
                UnityEngine.Object.Destroy(current);
            }

            m_stack.Clear();
        }

        readonly Stack<GameObject> m_stack = new Stack<GameObject>();
        readonly Action<GameObject> m_actionOnSpawn;
        readonly Action<GameObject> m_actionOnRelease;

    }

}
