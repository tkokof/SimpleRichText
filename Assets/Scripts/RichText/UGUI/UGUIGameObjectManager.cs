// desc ugui rich text game object manager, simple object pool implementation
// maintainer hugoyu

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RichText
{

    public class UGUIGameObjectManager : SimpleSingleton<UGUIGameObjectManager>, IGameObjectManager
    {

        public override void Init()
        {
            m_imagePool = new ObjectPool(OnSpawnImage, OnReleaseImage);
        }

        GameObject GetTextPrefab(string style)
        {
            if (!m_styleFontMap.ContainsKey(style))
            {
                var prefab = Resources.Load<GameObject>(RESOURCE_PATH_TEXT + "/" + style);
                if (prefab)
                {
                    m_styleFontMap[style] = prefab;
                }
            }

            return m_styleFontMap.ContainsKey(style) ? m_styleFontMap[style] : null;
        }

        ObjectPool GetTextPool(string style)
        {
            if (m_textPools.ContainsKey(style))
            {
                return m_textPools[style];
            }
            else
            {
                var textPool = new ObjectPool(OnSpawnText, OnReleaseText);
                m_textPools.Add(style, textPool);

                return textPool;
            }
        }

        public GameObject CreateText(string text, string style, Action clickHandler)
        {
            var textGOPrefab = GetTextPrefab(style);
            if (textGOPrefab)
            {
                var textPool = GetTextPool(style);
                var textGO = textPool.Spawn(textGOPrefab);
                var textComp = textGO.GetComponent<Text>();
                if (!textComp)
                {
                    Debug.LogError("[UGUIGameObjectManager]Error to get Text component : " + style);
                    textPool.Release(textGO);
                    return null;
                }
                
                textComp.text = text;

                // handler click handler
                var uguiClickHandler = RichTextUtil.GetOrAddComponent<UGUIClickHandler>(textGO);
                Debug.Assert(uguiClickHandler != null);
                // first clear
                uguiClickHandler.ClearHandlers();
                // then add handler
                if (clickHandler != null)
                {
                    uguiClickHandler.AddHandler(clickHandler);
                }

                return textGO;
            }

            return null;
        }

        public void DestroyText(string style, GameObject textGO)
        {
            if (textGO)
            {
                var textPool = GetTextPool(style);
                textPool.Release(textGO);
            }
        }

        GameObject GetImagePrefab()
        {
            if (!m_imagePrefab)
            {
                m_imagePrefab = Resources.Load<GameObject>(RESOURCE_PATH_IMAGE + "/default");
            }

            return m_imagePrefab;
        }

        Sprite GetImageSprite(string spriteName)
        {
            if (!m_imageSpriteMap.ContainsKey(spriteName))
            {
                spriteName = spriteName.Replace('.', '/');
                var imageSprite = Resources.Load<Sprite>(RESOURCE_PATH_IMAGE_SPRITE + "/" + spriteName);
                if (imageSprite)
                {
                    m_imageSpriteMap[spriteName] = imageSprite;
                }
            }

            return m_imageSpriteMap.ContainsKey(spriteName) ? m_imageSpriteMap[spriteName] : null;
        }

        public GameObject CreateImage(string imageName, Action clickHandler)
        {
            var imagePrefab = GetImagePrefab();
            var imageSprite = GetImageSprite(imageName);
            if (imagePrefab && imageSprite)
            {
                var imageGO = m_imagePool.Spawn(imagePrefab);
                var imageComp = imageGO.GetComponent<Image>();
                if (!imageComp)
                {
                    Debug.LogError("[UGUIGameObjectManager]Error to get Image component : " + imageName);
                    m_imagePool.Release(imageGO);
                    return null;
                }

                imageComp.sprite = imageSprite;

                // NOTE not so sure about this ...
                imageComp.type = Image.Type.Simple;
                imageComp.preserveAspect = true;
                imageComp.SetNativeSize();

                // handler click handler
                var uguiClickHandler = RichTextUtil.GetOrAddComponent<UGUIClickHandler>(imageGO);
                Debug.Assert(uguiClickHandler != null);
                // first clear
                uguiClickHandler.ClearHandlers();
                // then add handler
                if (clickHandler != null)
                {
                    uguiClickHandler.AddHandler(clickHandler);
                }

                return imageGO;
            }

            return null;
        }

        public void DestroyImage(GameObject imageGO)
        {
            if (imageGO)
            {
                m_imagePool.Release(imageGO);
            }
        }

        public GameObject CreateCustom(GameObject customPrototypeGO)
        {
            // now we just use Instantiate
            var customGO = UnityEngine.Object.Instantiate<GameObject>(customPrototypeGO);
            if (customGO)
            {
                OnSpawnCustom(customGO);
            }

            return customGO;
        }

        public void DestroyCustom(GameObject customGO)
        {
            if (customGO)
            {
                OnReleaseCustom(customGO);
                // now we just use Destroy
                UnityEngine.Object.Destroy(customGO);
            }
        }

        void OnSpawn(GameObject uguiGO)
        {
            if (uguiGO)
            {
                uguiGO.transform.SetParent(null, false);
                uguiGO.SetActive(true);
            }
        }

        void OnRelease(GameObject uguiGO)
        {
            if (uguiGO)
            {
                uguiGO.SetActive(false);
                uguiGO.transform.SetParent(transform, false);
            }
        }

        void OnSpawnText(GameObject uguiTextGO)
        {
            OnSpawn(uguiTextGO);
        }

        void OnReleaseText(GameObject uguiTextGO)
        {
            OnRelease(uguiTextGO);
        }

        void OnSpawnImage(GameObject uguiImageGO)
        {
            OnSpawn(uguiImageGO);
        }

        void OnReleaseImage(GameObject uguiImageGO)
        {
            OnRelease(uguiImageGO);
        }

        void OnSpawnCustom(GameObject uguiCustomGO)
        {
            OnSpawn(uguiCustomGO);
        }

        void OnReleaseCustom(GameObject uguiCustomGO)
        {
            OnRelease(uguiCustomGO);
        }

        const string RESOURCE_PATH = "RichText/UGUI";
        const string RESOURCE_PATH_TEXT = RESOURCE_PATH + "/Text";
        const string RESOURCE_PATH_IMAGE = RESOURCE_PATH + "/Image";
        const string RESOURCE_PATH_IMAGE_SPRITE = RESOURCE_PATH_IMAGE + "/Sprite";

        Dictionary<string, GameObject> m_styleFontMap = new Dictionary<string, GameObject>();
        Dictionary<string, ObjectPool> m_textPools = new Dictionary<string,ObjectPool>();

        GameObject m_imagePrefab;
        Dictionary<string, Sprite> m_imageSpriteMap = new Dictionary<string, Sprite>();
        ObjectPool m_imagePool;

    }

}