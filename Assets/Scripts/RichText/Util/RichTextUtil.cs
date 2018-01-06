// desc rich text util
// maintainer hugoyu

using UnityEngine;

namespace RichText
{

    public static class RichTextUtil
    {

        public static void OpenURL(string url)
        {
            Application.OpenURL(url);
        }

        public static T GetOrAddComponent<T>(GameObject go) where T : Component
        {
            if (go != null)
            {
                var comp = go.GetComponent<T>();
                if (comp == null)
                {
                    comp = go.AddComponent<T>();
                }

                return comp;
            }

            return null;
        }

        public static void DestroyComponent<T>(GameObject go) where T : Component
        {
            if (go != null)
            {
                var comp = go.GetComponent<T>();
                if (comp != null)
                {
                    Object.Destroy(comp);
                }
            }
        }

        public static void SetRichText(RichText richText, string richSyntax)
        {
            if (richText != null)
            {
                var ret = RichTextManager.ParseRichSyntax(richSyntax, richText);
                if (ret)
                {
                    richText.Format();
                }
                else
                {
                    richText.ClearRichElements();
                }
            }
        }

        public static void SetSyntaxText(this RichText richText, string richSyntax)
        {
            SetRichText(richText, richSyntax);
        }

    }

}