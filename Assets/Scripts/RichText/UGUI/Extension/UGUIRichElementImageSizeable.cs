// desc simple implementation of ugui sizeable image
// maintainer hugoyu

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RichText
{

    public class UGUIRichElementImageSizeable : UGUIRichElementImage
    {

        public UGUIRichElementImageSizeable(string image, Action clickHandler, Vector2 size)
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
            return new UGUIRichElementImageSizeableProxy();
        }

        protected Vector2 m_size;

    }

    public class UGUIRichElementImageSizeableProxy : UGUIRichElementImageProxy
    {

        public override GameObject Create(RichElement element)
        {
            var imageSizeableElement = element as UGUIRichElementImageSizeable;
            if (imageSizeableElement != null)
            {
                var gameObject = base.Create(element);
                if (gameObject != null)
                {
                    var image = gameObject.GetComponent<Image>();
                    if (image != null)
                    {
                        // first delete ContentSizeFitter
                        // TODO improve ?
                        RichTextUtil.DestroyComponent<ContentSizeFitter>(gameObject);
                        // then set size
                        var size = imageSizeableElement.GetSize();
                        image.rectTransform.sizeDelta = size;
                    }
                }

                return gameObject;
            }

            return null;
        }

        public override Vector2 GetSize(GameObject gameObject)
        {
            if (gameObject != null)
            {
                var image = gameObject.GetComponent<Image>();
                if (image != null)
                {
                    return image.rectTransform.sizeDelta;
                }
            }

            return base.GetSize(gameObject);
        }

    }

}