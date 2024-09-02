using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace dNetWkhtmlWrap
{
  /// <summary>
  /// Settings Object for Page Loading
  /// </summary>
  [Serializable]
  [TypeConverter( typeof( ExpandableObjectConverter ) )]
  public class LoadSettings : ISettings
  {
    /// <summary>
    /// cTor:
    /// </summary>
    public LoadSettings( )
    {
      this.Cookies = new Dictionary<string, string>( );
      this.CustomHeaders = new Dictionary<string, string>( );
      this.PostItems = new List<PostItem>( );
    }

    /// <summary>
    /// Error Handlers
    /// </summary>
    public enum ContentErrorHandling
    {
      /// <summary>
      /// Abort after error
      /// </summary>
      Abort,
      /// <summary>
      /// Skip
      /// </summary>
      Skip,
      /// <summary>
      /// Ignore errors
      /// </summary>
      Ignore
    }

    /// <summary>
    /// Do not allow conversion of a local file to read in other local files, 
    ///   unless explicitly allowed with --allow (default) 
    /// --disable-local-file-access
    /// 
    /// True:
    /// Allowed conversion of a local file to read in other local files.
    /// --enable-local-file-access
    /// </summary>
    public bool? BlockLocalFileAccess { get; set; }

    /// <summary>
    /// Set an additional cookie (repeatable), value should be url encoded.
    /// --cookie 'name' 'value'
    /// </summary>
    public Dictionary<string, string> Cookies { get; private set; }

    /// <summary>
    /// Set an additional HTTP header (repeatable)
    /// --custom-header 'name' 'value'
    /// </summary>
    public Dictionary<string, string> CustomHeaders { get; private set; }

    /// <summary>
    /// Do not show javascript debugging output (default)
    /// --no-debug-javascript
    /// 
    /// True:
    /// Show javascript debugging output
    /// --debug-javascript
    /// </summary>
    public bool? DebugJavascript { get; set; }

    /// <summary>
    /// Specify how to handle pages that fail to load: abort, ignore or skip(default abort)
    /// --load-error-handling 'handler'
    /// </summary>
    public ContentErrorHandling? ErrorHandling { get; set; }

    /// <summary>
    /// HTTP Authentication username
    /// --username 'username'
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// HTTP Authentication password (THIS IS UNSAFE - NOT RECOMMENDED TO USE !!!!)
    /// --password 'password'
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Add an additional post field (repeatable)
    /// --post 'name' 'value'
    /// 
    /// Post an additional file (repeatable)
    /// --post-file 'name' 'path'
    /// </summary>
    public IList<PostItem> PostItems { get; private set; }

    /// <summary>
    /// Use a proxy (default NOT)
    /// --proxy 'proxy'
    /// </summary>
    public string Proxy { get; set; }

    /// <summary>
    /// Do not add HTTP headers specified by --custom-header for each resource request.
    /// --no-custom-header-propagation
    /// 
    /// True:
    /// Add HTTP headers specified by --custom-header for each resource request.
    /// --custom-header-propagation
    /// </summary>
    public bool? RepeatCustomHeaders { get; set; }

    /// <summary>
    /// Stop slow running javascripts (default)
    /// --stop-slow-scripts
    /// 
    /// False:
    /// Do not Stop slow running javascripts
    /// --no-stop-slow-scripts
    /// </summary>
    public bool? StopSlowScript { get; set; }

    /// <summary>
    /// Use this zoom factor (default 1)
    /// --zoom 'float'
    /// </summary>
    public double? ZoomFactor { get; set; }


    /// <summary>
    /// Wait until window.status is equal to this string before rendering page
    /// --window-status 'windowStatus'
    /// ?? MAY NOT HAVE ANY EFFECT
    /// </summary>
    public string WindowStatus { get; set; }
    /// <summary>
    /// ?? MAY NOT HAVE ANY EFFECT
    /// </summary>
    public int? RenderDelay { get; set; }


  }
}