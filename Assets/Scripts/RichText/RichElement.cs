// desc rich text element implementation
// maintainer hugoyu

using System;
using UnityEngine;

namespace RichText
{

    public enum RichElementType
    {
        Text,
        Image,
        Custom,
        Newline,
    }

    public abstract class RichElement
    {

        public abstract RichElementType GetElementType();
        public abstract RichElementProxy CreateElementProxy();

    }

    public abstract class RichElementText : RichElement
    {

        public RichElementText(string text, string style, Action clickHandler)
        {
            m_text = text;
            m_style = style;
            m_clickHandler = clickHandler;
        }

        public override RichElementType GetElementType()
        {
            return RichElementType.Text;
        }

        public string GetText()
        {
            return m_text;
        }

        public string GetStyle()
        {
            return m_style;
        }

        public Action GetClickHandler()
        {
            return m_clickHandler;
        }

        public override string ToString()
        {
            return "[RichElementText]Text : " + m_text + " Style : " + m_style;
        }

        protected string m_text;
        protected string m_style;
        protected Action m_clickHandler;

    }

    public abstract class RichElementImage : RichElement
    {

        public RichElementImage(string image, Action clickHandler)
        {
            m_image = image;
            m_clickHandler = clickHandler;
        }

        public override RichElementType GetElementType()
        {
            return RichElementType.Image;
        }

        public string GetImage()
        {
            return m_image;
        }

        public Action GetClickHandler()
        {
            return m_clickHandler;
        }

        public override string ToString()
        {
            return "[RichElementImage]Name : " + m_image;
        }

        protected string m_image;
        protected Action m_clickHandler;
    }

    public abstract class RichElementCustom : RichElement
    {

        public RichElementCustom(GameObject gameObject)
        {
            m_gameObject = gameObject;
        }

        public override RichElementType GetElementType()
        {
            return RichElementType.Custom;
        }

        public GameObject GetGameObject()
        {
            return m_gameObject;
        }

        public override string ToString()
        {
            return "[RichElementCustom]GameObject : " + m_gameObject != null ? m_gameObject.ToString() : "None";
        }

        protected GameObject m_gameObject;

    }

    public class RichElementNewline : RichElement
    {

        public override RichElementType GetElementType()
        {
            return RichElementType.Newline;
        }

        public override RichElementProxy CreateElementProxy()
        {
            return new RichElementNewlineProxy();
        }

        public override string ToString()
        {
            return "[RichElementNewline]";
        }

    }

}