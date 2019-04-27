// desc simple rich text implementation
// maintainer hugoyu

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RichText
{
    public enum RichTextPivot
    {
        TopLeft,
        Top,
        TopRight,
        Left,
        Center,
        Right,
        BottomLeft,
        Bottom,
        BottomRight,
    }

    public class RichText : MonoBehaviour
    {
        void Awake()
        {
            // create pivot transform
            CreatePivot();
        }

        public virtual void AddRichElement(RichElement element)
        {
            if (element != null)
            {
                m_richElements.Add(element);
            }
        }
        
        public virtual void RemoveRichElement(RichElement element)
        {
            if (element != null)
            {
                m_richElements.Remove(element);
            }
        }

        public virtual void ClearRichElements()
        {
            m_richElements.Clear();
        }

        public virtual int GetRichElementCount()
        {
            return m_richElements.Count;
        }

        public virtual RichElement GetRichElement(int index)
        {
            if (index >= 0 && index < m_richElements.Count)
            {
                return m_richElements[index];
            }

            return null;
        }

        public virtual List<GameObject> GetRichElementRenderers(RichElement element)
        {
            if (element != null)
            {
                if (m_richElementRenderers.ContainsKey(element))
                {
                    return new List<GameObject>(m_richElementRenderers[element]);
                }
            }

            return null;
        }

        protected void DestroyChildren()
        {
            var iter = m_richElementRenderers.GetEnumerator();
            while (iter.MoveNext())
            {
                var element = iter.Current.Key;
                var elementRenders = iter.Current.Value;
                if (elementRenders != null)
                {
                    for (int i = 0; i < elementRenders.Count; ++i)
                    {
                        var proxy = RichTextManager.GetOrCreateProxy(element);
                        if (proxy != null)
                        {
                            proxy.Destroy(element, elementRenders[i]);
                        }
                    }
                    elementRenders.Clear();
                }
            }

            Debug.Assert(m_pivotTransform.childCount == 0);
        }

        // NOTE not so sure about this ...
        protected virtual void ResetFormat()
        {
            DestroyChildren();

            m_lineLeftWidth = m_textWidth;
            m_textDrawWidth = 0;
            m_textDrawHeight = 0;
            m_lineMaxWidth = 0;
            m_lineMaxHeight = 0;
            m_renderPos = Vector3.zero;
        }

        public virtual void Format()
        {
            // reset format first
            ResetFormat();

            bool handleRet = true;

            // then format other elements
            for (int i = 0; i < m_richElements.Count; ++i)
            {
                var element = m_richElements[i];
                var elementType = element.GetElementType();
                var elementProxy = RichTextManager.GetOrCreateProxy(element);
                if (elementProxy != null)
                {
                    switch (elementType)
                    {
                        case RichElementType.Text:
                            handleRet &= HandleText(element as RichElementText, elementProxy as RichElementTextProxy);
                            break;
                        case RichElementType.Image:
                            handleRet &= HandleImage(element as RichElementImage, elementProxy as RichElementImageProxy);
                            break;
                        case RichElementType.Custom:
                            handleRet &= HandleCustom(element as RichElementCustom, elementProxy as RichElementCustomProxy);
                            break;
                        case RichElementType.Newline:
                            handleRet &= HandleNewline();
                            break;
                    }

                    if (!handleRet)
                    {
                        ResetFormat();
                        break;
                    }
                }
            }

            if (handleRet)
            {
                HandlePivot();
            }
        }

        protected void AddElementRenderer(RichElement element, GameObject renderer)
        {
            if (m_richElementRenderers.ContainsKey(element))
            {
                m_richElementRenderers[element].Add(renderer);
            }
            else
            {
                m_richElementRenderers[element] = new List<GameObject> { renderer };
            }
        }

        // NOTE now we always anchor of game object is at the top-left,
        //      also we assume rich text anchor is at the top-left
        // TODO improve
        protected bool HandleTextRecur(string text, string style, Action clickHandler, RichElementText element, RichElementTextProxy proxy)
        {
            // param check
            if (string.IsNullOrEmpty(text))
            {
                return true;
            }

            if (m_lineLeftWidth <= 0)
            {
                HandleNewline();
            }

            var textGO = proxy.Create(text, style, clickHandler);
            if (!textGO)
            {
                Debug.LogError("[RichText]Unable to create rich element : " + element.ToString());
                return false;
            }

            // NOTE we need to add it to textGO immediately for retrieving correct size
            textGO.transform.SetParent(m_pivotTransform, false);
            AddElementRenderer(element, textGO);

            var size = proxy.GetSize(textGO);

            if (size.x < m_lineLeftWidth)
            {
                // enough width space, just add game object
                textGO.transform.localPosition = m_renderPos;

                // update control data
                var oldMaxHeight = m_lineMaxHeight;
                m_lineMaxHeight = size.y > m_lineMaxHeight ? size.y : m_lineMaxHeight;
                m_textDrawHeight = m_textDrawHeight - oldMaxHeight + m_lineMaxHeight;
                m_lineMaxWidth += size.x;
                m_textDrawWidth = m_lineMaxWidth > m_textDrawWidth ? m_lineMaxWidth : m_textDrawWidth;
                m_lineLeftWidth -= size.x;
                m_renderPos += new Vector3(size.x, 0, 0);

                return true;
            }
            else
            {
                // not enough space, have to adjust
                float factor = m_lineLeftWidth / size.x;
                int subStrLength = Mathf.FloorToInt(factor * text.Length);
                if (subStrLength <= 0)
                {
                    HandleNewline();

                    // recalculate sub string length
                    factor = m_lineLeftWidth / size.x;
                    factor = Mathf.Min(factor, 1);
                    subStrLength = Mathf.FloorToInt(factor * text.Length);

                    // still not enough space, seems ill situation
                    if (subStrLength <= 0)
                    {
                        // NOTE not so sure about this ...
                        Debug.LogWarning("[RichText]Incorrect to handle rich text here, seems width is too small ...");
                        // we reset sub string length here
                        // NOTE not so sure about this ...
                        subStrLength = 1;
                    }
                }

                string subStr = text.Substring(0, subStrLength);
                proxy.SetText(textGO, subStr);

                // check width after adjust text
                var adjustSize = proxy.GetSize(textGO);
                if (adjustSize.x > m_lineLeftWidth)
                {
                    // still out of width
                    for (int i = subStrLength - 1; i > 0; --i)
                    {
                        subStr = text.Substring(0, i);
                        proxy.SetText(textGO, subStr);
                        adjustSize = proxy.GetSize(textGO);
                        if (adjustSize.x <= m_lineLeftWidth)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    // enough space, but we need to search most adjust
                    for (int i = subStrLength + 1; i < text.Length; ++i)
                    {
                        subStr = text.Substring(0, i);
                        proxy.SetText(textGO, subStr);
                        adjustSize = proxy.GetSize(textGO);
                        if (adjustSize.x > m_lineLeftWidth)
                        {
                            // roll back
                            subStr = text.Substring(0, i - 1);
                            proxy.SetText(textGO, subStr);
                            adjustSize = proxy.GetSize(textGO);
                            break;
                        }
                    }
                }

                textGO.transform.localPosition = m_renderPos;

                // update control data
                m_renderPos += new Vector3(adjustSize.x, 0, 0);
                var oldMaxHeight = m_lineMaxHeight;
                m_lineMaxHeight = size.y > m_lineMaxHeight ? size.y : m_lineMaxHeight;
                m_textDrawHeight = m_textDrawHeight - oldMaxHeight + m_lineMaxHeight;
                m_lineMaxWidth += adjustSize.x;
                m_textDrawWidth = m_lineMaxWidth > m_textDrawWidth ? m_lineMaxWidth : m_textDrawWidth;
                m_lineLeftWidth -= adjustSize.x;

                return HandleTextRecur(text.Substring(subStr.Length, text.Length - subStr.Length), style, clickHandler, element, proxy);
            }
        }

        protected bool HandleText(RichElementText element, RichElementTextProxy proxy)
        {
            if (proxy != null)
            {
                return HandleTextRecur(element.GetText(), element.GetStyle(), element.GetClickHandler(), element, proxy);
            }

            return false;
        }

        protected bool HandleImage(RichElementImage element, RichElementImageProxy proxy)
        {
            if (proxy != null)
            {
                var imageName = element.GetImage();

                // param check
                if (string.IsNullOrEmpty(imageName))
                {
                    return true;
                }

                if (m_lineLeftWidth <= 0)
                {
                    HandleNewline();
                }

                var imageGO = proxy.Create(element);
                if (!imageGO)
                {
                    Debug.LogError("[RichText]Unable to create rich element : " + element.ToString());
                    return false;
                }

                // NOTE we need to add it to imageGO immediately for retrieving correct size
                imageGO.transform.SetParent(m_pivotTransform, false);
                AddElementRenderer(element, imageGO);

                var imageSize = proxy.GetSize(imageGO);

                // NOTE special handle when image size is big than text width :
                //      1. when it is new line, then just put image here and add new line,
                //      2. otherwise add new line, then do 1
                if (imageSize.x > m_textWidth)
                {
                    Debug.LogWarning("[RichText]Incorrect to handle rich text here, seems width is too small or image size is too large ...");
                    // we reset image size here
                    // NOTE not so sure about this ...
                    //imageSize.x = m_textWidth;
                }

                if (imageSize.x > m_lineLeftWidth)
                {
                    if (imageSize.x <= m_textWidth)
                    {
                        HandleNewline();
                    }
                    //Debug.Assert(imageSize.x <= m_lineLeftWidth);
                }
                
                imageGO.transform.localPosition = m_renderPos;

                // update control data
                m_renderPos += new Vector3(imageSize.x, 0, 0);
                var oldMaxHeight = m_lineMaxHeight;
                m_lineMaxHeight = imageSize.y > m_lineMaxHeight ? imageSize.y : m_lineMaxHeight;
                m_textDrawHeight = m_textDrawHeight - oldMaxHeight + m_lineMaxHeight;
                m_lineMaxWidth += imageSize.x;
                m_textDrawWidth = m_lineMaxWidth > m_textDrawWidth ? m_lineMaxWidth : m_textDrawWidth;
                m_lineLeftWidth -= imageSize.x;

                return true;
            }

            return false;
        }

        protected bool HandleCustom(RichElementCustom element, RichElementCustomProxy proxy)
        {
            if (proxy != null)
            {
                var customGOPrototype = element.GetGameObject();

                // param check
                if (!customGOPrototype)
                {
                    return true;
                }

                if (m_lineLeftWidth <= 0)
                {
                    HandleNewline();
                }

                var customGO = proxy.Create(element);
                if (!customGO)
                {
                    Debug.LogError("[RichText]Unable to create rich element : " + element.ToString());
                    return false;
                }

                // NOTE we need to add it to customGO immediately for retrieving correct size
                customGO.transform.SetParent(m_pivotTransform, false);
                AddElementRenderer(element, customGO);

                var customSize = proxy.GetSize(customGO);
                if (customSize.x > m_textWidth)
                {
                    Debug.LogWarning("[RichText]Incorrect to handle rich text here, seems width is too small or custom size is too large ...");
                    // we reset custom size here
                    // NOTE not so sure about this ...
                    //customSize.x = m_textWidth;
                }

                if (customSize.x > m_lineLeftWidth)
                {
                    if (customSize.x <= m_textWidth)
                    {
                        HandleNewline();
                    }
                    //Debug.Assert(customSize.x <= m_lineLeftWidth);
                }

                customGO.transform.localPosition = m_renderPos;

                // update control data
                m_renderPos += new Vector3(customSize.x, 0, 0);
                var oldMaxHeight = m_lineMaxHeight;
                m_lineMaxHeight = customSize.y > m_lineMaxHeight ? customSize.y : m_lineMaxHeight;
                m_textDrawHeight = m_textDrawHeight - oldMaxHeight + m_lineMaxHeight;
                m_lineMaxWidth += customSize.x;
                m_textDrawWidth = m_lineMaxWidth > m_textDrawWidth ? m_lineMaxWidth : m_textDrawWidth;
                m_lineLeftWidth -= customSize.x;

                return true;
            }

            return false;
        }

        protected bool HandleNewline()
        {
            m_renderPos = new Vector3(0, m_renderPos.y - m_lineMaxHeight - m_verticalSpace, 0);
            m_lineLeftWidth = m_textWidth;
            m_textDrawHeight += m_verticalSpace;
            m_lineMaxHeight = 0;

            return true;
        }

        public virtual float GetTextWidth()
        {
            return m_textWidth;
        }

        public virtual void SetTextWidth(float textWidth)
        {
            m_textWidth = textWidth;
        }

        public virtual float GetVerticalSpace()
        {
            return m_verticalSpace;
        }

        public virtual void SetVerticalSpace(float space)
        {
            m_verticalSpace = space;
        }

        public virtual float GetTextDrawWidth()
        {
            return m_textDrawWidth;
        }

        public virtual float GetTextDrawHeight()
        {
            return m_textDrawHeight;
        }

        protected void CreatePivot()
        {
            RichTextUtil.ReleaseGameObject(m_pivotTransform);
            var pivotGO = RichTextUtil.CreateGameObject("Pivot", gameObject);
            Debug.Assert(pivotGO != null);
            m_pivotTransform = pivotGO.transform;
        }

        protected void HandlePivot()
        {
            switch (m_pivot)
            {
                case RichTextPivot.TopLeft:
                    m_pivotTransform.localPosition = Vector3.zero;
                    break;
                case RichTextPivot.Top:
                    m_pivotTransform.localPosition = new Vector3(-m_textDrawWidth * 0.5f, 0, 0);
                    break;
                case RichTextPivot.TopRight:
                    m_pivotTransform.localPosition = new Vector3(-m_textDrawWidth, 0, 0);
                    break;
                case RichTextPivot.Left:
                    m_pivotTransform.localPosition = new Vector3(0, m_textDrawHeight * 0.5f, 0);
                    break;
                case RichTextPivot.Center:
                    m_pivotTransform.localPosition = new Vector3(-m_textDrawWidth * 0.5f, m_textDrawHeight * 0.5f, 0);
                    break;
                case RichTextPivot.Right:
                    m_pivotTransform.localPosition = new Vector3(-m_textDrawWidth, m_textDrawHeight * 0.5f, 0);
                    break;
                case RichTextPivot.BottomLeft:
                    m_pivotTransform.localPosition = new Vector3(0, m_textDrawHeight, 0);
                    break;
                case RichTextPivot.Bottom:
                    m_pivotTransform.localPosition = new Vector3(-m_textDrawWidth * 0.5f, m_textDrawHeight, 0);
                    break;
                case RichTextPivot.BottomRight:
                    m_pivotTransform.localPosition = new Vector3(-m_textDrawWidth, m_textDrawHeight, 0);
                    break;
            }
        }

        public void SetPivot(RichTextPivot pivot)
        {
            if (m_pivot != pivot)
            {
                m_pivot = pivot;
                HandlePivot();
            }
        }

        public RichTextPivot GetPivot()
        {
            return m_pivot;
        }

        protected float m_lineLeftWidth;
        protected float m_lineMaxWidth;
        protected float m_lineMaxHeight;
        protected Vector3 m_renderPos;

        protected Transform m_pivotTransform;
        [SerializeField]
        protected RichTextPivot m_pivot = RichTextPivot.TopLeft;

        protected float m_textWidth;
        protected float m_textDrawWidth;
        protected float m_textDrawHeight;
        protected float m_verticalSpace;
        protected List<RichElement> m_richElements = new List<RichElement>();
        protected Dictionary<RichElement, List<GameObject>> m_richElementRenderers = new Dictionary<RichElement, List<GameObject>>();
    }
}