// desc rich text util
// maintainer hugoyu

using System.Collections.Generic;
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

        public static void DestroyAllChildren(GameObject root)
        {
            if (root)
            {
                int childCount = root.transform.childCount;
                for (int i = 0; i < childCount; ++i)
                {
                    var child = root.transform.GetChild(i);
                    if (child)
                    {
                        UnityEngine.Object.Destroy(child.gameObject);
                    }
                }
            }
        }

        public static void DestroyAllChildrenImmediate(GameObject root)
        {
            if (root)
            {
                int childCount = root.transform.childCount;
                if (childCount > 0)
                {
                    var childrenBuffer = new List<Transform>(childCount);

                    for (int i = 0; i < childCount; ++i)
                    {
                        var child = root.transform.GetChild(i);
                        if (child)
                        {
                            childrenBuffer.Add(child);
                        }
                    }

                    for (int i = 0; i < childrenBuffer.Count; ++i)
                    {
                        UnityEngine.Object.DestroyImmediate(childrenBuffer[i].gameObject);
                    }
                }
            }
        }

        public static void ResetTransform(Transform transform)
        {
            if (transform)
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                transform.localScale = Vector3.one;
            }
        }

        public static void ResetTransform(GameObject go)
        {
            if (go)
            {
                var transform = go.transform;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                transform.localScale = Vector3.one;
            }
        }

        public static GameObject CreateGameObject(string name, GameObject parent = null)
        {
            var gameObject = new GameObject(name);
            if (parent)
            {
                gameObject.transform.SetParent(parent.transform, false);
            }

            ResetTransform(gameObject);

            return gameObject;
        }

        public static void ReleaseGameObject(GameObject go)
        {
            if (go)
            {
                UnityEngine.Object.Destroy(go);
            }
        }

        public static void ReleaseGameObject(Transform go)
        {
            if (go)
            {
                UnityEngine.Object.Destroy(go);
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