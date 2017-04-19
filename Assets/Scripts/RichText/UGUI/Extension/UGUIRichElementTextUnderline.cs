// desc simple implementation of underline text of UGUI
// maintainer hugoyu

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RichText
{

    public class UGUIRichElementTextUnderline : UGUIRichElementText
    {

        public UGUIRichElementTextUnderline(string text, string style, Action clickHandler)
            : base(text, style, clickHandler)
        {
        }

        public override RichElementProxy CreateElementProxy()
        {
            return new UGUIRichElementTextUnderlineProxy();
        }

    }

    public class UGUIRichElementTextUnderlineProxy : UGUIRichElementTextProxy
    {

        public override GameObject Create(string text, string style, Action clickHandler)
        {
            var textGO = base.Create(text, style, clickHandler);
            UGUIUtil.CreateTextUnderline(textGO);

            return textGO;
        }

        public override void Destroy(RichElement element, GameObject gameObject)
        {
            if (gameObject != null)
            {
                UGUIUtil.DestroyTextUnderline(gameObject);
                base.Destroy(element, gameObject);
            }
        }

    }


}
