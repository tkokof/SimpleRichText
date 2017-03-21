// desc rich element display proxy
// maintainer hugoyu

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RichText
{

    public abstract class RichElementProxy
    {

        public abstract GameObject Create(RichElement element);
        public abstract void Destroy(RichElement element, GameObject gameObject);
        public abstract Vector2 GetSize(GameObject gameObject);

    }

    public abstract class RichElementTextProxy : RichElementProxy
    {

        public abstract GameObject Create(string text, string style, Action clickHandler);
        public abstract void SetText(GameObject gameObject, string text);

    }

    public abstract class RichElementImageProxy : RichElementProxy
    {
    }

    public abstract class RichElementCustomProxy : RichElementProxy
    {
    }

    public class RichElementNewlineProxy : RichElementProxy
    {

        public override GameObject Create(RichElement element)
        {
            return null;
        }

        public override void Destroy(RichElement element, GameObject gameObject)
        {
        }

        public override Vector2 GetSize(GameObject gameObject)
        {
            return Vector2.zero;
        }

    }

}