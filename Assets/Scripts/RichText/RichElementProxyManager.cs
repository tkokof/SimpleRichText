// desc rich element manager
// maintainer hugoyu

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RichText
{

    public class RichElementProxyManager
    {

        public void RegisterProxy(Type type, RichElementProxy proxy)
        {
            if (!m_richElementProxies.ContainsKey(type))
            {
                m_richElementProxies[type] = proxy;
            }
        }

        public void UnregsiterProxy(Type type)
        {
            m_richElementProxies.Remove(type);
        }

        public void ClearProxies()
        {
            m_richElementProxies.Clear();
        }

        public RichElementProxy GetOrCreateProxy(RichElement element)
        {
            Debug.Assert(element != null);
            var type = element.GetType();
            if (m_richElementProxies.ContainsKey(type))
            {
                return m_richElementProxies[type];
            }
            else
            {
                var proxy = element.CreateElementProxy();
                m_richElementProxies[type] = proxy;
                return proxy;
            }
        }

        Dictionary<Type, RichElementProxy> m_richElementProxies = new Dictionary<Type, RichElementProxy>();

    }

}