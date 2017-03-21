// desc simple implementation of underline text of NGUI
// maintainer hugoyu

#if RICH_TEXT_ENABLE_NGUI

using System;
using System.Collections;
using UnityEngine;

namespace RichText
{

    public class NGUIRichElementTextUnderline : NGUIRichElementText
    {

        public NGUIRichElementTextUnderline(string text, string style, Action clickHandler)
            : base(text, style, clickHandler)
        {
        }

        public override RichElementProxy CreateElementProxy()
        {
            return new NGUIRichElementTextUnderlineProxy();
        }

    }

    public class NGUIRichElementTextUnderlineProxy : NGUIRichElementTextProxy
    {

        public override GameObject Create(string text, string style, Action clickHandler)
        {
            var textGO = base.Create(text, style, clickHandler);
            if (textGO)
            {
                var textComp = textGO.GetComponent<UILabel>();
                textComp.text = "[u]" + textComp.text + "[/u]";
            }

            return textGO;
        }

        public override void SetText(GameObject gameObject, string text)
        {
            if (gameObject != null)
            {
                var textComp = gameObject.GetComponent<UILabel>();
                textComp.text = "[u]" + text + "[/u]";
            }
        }

    }

}

#endif