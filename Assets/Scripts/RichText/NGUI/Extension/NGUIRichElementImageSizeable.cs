// desc simple implementation of ngui sizeable image
// maintainer hugoyu

#if RICH_TEXT_ENABLE_NGUI

using System;
using System.Collections;
using UnityEngine;

namespace RichText
{

    public class NGUIRichElementImageSizeable : NGUIRichElementImage
    {

        public NGUIRichElementImageSizeable(string image, Action clickHandler, Vector2 size)
            : base(image, clickHandler)
        {
            m_size = size;
        }

        public void SetSize(Vector2 size)
        {
            m_size = size;
        }

        public Vector2 GetSize()
        {
            return m_size;
        }

        public override RichElementProxy CreateElementProxy()
        {
            return new NGUIRichElementImageSizeableProxy();
        }

        protected Vector2 m_size;

    }

    public class NGUIRichElementImageSizeableProxy : NGUIRichElementImageProxy
    {

        public override GameObject Create(RichElement element)
        {
            var imageSizeableElement = element as NGUIRichElementImageSizeable;
            if (imageSizeableElement != null)
            {
                var gameObject = base.Create(element);
                if (gameObject != null)
                {
                    var uiSprite = gameObject.GetComponent<UISprite>();
                    if (uiSprite != null)
                    {
                        var size = imageSizeableElement.GetSize();
                        uiSprite.SetDimensions((int)size.x, (int)size.y);
                    }
                }

                return gameObject;
            }

            return null;
        }

    }

}

#endif