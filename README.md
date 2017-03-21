# SimpleRichText

## Desc
SimpleRichText is a simple rich text plugin for Unity3D, based on UGUI or NGUI (you can implement custom rich text mode).
Both UGUI and NGUI support basic rich text formats, but they are limited, this plugin is aim to enhance their rich 
text ability (still you can get rid of UGUI or NGUI by implementing your custom rich text mode)

## Usage
Usage is simple, below are sample codes:

1. first you need to init rich text

  RichTextManager.Init(RichTextManager.Mode.UGUI);
  
2. then you could add rich elements to rich text by RichText.AddRichElement method,
   and then call RichText.Format
   
3. for simple useage, we could create a rich text syntax string to build rich text by
   using RichTextManager.ParseRichSyntax method
   
Please check example scenes under [ProjectRoot]/Scenes : test_rich_text_ugui and test_rich_text_ngui

## Notice
We do not support NGUI mode by default, you can enable it by adding RICH_TEXT_ENABLE_NGUI macro symbol.
