// desc tests for ngui rich text
// maintainer hugoyu

using UnityEngine;
using RichText;

public class NGUIRichTextTest : MonoBehaviour
{

#if RICH_TEXT_ENABLE_NGUI

    [SerializeField]
    string m_richSyntax = "this is a <u><url|value=www.google.com><style|value=yellow_24>Test RichText </u><image|name=Test1.smile|size=64,64></url><image|name=Test1.heart|size=64,64></url><style|value=default>~<br>";
    [SerializeField]
    UIWidget m_widget = null;
    [SerializeField]
    RichText.RichText m_richText = null;

    void Awake()
    {
        RichTextManager.Init(RichTextManager.Mode.NGUI);

        if (m_richText)
        {
            if (m_widget)
            {
                m_richText.SetTextWidth(m_widget.width);
                m_richText.SetVerticalSpace(8);
            }
            else
            {
                m_richText.SetTextWidth(512);
                m_richText.SetVerticalSpace(8);
            }

            Generate();
        }
    }

    void OnEnable()
    {
        if (m_widget)
        {
            m_widget.onChange += OnDimensionChange;
        }
    }

    void OnDisable()
    {
        if (m_widget)
        {
            m_widget.onChange -= OnDimensionChange;
        }
    }

    void Generate()
    {
        if (m_richText)
        {
            m_richText.SetSyntaxText(m_richSyntax);
            if (m_widget)
            {
                NGUIRichTextUtil.AdjustDepth(m_richText.gameObject, m_widget.depth);
            }
        }
    }

    void OnDimensionChangeInternal()
    {
        if (m_richText)
        {
            if (m_widget)
            {
                if (m_richText.GetTextWidth() != m_widget.width)
                {
                    m_richText.SetTextWidth(m_widget.width);
                    m_richText.Format();
                    if (m_widget)
                    {
                        NGUIRichTextUtil.AdjustDepth(m_richText.gameObject, m_widget.depth);
                    }
                }
            }
        }
    }

    void OnDimensionChange()
    {
        // NOTE since UIWidget's onChange is called in UIPanel's Update loop, 
        //      we can not adjust widget's hierachy directly which will be done by RichText.Format(),
        //      so we delay invoke handler here
        //      not so sure about this ...
        Invoke("OnDimensionChangeInternal", 0);
    }

    void OnGUI()
    {
        if (GUILayout.Button("Generate"))
        {
            Generate();
        }
    }

#endif

}