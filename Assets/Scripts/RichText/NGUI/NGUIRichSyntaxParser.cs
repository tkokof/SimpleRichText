// desc ngui rich syntax parser
// maintainer hugoyu

#if RICH_TEXT_ENABLE_NGUI

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RichText
{

    public class NGUIRichSyntaxParser : RichSyntaxParser
    {

        protected string m_defaultFontStyle = "default";
        protected string m_currentFontStyle;
        protected bool m_isUnderline = false;
        protected string m_url;

        protected Dictionary<string, Action<RichSyntaxData>> m_syntaxHandlers;

        public NGUIRichSyntaxParser()
        {
            // initialize syntax handlers
            m_syntaxHandlers = new Dictionary<string, Action<RichSyntaxData>>
            {
                { "size", OnSizeSyntax },
                { "style", OnStyleSyntax },
                { "break", OnBreakSyntax },
                { "br", OnBreakSyntax },
                { "image", OnImageSyntax },
                { "underline", OnUnderlineBeginSyntax },
                { "u", OnUnderlineBeginSyntax },
                { "/underline", OnUnderlineEndSyntax },
                { "/u", OnUnderlineEndSyntax },
                { "url", OnURLBeginSyntax },
                { "/url", OnURLEndSyntax },
                // other handlers goes here ...
            };
        }

        protected string GetFontStyle()
        {
            if (!string.IsNullOrEmpty(m_currentFontStyle))
            {
                return m_currentFontStyle;
            }
            else
            {
                return m_defaultFontStyle;
            }
        }

        protected override void Reset()
        {
            base.Reset();
            m_currentFontStyle = null;
            m_isUnderline = false;
        }

        protected override void OnParseBegin()
        {
            Debug.Log("[NGUIRichSyntaxParser]Parse Begin");
        }

        protected override void OnParseEnd()
        {
            Debug.Log("[NGUIRichSyntaxParser]Parse End");
        }

        protected override void OnSyntax(RichSyntaxData syntaxData)
        {
            if (syntaxData != null && syntaxData.IsValid())
            {
                var syntaxName = syntaxData.GetName();
                if (m_syntaxHandlers.ContainsKey(syntaxName))
                {
                    m_syntaxHandlers[syntaxName](syntaxData);
                }
                else
                {
                    Debug.LogError("[NGUIRichSyntaxParser]Can not handle syntax : " + syntaxName);
                }
            }
        }

        protected NGUIRichElementText CreateTextElement(string text, string fontStyle, bool underline, string url)
        {
            Action openURLHandler = null;

            if (!string.IsNullOrEmpty(url))
            {
                openURLHandler = () => { RichTextUtil.OpenURL(url); };
            }

            if (underline)
            {
                return new NGUIRichElementTextUnderline(text, fontStyle, openURLHandler);
            }
            else
            {
                return new NGUIRichElementText(text, fontStyle, openURLHandler);
            }
        }

        protected NGUIRichElementImage CreateImageElement(string image, string url)
        {
            Action openURLHandler = null;
            if (!string.IsNullOrEmpty(url))
            {
                openURLHandler = () => { RichTextUtil.OpenURL(url); };
            }

            return new NGUIRichElementImage(image, openURLHandler);
        }

        protected NGUIRichElementImageSizeable CreateImageSizeableElement(string image, string url, Vector2 size)
        {
            Action openURLHandler = null;
            if (!string.IsNullOrEmpty(url))
            {
                openURLHandler = () => { RichTextUtil.OpenURL(url); };
            }

            return new NGUIRichElementImageSizeable(image, openURLHandler, size);
        }

        protected override void OnText(string text)
        {
            if (m_richText)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    m_richText.AddRichElement(CreateTextElement(text, GetFontStyle(), m_isUnderline, m_url));
                }
            }
        }

        protected void OnSizeSyntax(RichSyntaxData syntaxData)
        {
            if (m_richText)
            {
                var width = syntaxData.GetParamFloat("width");
                // TODO should not be called "height"
                var height = syntaxData.GetParamFloat("height");
                m_richText.SetTextWidth(width);
                m_richText.SetVerticalSpace(height);
            }
        }

        protected void OnStyleSyntax(RichSyntaxData syntaxData)
        {
            m_currentFontStyle = syntaxData.GetParamString("value");
        }

        protected void OnBreakSyntax(RichSyntaxData syntaxData)
        {
            if (m_richText)
            {
                m_richText.AddRichElement(new UGUIRichElementNewline());
            }
        }

        protected void OnImageSyntax(RichSyntaxData syntaxData)
        {
            if (m_richText)
            {
                var image = syntaxData.GetParamString("name");
                Debug.Assert(image != null);
                if (syntaxData.HasParam("size"))
                {
                    m_richText.AddRichElement(CreateImageSizeableElement(image, m_url, syntaxData.GetParamVector2("size")));
                }
                else
                {
                    m_richText.AddRichElement(CreateImageElement(image, m_url));
                }
            }
        }

        protected void OnUnderlineBeginSyntax(RichSyntaxData syntaxData)
        {
            m_isUnderline = true;
        }

        protected void OnUnderlineEndSyntax(RichSyntaxData syntaxData)
        {
            m_isUnderline = false;
        }

        protected void OnURLBeginSyntax(RichSyntaxData syntaxData)
        {
            m_url = syntaxData.GetParamString("value");
        }

        protected void OnURLEndSyntax(RichSyntaxData syntaxData)
        {
            m_url = null;
        }

        // TODO other syntax handlers here ...

    }

}

#endif