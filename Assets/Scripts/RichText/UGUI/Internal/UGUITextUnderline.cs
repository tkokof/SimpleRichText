// desc simple ugui text underline implementation
// maintainer hugoyu
// TODO improve

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RichText
{

    [RequireComponent(typeof(Text))]
    public class UGUITextUnderline : MonoBehaviour
    {

        Text m_textComp;
        RectTransform m_textRectTransform;
        TextGenerator m_textGenerator;

        GameObject m_underlineGameObject;
        Image m_underlineImageComp;
        RectTransform m_underlineRectTransform;

        void Start()
        {
            m_textComp = gameObject.GetComponent<Text>();
            m_textRectTransform = gameObject.GetComponent<RectTransform>();
            m_textGenerator = m_textComp.cachedTextGenerator;

            // TODO use ObjectPool ?
            m_underlineGameObject = new GameObject("Underline");
            m_underlineImageComp = m_underlineGameObject.AddComponent<Image>();
            m_underlineImageComp.color = m_textComp.color;

            m_underlineRectTransform = m_underlineGameObject.GetComponent<RectTransform>();
            m_underlineRectTransform.SetParent(transform, false);
            m_underlineRectTransform.anchorMin = m_textRectTransform.pivot;
            m_underlineRectTransform.anchorMax = m_textRectTransform.pivot;
        }

        void Update()
        {
            UICharInfo[] charactersInfo = m_textGenerator.GetCharactersArray();
            UILineInfo[] linesInfo = m_textGenerator.GetLinesArray();
            Canvas canvas = m_textComp.canvas;

            if (charactersInfo.Length > 0 && linesInfo.Length > 0 && canvas)
            {
                m_underlineGameObject.SetActive(true);

                UICharInfo firstCharInfo = charactersInfo[0];
                UICharInfo lastCharInfo = charactersInfo[charactersInfo.Length - 1];
                float height = linesInfo[0].height;
                // NOTE not so sure about this
                float factor = 1.0f / canvas.scaleFactor;

                m_underlineRectTransform.anchoredPosition =
                    new Vector2(
                        factor * (firstCharInfo.cursorPos.x + lastCharInfo.cursorPos.x) / 2.0f,
                        factor * (firstCharInfo.cursorPos.y - height)
                    );
                m_underlineRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, factor * Mathf.Abs(firstCharInfo.cursorPos.x - lastCharInfo.cursorPos.x));
                m_underlineRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, factor * height / 10.0f);
            }
            else
            {
                // just hide underline when text is empty
                m_underlineGameObject.SetActive(false);
            }
        }

        void OnDestroy()
        {
            if (m_underlineGameObject)
            {
                Debug.Assert(m_underlineRectTransform != null);
                m_underlineRectTransform.SetParent(null, false);

                Object.Destroy(m_underlineGameObject);
            }
        }

    }

}
