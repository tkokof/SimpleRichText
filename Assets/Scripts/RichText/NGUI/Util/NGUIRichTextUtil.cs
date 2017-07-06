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

    public static void CreateAnimationImage(GameObject imageGO, string imagePrefix, uint imageCount, float animFPS)
    {
        if (imageGO != null)
        {
            var imageInfo = imagePrefix.Split('.');
            if (imageInfo.Length < 2)
            {
                Debug.LogError("[NGUIRichTextUtil]Error to get animation image prefix : " + imagePrefix);
            }
            else
            {
                var animationComp = imageGO.AddComponent<UISpriteAnimation>();
                animationComp.namePrefix = imageInfo[1];
                animationComp.loop = true;
                if (animFPS <= 0)
                {
                    animFPS = 30;
                }
                animationComp.framesPerSecond = (int)animFPS;
            }
        }
    }

    public static void DestroyAnimationImage(GameObject imageGO)
    {
        if (imageGO != null)
        {
            var animationComp = imageGO.GetComponent<UISpriteAnimation>();
            if (animationComp != null)
            {
                Object.DestroyImmediate(animationComp);
            }
        }
    }

    static List<UIWidget> s_widgetsBuffer = new List<UIWidget>();

}

#endif