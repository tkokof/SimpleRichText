// desc simple implementation of ngui animation image
// maintainer hugoyu

using System;
using System.Collections;
using UnityEngine;

namespace RichText
{

    public class NGUIRichElementAnimationImage : NGUIRichElementImage
    {

        public NGUIRichElementAnimationImage(string imagePrefix, Action clickHandler, uint imageCount, float animFPS)
            : base(imagePrefix, clickHandler)
        {
            m_imageCount = imageCount;
            m_animFPS = animFPS;
        }

        public override RichElementProxy CreateElementProxy()
        {
            return new NGUIRichElementAnimationImageProxy();
        }

        public uint GetImageCount()
        {
            return m_imageCount;
        }

        public float GetAnimFPS()
        {
            return m_animFPS;
        }

        protected uint m_imageCount;
        protected float m_animFPS;

    }

    public class NGUIRichElementAnimationImageProxy : NGUIRichElementImageProxy
    {

        public override GameObject Create(RichElement element)
        {
            var imageGO = base.Create(element);
            if (imageGO != null)
            {
                var animationImage = element as NGUIRichElementAnimationImage;
                Debug.Assert(animationImage != null);

                NGUIRichTextUtil.CreateAnimationImage(imageGO, animationImage.GetImage(), animationImage.GetImageCount(), animationImage.GetAnimFPS());
            }

            return imageGO;
        }

        public override void Destroy(RichElement element, GameObject gameObject)
        {
            if (gameObject)
            {
                NGUIRichTextUtil.DestroyAnimationImage(gameObject);
                base.Destroy(element, gameObject);
            }
        }

    }


}