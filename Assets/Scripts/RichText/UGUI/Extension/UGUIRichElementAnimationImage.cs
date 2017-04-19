// desc simple implementation of animation image
// maintainer hugoyu

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RichText
{

    public class UGUIRichElementAnimationImage: UGUIRichElementImage
    {

        public UGUIRichElementAnimationImage(string imagePrefix, Action clickHandler, uint imageCount, float animFPS)
            : base(imagePrefix, clickHandler)
        {
            m_imageCount = imageCount;
            m_animFPS = animFPS;
        }

        public override RichElementProxy CreateElementProxy()
        {
            return new UGUIRichElementAnimationImageProxy();
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

    public class UGUIRichElementAnimationImageProxy : UGUIRichElementImageProxy
    {

        public override GameObject Create(RichElement element)
        {
            var imageGO = base.Create(element);
            if (imageGO != null)
            {
                var animationImage = element as UGUIRichElementAnimationImage;
                Debug.Assert(animationImage != null);

                UGUIUtil.CreateAnimationImage(imageGO, animationImage.GetImage(), animationImage.GetImageCount(), animationImage.GetAnimFPS());
            }

            return imageGO;
        }

        public override void Destroy(RichElement element, GameObject gameObject)
        {
            if (gameObject)
            {
                UGUIUtil.DestroyAnimationImage(gameObject);
                base.Destroy(element, gameObject);
            }
        }

    }


}