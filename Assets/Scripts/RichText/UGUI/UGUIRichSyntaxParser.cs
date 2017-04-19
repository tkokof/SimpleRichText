// desc ugui rich syntax parser
// maintainer hugoyu

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RichText
{

    public class UGUIRichSyntaxParser : RichSyntaxParser
    {

        protected string m_defaultFontStyle = "default";
        protected string m_currentFontStyle;
        protected bool m_isUnderline = false;
        protected string m_url;

        protected Dictionary<string, Action<RichSyntaxData>> m_syntaxHandlers;

        public UGUIRichSyntaxParser()
        {
            // initialize syntax handlers
            m_syntaxHandlers = new Dictionary<string,Action<RichSyntaxData>>
            {
                { "size", OnSizeSyntax },
                { "style", OnStyleSyntax },
                { "break", OnBreakSyntax },
                { "br", OnBreakSyntax },
                { "image", OnImageSyntax },
                { "animimage", OnAnimImageSyntax },
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
                    Debug.LogError("[UGUIRichSyntaxParser]Can not handle syntax : " + syntaxName);
                }
            }
        }

        protected UGUIRichElementText CreateTextElement(string text, string fontStyle, bool underline, string url)
        {
            Action openURLHandler = null;

            if (!string.IsNullOrEmpty(url))
            {
                openURLHandler = () => { RichTextUtil.OpenURL(url); };
            }

            if (underline)
            {
                return new UGUIRichElementTextUnderline(text, fontStyle, openURLHandler);
            }
            else
            {
                return new UGUIRichElementText(text, fontStyle, openURLHandler);
            }
        }

        protected UGUIRichElementImage CreateImageElement(string image, string url)
        {
            Action openURLHandler = null;
            if (!string.IsNullOrEmpty(url))
            {
                openURLHandler = () => { RichTextUtil.OpenURL(url); };
            }
            
            return new UGUIRichElementImage(image, openURLHandler);
        }

        protected UGUIRichElementAnimationImage CreateAnimImageElement(string image, string url, uint count, float fps)
        {
            Action openURLHandler = null;
            if (!string.IsNullOrEmpty(url))
            {
                openURLHandler = () => { RichTextUtil.OpenURL(url); };
            }

            return new UGUIRichElementAnimationImage(image, openURLHandler, count, fps);
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
                m_richText.AddRichElement(CreateImageElement(image, m_url));
            }
        }

        protected void OnAnimImageSyntax(RichSyntaxData syntaxData)
        {
            if (m_richText)
            {
                var imagePrefix = syntaxData.GetParamString("name");
                var imageCount = syntaxData.GetParamUInt("count");
                var animFPS = syntaxData.GetParamFloat("fps", 30);
                m_richText.AddRichElement(CreateAnimImageElement(imagePrefix, m_url, imageCount, animFPS));
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
