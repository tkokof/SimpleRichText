// desc simple create text underscore implementation
// maintainer hugoyu

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class UGUIUtil
{

    /*
    static Text GetTextUnderline(Text text)
    {
        if (text)
        {
            var textUnderlineGO = text.transform.Find("Underline");
            return textUnderlineGO != null ? textUnderlineGO.GetComponent<Text>() : null;
        }

        return null;
    }

    public static void AdjustTextUnderline(Text text)
    {
        var textUnderline = GetTextUnderline(text);
        if (textUnderline)
        {
            textUnderline.text = "_";
            float perlineWidth = textUnderline.preferredWidth;
            float width = text.preferredWidth;
            int lineCount = Mathf.CeilToInt(width / perlineWidth);

            for (int i = 1; i < lineCount; ++i)
            {
                textUnderline.text += "_";
            }
        }
    }

    public static GameObject CreateTextUnderline(Text text)
    {
        if (text)
        {
            // create underline game object
            // TODO use object pool ?
            var underlineGO = GameObject.Instantiate<GameObject>(text.gameObject);
            underlineGO.name = "Underline";
            Text underline = underlineGO.GetComponent<Text>();

            underlineGO.transform.SetParent(text.transform, false);

            RectTransform rt = underline.rectTransform;

            // set properties
            rt.anchoredPosition3D = Vector3.zero;
            rt.offsetMax = Vector2.zero;
            rt.offsetMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.anchorMin = Vector2.zero;

            AdjustTextUnderline(text);

            return underlineGO;
        }

        return null;
    }
	
    public static void DestroyTextUnderline(Text text)
    {
        var textUnderline = GetTextUnderline(text);
        if (textUnderline)
        {
            // TODO use game object pool ?
            textUnderline.transform.SetParent(null, false);
            GameObject.Destroy(textUnderline.gameObject);
        }
    }
    */

    public static void CreateTextUnderline(Text text)
    {
        // now we just add underline component
        if (text)
        {
            text.gameObject.AddComponent<UGUITextUnderline>();
        }
    }

    public static void DestroyTextUnderline(Text text)
    {
        // now we just destroy underline component
        if (text)
        {
            var underlineComp = text.GetComponent<UGUITextUnderline>();
            if (underlineComp)
            {
                Object.DestroyImmediate(underlineComp);
            }
        }
    }

}
