// desc ugui rich element proxy
// maintainer hugoyu

using System;
using UnityEngine;
using UnityEngine.UI;

namespace RichText
{

    public class UGUIRichElementTextProxy : RichElementTextProxy
    {

        public override GameObject Create(RichElement element)
        {
            var textElement = element as RichElementText;
            Debug.Assert(textElement != null);
            return Create(textElement.GetText(), textElement.GetStyle(), textElement.GetClickHandler());
        }

        public override GameObject Create(string text, string style, Action clickHandler)
        {
            var gameObjectManager = RichTextManager.GetGameObjectManager();
            if (gameObjectManager != null)
            {
                return gameObjectManager.CreateText(text, style, clickHandler);
            }

            return null;
        }

        public override void Destroy(RichElement element, GameObject gameObject)
        {
            var textElement = element as UGUIRichElementText;
            Debug.Assert(textElement != null);

            var gameObjectManager = RichTextManager.GetGameObjectManager();
            if (gameObjectManager != null)
            {
                gameObjectManager.DestroyText(textElement.GetStyle(), gameObject);
            }
        }

        public override Vector2 GetSize(GameObject gameObject)
        {
            var textComp = gameObject.GetComponent<Text>();
            if (textComp != null)
            {
                return new Vector2(textComp.preferredWidth, textComp.preferredHeight);
            }

            Debug.LogWarning("[UGUIRichElementTextProxy]Error to get size of UGUI Text component");
            return Vector2.zero;
        }

        public override void SetText(GameObject gameObject, string text)
        {
            var textComp = gameObject.GetComponent<Text>();
            if (textComp != null)
            {
                textComp.text = text;

                // NOTE we need to do force update layout here
                LayoutRebuilder.ForceRebuildLayoutImmediate(textComp.rectTransform);
            }
        }

    }

    public class UGUIRichElementImageProxy : RichElementImageProxy
    {

        public override GameObject Create(RichElement element)
        {
            var imageElement = element as RichElementImage;
            Debug.Assert(imageElement != null);

            var gameObjectManager = RichTextManager.GetGameObjectManager();
            if (gameObjectManager != null)
            {
                return gameObjectManager.CreateImage(imageElement.GetImage(), imageElement.GetClickHandler());
            }

            return null;
        }

        public override void Destroy(RichElement element, GameObject gameObject)
        {
            var gameObjectManager = RichTextManager.GetGameObjectManager();
            if (gameObjectManager != null)
            {
                gameObjectManager.DestroyImage(gameObject);
            }
        }

        public override Vector2 GetSize(GameObject gameObject)
        {
            var imageComp = gameObject.GetComponent<Image>();
            if (imageComp != null)
            {
                // NOTE force update layout here
                //      not so sure about this ...
                LayoutRebuilder.ForceRebuildLayoutImmediate(imageComp.rectTransform);

                return new Vector2(imageComp.preferredWidth, imageComp.preferredHeight);
            }

            Debug.LogWarning("[UGUIRichElementImageProxy]Error to get size of UGUI Image component");
            return Vector2.zero;
        }

    }

    public class UGUIRichElementCustomProxy : RichElementCustomProxy
    {

        public override GameObject Create(RichElement element)
        {
            var richElementCustom = element as RichElementCustom;
            Debug.Assert(richElementCustom != null);

            var gameObjectManager = RichTextManager.GetGameObjectManager();
            if (gameObjectManager != null)
            {
                return gameObjectManager.CreateCustom(richElementCustom.GetGameObject());
            }

            return null;
        }

        public override void Destroy(RichElement element, GameObject gameObject)
        {
            var gameObjectManager = RichTextManager.GetGameObjectManager();
            if (gameObjectManager != null)
            {
                gameObjectManager.DestroyCustom(gameObject);
            }
        }

        public override Vector2 GetSize(GameObject gameObject)
        {
            var rectTransforms = gameObject.GetComponentsInChildren<RectTransform>();
            if (rectTransforms != null && rectTransforms.Length > 0)
            {
                var baseRectTransform = rectTransforms[0];
                var baseRect = baseRectTransform.rect;
                
                for (int i = 1; i < rectTransforms.Length; ++i)
                {
                    var rect = rectTransforms[i].rect;

                    if (rect.xMin < baseRect.xMin)
                    {
                        baseRect.xMin = rect.xMin;
                    }

                    if (rect.xMax > baseRect.xMax)
                    {
                        baseRect.xMax = rect.xMax;
                    }

                    if (rect.yMin < baseRect.yMin)
                    {
                        baseRect.yMin = rect.yMin;
                    }

                    if (rect.yMax > baseRect.yMax)
                    {
                        baseRect.yMax = rect.yMax;
                    }
                }

                return baseRect.size;
            }
            else
            {
                return Vector2.zero;
            }
        }

    }

}


