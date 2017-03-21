// desc ngui rich text game object manager, simple object pool implementation
// maintainer hugoyu

#if RICH_TEXT_ENABLE_NGUI

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RichText
{

    public class NGUIGameObjectManager : SimpleSingleton<NGUIGameObjectManager>, IGameObjectManager
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
                var textComp = textGO.GetComponent<UILabel>();
                if (!textComp)
                {
                    Debug.LogError("[NGUIGameObjectManager]Error to get UILabel component : " + style);
                    textPool.Release(textGO);
                    return null;
                }

                textComp.text = text;

                // add collider to widget first
                NGUITools.AddWidgetCollider(textGO);

                // handler click handler
                var nguiClickHandler = RichTextUtil.GetOrAddComponent<NGUIClickHandler>(textGO);
                Debug.Assert(nguiClickHandler != null);
                // first clear
                nguiClickHandler.ClearHandlers();
                // then add handler
                if (clickHandler != null)
                {
                    nguiClickHandler.AddHandler(clickHandler);
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

        UIAtlas GetImageAtlas(string atlasName)
        {
            var atlasGO = Resources.Load<GameObject>(RESOURCE_PATH_IMAGE_ATLAS + "/" + atlasName);
            if (atlasGO)
            {
                return atlasGO.GetComponent<UIAtlas>();
            }

            return null;
        }

        public GameObject CreateImage(string imageName, Action clickHandler)
        {
            var imagePrefab = GetImagePrefab();
            if (imagePrefab)
            {
                var imageGO = m_imagePool.Spawn(imagePrefab);
                var imageComp = imageGO.GetComponent<UISprite>();
                if (!imageComp)
                {
                    Debug.LogError("[NGUIGameObjectManager]Error to get UISprite component : " + imageName);
                    m_imagePool.Release(imageGO);
                    return null;
                }

                // get atlas name and sprite name
                var imageInfo = imageName.Split('.');
                if (imageInfo.Length < 2)
                {
                    Debug.LogError("[NGUIGameObjectManager]Error to get atlas name and sprite name : " + imageName);
                    m_imagePool.Release(imageGO);
                    return null;
                }

                var atlasName = imageInfo[0];
                var spriteName = imageInfo[1];
                imageComp.atlas = GetImageAtlas(atlasName);
                imageComp.spriteName = spriteName;
                // NOTE not so sure about this ...
                imageComp.MakePixelPerfect();

                // add collider to widget first
                NGUITools.AddWidgetCollider(imageGO);

                // handler click handler
                var nguiClickHandler = RichTextUtil.GetOrAddComponent<NGUIClickHandler>(imageGO);
                Debug.Assert(nguiClickHandler != null);
                // first clear
                nguiClickHandler.ClearHandlers();
                // then add handler
                if (clickHandler != null)
                {
                    nguiClickHandler.AddHandler(clickHandler);
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

        void OnSpawn(GameObject nguiGO)
        {
            if (nguiGO)
            {
                nguiGO.transform.SetParent(null, false);
                nguiGO.SetActive(true);
            }
        }

        void OnRelease(GameObject nguiGO)
        {
            if (nguiGO)
            {
                nguiGO.SetActive(false);
                nguiGO.transform.SetParent(transform, false);
            }
        }

        void OnSpawnText(GameObject nguiTextGO)
        {
            OnSpawn(nguiTextGO);
        }

        void OnReleaseText(GameObject nguiTextGO)
        {
            OnRelease(nguiTextGO);
        }

        void OnSpawnImage(GameObject nguiImageGO)
        {
            OnSpawn(nguiImageGO);
        }

        void OnReleaseImage(GameObject nguiImageGO)
        {
            OnRelease(nguiImageGO);
        }

        void OnSpawnCustom(GameObject nguiCustomGO)
        {
            OnSpawn(nguiCustomGO);
        }

        void OnReleaseCustom(GameObject nguiCustomGO)
        {
            OnRelease(nguiCustomGO);
        }

        const string RESOURCE_PATH = "RichText/NGUI";
        const string RESOURCE_PATH_TEXT = RESOURCE_PATH + "/Text";
        const string RESOURCE_PATH_IMAGE = RESOURCE_PATH + "/Image";
        const string RESOURCE_PATH_IMAGE_ATLAS = RESOURCE_PATH_IMAGE + "/Atlas";

        Dictionary<string, GameObject> m_styleFontMap = new Dictionary<string, GameObject>();
        Dictionary<string, ObjectPool> m_textPools = new Dictionary<string, ObjectPool>();

        GameObject m_imagePrefab;
        ObjectPool m_imagePool;

    }

}

#endif