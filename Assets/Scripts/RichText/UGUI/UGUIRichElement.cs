// desc ugui rich element implementation
// maintainer hugoyu

using System;
using UnityEngine;

namespace RichText
{

    public class UGUIRichElementText : RichElementText
    {

        public UGUIRichElementText(string text, string style, Action clickHandler) : base(text, style, clickHandler)
        {
        }

        public override RichElementProxy CreateElementProxy()
        {
            return new UGUIRichElementTextProxy();
        }
        
    }

    public class UGUIRichElementImage : RichElementImage
    {

        public UGUIRichElementImage(string image, Action clickHandler) : base(image, clickHandler)
        {
        }

        public override RichElementProxy CreateElementProxy()
        {
            return new UGUIRichElementImageProxy();
        }

    }

    public class UGUIRichElementCustom : RichElementCustom
    {

        public UGUIRichElementCustom(GameObject gameObject) : base(gameObject)
        {
        }

        public override RichElementProxy CreateElementProxy()
        {
            return new UGUIRichElementCustomProxy();
        }

    }

    // NOTE just for unified
    public class UGUIRichElementNewline : RichElementNewline
    {
    }

}
