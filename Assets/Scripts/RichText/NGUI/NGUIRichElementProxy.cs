// desc ngui rich element proxy
// maintainer hugoyu

#if RICH_TEXT_ENABLE_NGUI

using System;
using UnityEngine;

namespace RichText
{

    public class NGUIRichElementTextProxy : RichElementTextProxy
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
            var textElement = element as NGUIRichElementText;
            Debug.Assert(textElement != null);

            var gameObjectManager = RichTextManager.GetGameObjectManager();
            if (gameObjectManager != null)
            {
                gameObjectManager.DestroyText(textElement.GetStyle(), gameObject);
            }
        }

        public override Vector2 GetSize(GameObject gameObject)
        {
            var textComp = gameObject.GetComponent<UILabel>();
            if (textComp != null)
            {
                return new Vector2(textComp.width, textComp.height);
            }

            Debug.LogWarning("[NGUIRichElementTextProxy]Error to get size of NGUI UILabel component");
            return Vector2.zero;
        }

        public override void SetText(GameObject gameObject, string text)
        {
            var textComp = gameObject.GetComponent<UILabel>();
            if (textComp != null)
            {
                textComp.text = text;
            }
        }

    }

    public class NGUIRichElementImageProxy : RichElementImageProxy
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
            var imageComp = gameObject.GetComponent<UISprite>();
            if (imageComp != null)
            {
                return new Vector2(imageComp.width, imageComp.height);
            }

            Debug.LogWarning("[NGUIRichElementImageProxy]Error to get size of NGUI UISprite component");
            return Vector2.zero;
        }

    }

    public class NGUIRichElementCustomProxy : RichElementCustomProxy
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
            if (gameObject)
            {
                var bounds = NGUIMath.CalculateRelativeWidgetBounds(gameObject.transform);
                return bounds.size;
            }
            else
            {
                return Vector2.zero;
            }
        }

    }

}

#endif