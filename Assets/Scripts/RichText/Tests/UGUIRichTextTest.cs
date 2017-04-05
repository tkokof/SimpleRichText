// desc tests for ugui rich text
// maintainer hugoyu

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RichText;

public class UGUIRichTextTest : UIBehaviour
{

    [SerializeField]
    string m_richSyntax = "用以测试的<u><url|value=www.baidu.com>RichText</u><br>This is RichText<image|name=sprite_2></url><break><style|value=default>Text<style|value=default><image|name=sprite_3><image|name=sprite_3><image|name=sprite_3>文本";
    [SerializeField]
    RectTransform m_rectTransform = null;
    [SerializeField]
    RichText.RichText m_richText = null;

    protected override void Awake()
    {
        base.Awake();

        RichText.RichTextManager.Init(RichTextManager.Mode.UGUI);

        if (m_rectTransform)
        {
            if (m_richText)
            {
                var size = m_rectTransform.rect.size;
                m_richText.SetTextWidth(size.x);
                m_richText.SetVerticalSpace(8);
            }
        }
    }

    void Generate()
    {
        if (m_richText)
        {
            m_richText.SetSyntaxText(m_richSyntax);
        }
    }

    void OnRectTransformDimensionsChangeInternal()
    {
        if (m_rectTransform)
        {
            if (m_richText)
            {
                var size = m_rectTransform.rect.size;
                m_richText.SetTextWidth(size.x);
                m_richText.Format();
            }
        }
    }

    protected override void OnRectTransformDimensionsChange()
    {
        Invoke("OnRectTransformDimensionsChangeInternal", 0);
    }

    void OnGUI()
    {
        if (GUILayout.Button("Generate"))
        {
            Generate();
        }
    }

}
