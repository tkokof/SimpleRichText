// desc ngui rich element
// maintainer hugoyu

#if RICH_TEXT_ENABLE_NGUI

using System;
using UnityEngine;

namespace RichText
{

    public class NGUIRichElementText : RichElementText
    {

        public NGUIRichElementText(string text, string style, Action clickHandler)
            : base(text, style, clickHandler)
        {
        }

        public override RichElementProxy CreateElementProxy()
        {
            return new NGUIRichElementTextProxy();
        }

    }

    public class NGUIRichElementImage : RichElementImage
    {

        public NGUIRichElementImage(string image, Action clickHandler)
            : base(image, clickHandler)
        {
        }

        public override RichElementProxy CreateElementProxy()
        {
            return new NGUIRichElementImageProxy();
        }

    }

    public class NGUIRichElementCustom : RichElementCustom
    {

        public NGUIRichElementCustom(GameObject gameObject)
            : base(gameObject)
        {
        }

        public override RichElementProxy CreateElementProxy()
        {
            return new NGUIRichElementCustomProxy();
        }

    }

    // NOTE just for unified
    public class NGUIRichElementNewline : RichElementNewline
    {
    }

}

#endif