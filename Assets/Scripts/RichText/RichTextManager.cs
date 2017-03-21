// desc rich text manager
// maintainer hugoyu

using System;
using System.Collections;
using UnityEngine;

namespace RichText
{

    public static class RichTextManager
    {

        public enum Mode
        {
            UGUI,
            NGUI,
            Custom,
            None,
        }

        public static void Init(Mode mode, 
                                Action<RichElementProxyManager> customInit = null, 
                                IGameObjectManager gameObjectManager = null, 
                                RichSyntaxParser richSyntaxParser = null)
        {
            // should we release here ?
            Release();

            switch (mode)
            {
                case Mode.UGUI:
                    InitUGUI();
                    break;
                case Mode.NGUI:
                    InitNGUI();
                    break;
                case Mode.Custom:
                    InitCustom(customInit, gameObjectManager, richSyntaxParser);
                    break;
            }
        }

        public static void Release()
        {
            s_proxyManager.ClearProxies();
            s_mode = Mode.None;
        }

        public static Mode GetMode()
        {
            return s_mode;
        }

        public static RichElementProxy GetOrCreateProxy(RichElement element)
        {
            return s_proxyManager.GetOrCreateProxy(element);
        }

        public static void SetGameObjectManager(IGameObjectManager gameObjectManager)
        {
            s_gameObjectManager = gameObjectManager;
        }

        public static IGameObjectManager GetGameObjectManager()
        {
            return s_gameObjectManager;
        }

        public static void SetRichSyntaxParser(RichSyntaxParser richSyntaxParser)
        {
            s_richSyntaxParser = richSyntaxParser;
        }

        public static RichSyntaxParser GetRichSyntaxParser()
        {
            return s_richSyntaxParser;
        }

        public static bool ParseRichSyntax(string richSyntax, RichText richText)
        {
            if (s_richSyntaxParser != null)
            {
                return s_richSyntaxParser.Parse(richSyntax, richText);
            }

            return false;
        }

        static void InitUGUI()
        {
            s_mode = Mode.UGUI;
            s_proxyManager.ClearProxies();
            s_gameObjectManager = UGUIGameObjectManager.Instance;
            s_richSyntaxParser = new UGUIRichSyntaxParser();
        }

        static void InitNGUI()
        {
#if RICH_TEXT_ENABLE_NGUI
            s_mode = Mode.NGUI;
            s_proxyManager.ClearProxies();
            s_gameObjectManager = NGUIGameObjectManager.Instance;
            s_richSyntaxParser = new NGUIRichSyntaxParser();
#else
            Debug.LogError("[RichTextManager]Can not init NGUI mode since it is disabled ...");
#endif
        }

        static void InitCustom(Action<RichElementProxyManager> customInit, IGameObjectManager gameObjectManager, RichSyntaxParser richSyntaxParser)
        {
            s_mode = Mode.Custom;
            if (customInit != null)
            {
                customInit(s_proxyManager);
            }
            s_gameObjectManager = gameObjectManager;
            s_richSyntaxParser = richSyntaxParser;
        }

        static Mode s_mode = Mode.None;
        static RichElementProxyManager s_proxyManager = new RichElementProxyManager();
        static IGameObjectManager s_gameObjectManager = null;
        static RichSyntaxParser s_richSyntaxParser = null;
    }

}
