// desc ngui rich text util
// maintainer hugoyu
// TODO improve

#if RICH_TEXT_ENABLE_NGUI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NGUIRichTextUtil
{

    public static void AdjustDepth(GameObject richTextGO, int depth)
    {
        if (richTextGO != null)
        {
            s_widgetsBuffer.Clear();
            richTextGO.GetComponentsInChildren<UIWidget>(true, s_widgetsBuffer);
            for (int i = 0; i < s_widgetsBuffer.Count; ++i)
            {
                s_widgetsBuffer[i].depth = depth;
            }
            s_widgetsBuffer.Clear();
        }
    }

    static List<UIWidget> s_widgetsBuffer = new List<UIWidget>();

}

#endif