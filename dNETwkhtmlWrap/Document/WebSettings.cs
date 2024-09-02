using System;
using System.ComponentModel;

namespace dNetWkhtmlWrap
{
  /// <summary>
  /// Settings for WebSite access and loading specifics
  /// </summary>
  [Serializable]
  [TypeConverter( typeof( ExpandableObjectConverter ) )]
  public class WebSettings : ISettings
  {
    /// <summary>
    /// Set the default text encoding, for input E.g. "utf-8"
    /// --encoding 'encoding'
    /// </summary>
    public string DefaultEncoding { get; set; }

    /// <summary>
    /// Enable the intelligent shrinking strategy used by WebKit that makes the pixel/dpi ratio non-constant (default)
    /// --enable-smart-shrinking
    /// 
    /// False:
    /// Disable the intelligent shrinking strategy used by WebKit that makes the pixel/dpi ratio non-constant
    /// --disable-smart-shrinking
    /// </summary>
    public bool? EnableIntelligentShrinking { get; set; }

    /// <summary>
    /// Do allow web pages to run javascript (default)
    /// --enable-javascript
    /// 
    /// False:
    /// Do not allow web pages to run javascript
    /// --disable-javascript
    /// </summary>
    public bool? EnableJavascript { get; set; }

    /// <summary>
    /// Disable installed plugins (default)
    /// --disable-plugins
    /// 
    /// True:
    /// Enable installed plugins (plugins will likely not work)
    /// --enable-plugins
    /// </summary>
    public bool? EnablePlugins { get; set; }

    /// <summary>
    /// Do load or print images (default)
    /// --images
    /// 
    /// False:
    /// Do not load or print images
    /// --no-images
    /// </summary>
    public bool? LoadImages { get; set; }

    /// <summary>
    /// Minimum font size
    /// --minimum-font-size 'int'
    /// </summary>
    public int? MinimumFontSize { get; set; }

    /// <summary>
    /// Do print background (default)
    /// --background
    /// 
    /// False:
    /// Do not print background
    /// --no-background
    /// </summary>
    public bool? PrintBackground { get; set; }

    /// <summary>
    /// Do not use print media-type instead of screen (default)
    /// --no-print-media-type
    /// 
    /// True:
    /// Use print media-type instead of screen
    /// --print-media-type
    /// </summary>
    public bool? PrintMediaType { get; set; }

    /// <summary>
    /// Specify a user style sheet, to load with every page
    /// --user-style-sheet 'path'
    /// </summary>
    public string UserStyleSheet { get; set; }
  }
}