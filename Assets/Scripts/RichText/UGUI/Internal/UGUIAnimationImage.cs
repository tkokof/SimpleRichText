// desc ugui animation image implementation
// maintainer hugoyu

using UnityEngine;
using UnityEngine.UI;

namespace RichText
{

    [RequireComponent(typeof(Image))]
    public class UGUIAnimationImage : MonoBehaviour
    {

        protected Image m_image;
        protected string m_imagePrefix = "";
        protected uint m_imageCount = 1;
        protected float m_animInterval;

        protected uint m_curImageIndex;
        protected float m_elapsedTime;

        public string ImagePrefix
        {
            get
            {
                return m_imagePrefix;
            }

            set
            {
                if (m_imagePrefix != value)
                {
                    if (value == null)
                    {
                        Debug.LogWarning("[UGUIAnimationImage]Can not set image prefix to null");
                        value = "";
                    }
                    m_imagePrefix = value;
                    
                    Reset();
                }
            }
        }

        public uint ImageCount
        {
            get
            {
                return m_imageCount;
            }

            set
            {
                if (m_imageCount != value)
                {
                    if (value == 0)
                    {
                        Debug.LogWarning("[UGUIAnimationImage]Can not set image count to 0");
                        value = 1;
                    }
                    m_imageCount = value;

                    Reset();
                }
            }
        }

        public float AnimInterval
        {
            get
            {
                return m_animInterval;
            }

            set
            {
                if (m_animInterval != value)
                {
                    m_animInterval = value;

                    Reset();
                }
            }
        }

        protected void Awake()
        {
            m_image = gameObject.GetComponent<Image>();
        }

        protected void Update()
        {
            m_elapsedTime += Time.deltaTime;
            if (m_elapsedTime >= m_animInterval)
            {
                m_curImageIndex = ++m_curImageIndex % m_imageCount;
                m_elapsedTime = 0;

                if (m_image)
                {
                    m_image.sprite = GetCurSprite();
                }
            }
        }

        protected void Reset()
        {
            m_curImageIndex = 0;
            m_elapsedTime = 0;

            if (m_image)
            {
                m_image.sprite = GetCurSprite();
            }
        }

        protected Sprite GetCurSprite()
        {
            Debug.Assert(m_imagePrefix != null);
            var spriteName = m_imagePrefix + m_curImageIndex.ToString();
            return UGUIGameObjectManager.Instance.GetImageSprite(spriteName);
        }

    }

}