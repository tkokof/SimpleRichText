// desc rich text util
// maintainer hugoyu

using UnityEngine;

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

}
