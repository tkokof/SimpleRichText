// desc IGameObjectManager interface
// maintainer hugoyu

using System;
using UnityEngine;

public interface IGameObjectManager
{

    GameObject CreateText(string text, string style, Action clickHandler);
    void DestroyText(string style, GameObject textGO);
    GameObject CreateImage(string imageName, Action clickHandler);
    void DestroyImage(GameObject imageGO);
    GameObject CreateCustom(GameObject customPrototypeGO);
    void DestroyCustom(GameObject customGO);

}