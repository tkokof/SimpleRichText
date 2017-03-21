// desc simple rich text parser test
// maintainer hugoyu

using System.Collections;
using UnityEngine;
using RichText;

public class RichSyntaxParserTest : RichSyntaxParser
{

    protected override void OnParseBegin()
    {
        Debug.Log("Parse Begin");
    }

    protected override void OnSyntax(RichSyntaxData syntaxData)
    {
        Debug.Log(syntaxData);
    }

    protected override void OnText(string text)
    {
        Debug.Log(text);
    }

    protected override void OnParseEnd()
    {
        Debug.Log("Parse End");
    }

}
