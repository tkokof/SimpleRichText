// desc simple singleton implementation
// maintainer hugoyu

using System.Collections;
using UnityEngine;

namespace RichText
{

    public class SimpleSingleton<T> : MonoBehaviour where T : SimpleSingleton<T>
    {

        static T s_instance;

        static bool Check()
        {
            var gameObject = GameObject.FindObjectOfType(typeof(T));
            return gameObject == null;
        }

        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    Debug.Assert(Check());
                    var t = typeof(T);
                    var newGameObject = new GameObject(t.Name);
                    UnityEngine.Object.DontDestroyOnLoad(newGameObject);
                    s_instance = newGameObject.AddComponent<T>();
                    s_instance.Init();
                }

                return s_instance;
            }
        }

        public virtual void Init()
        {
        }

    }

}
