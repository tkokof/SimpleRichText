// desc simple ngui click handler
// maintainer hugoyu

#if RICH_TEXT_ENABLE_NGUI

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class NGUIClickHandler : MonoBehaviour
{

    public void AddHandler(Action handler)
    {
        m_handlers.Add(handler);
    }

    public void RemoveHandler(Action handler)
    {
        m_handlers.Remove(handler);
    }

    public void ClearHandlers()
    {
        m_handlers.Clear();
    }

    public void OnClick()
    {
        m_handlersBuffer.Clear();
        m_handlersBuffer.AddRange(m_handlers);

        for (int i = 0; i < m_handlersBuffer.Count; ++i)
        {
            if (m_handlersBuffer[i] != null)
            {
                m_handlersBuffer[i]();
            }
        }
    }

    List<Action> m_handlers = new List<Action>();
    List<Action> m_handlersBuffer = new List<Action>();
	
}

#endif