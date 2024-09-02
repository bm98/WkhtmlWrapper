using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dNetWkhtmlWrap
{
  // NOTE: Added to Package bm98

  /// <summary>
  /// Create a IMAGE Command Line
  /// </summary>
  internal class ImageRequest
  {
    #region Command line Extended HELP 

    /*
     * 
        Name:
          wkhtmltoimage 0.12.6 (with patched qt)

        Synopsis:
          wkhtmltoimage [OPTIONS]... <input file> <output file>

        Description:
          Converts an HTML page into an image,

        General Options:
              --allow <path>                  Allow the file or files from the specified
                                              folder to be loaded (repeatable)
              --bypass-proxy-for <value>      Bypass proxy for host (repeatable)
              --cache-dir <path>              Web cache directory
              --checkbox-checked-svg <path>   Use this SVG file when rendering checked
                                              checkboxes
              --checkbox-svg <path>           Use this SVG file when rendering unchecked
                                              checkboxes
              --cookie <name> <value>         Set an additional cookie (repeatable),
                                              value should be url encoded.
              --cookie-jar <path>             Read and write cookies from and to the
                                              supplied cookie jar file
              --crop-h <int>                  Set height for cropping
              --crop-w <int>                  Set width for cropping
              --crop-x <int>                  Set x coordinate for cropping
              --crop-y <int>                  Set y coordinate for cropping
              --custom-header <name> <value>  Set an additional HTTP header (repeatable)
              --custom-header-propagation     Add HTTP headers specified by
                                              --custom-header for each resource request.
              --no-custom-header-propagation  Do not add HTTP headers specified by
                                              --custom-header for each resource request.
              --debug-javascript              Show javascript debugging output
              --no-debug-javascript           Do not show javascript debugging output
                                              (default)
              --encoding <encoding>           Set the default text encoding, for input
          -H, --extended-help                 Display more extensive help, detailing
                                              less common command switches
          -f, --format <format>               Output file format
              --height <int>                  Set screen height (default is calculated
                                              from page content) (default 0)
          -h, --help                          Display help
              --htmldoc                       Output program html help
              --images                        Do load or print images (default)
              --no-images                     Do not load or print images
          -n, --disable-javascript            Do not allow web pages to run javascript
              --enable-javascript             Do allow web pages to run javascript
                                              (default)
              --javascript-delay <msec>       Wait some milliseconds for javascript
                                              finish (default 200)
              --license                       Output license information and exit
              --load-error-handling <handler> Specify how to handle pages that fail to
                                              load: abort, ignore or skip (default
                                              abort)
              --load-media-error-handling <handler> Specify how to handle media files
                                              that fail to load: abort, ignore or skip
                                              (default ignore)
              --disable-local-file-access     Do not allowed conversion of a local file
                                              to read in other local files, unless
                                              explicitly allowed with --allow (default)
              --enable-local-file-access      Allowed conversion of a local file to read
                                              in other local files.
              --log-level <level>             Set log level to: none, error, warn or
                                              info (default info)
              --manpage                       Output program man page
              --minimum-font-size <int>       Minimum font size
              --password <password>           HTTP Authentication password
              --disable-plugins               Disable installed plugins (default)
              --enable-plugins                Enable installed plugins (plugins will
                                              likely not work)
              --post <name> <value>           Add an additional post field (repeatable)
              --post-file <name> <path>       Post an additional file (repeatable)
          -p, --proxy <proxy>                 Use a proxy
              --proxy-hostname-lookup         Use the proxy for resolving hostnames
              --quality <int>                 Output image quality (between 0 and 100)
                                              (default 94)
          -q, --quiet                         Be less verbose, maintained for backwards
                                              compatibility; Same as using --log-level
                                              none
              --radiobutton-checked-svg <path> Use this SVG file when rendering checked
                                              radiobuttons
              --radiobutton-svg <path>        Use this SVG file when rendering unchecked
                                              radiobuttons
              --readme                        Output program readme
              --run-script <js>               Run this additional javascript after the
                                              page is done loading (repeatable)
              --disable-smart-width           Use the specified width even if it is not
                                              large enough for the content
              --enable-smart-width            Extend --width to fit unbreakable content
                                              (default)
              --ssl-crt-path <path>           Path to the ssl client cert public key in
                                              OpenSSL PEM format, optionally followed by
                                              intermediate ca and trusted certs
              --ssl-key-password <password>   Password to ssl client cert private key
              --ssl-key-path <path>           Path to ssl client cert private key in
                                              OpenSSL PEM format
              --stop-slow-scripts             Stop slow running javascripts (default)
              --no-stop-slow-scripts          Do not Stop slow running javascripts
              --transparent                   Make the background transparent in pngs
              --user-style-sheet <path>       Specify a user style sheet, to load with
                                              every page
              --username <username>           HTTP Authentication username
          -V, --version                       Output version information and exit
              --width <int>                   Set screen width, note that this is used
                                              only as a guide line. Use
                                              --disable-smart-width to make it strict.
                                              (default 1024)
              --window-status <windowStatus>  Wait until window.status is equal to this
                                              string before rendering page
              --zoom <float>                  Use this zoom factor (default 1)

        Specifying A Proxy:
          By default proxy information will be read from the environment variables:
          proxy, all_proxy and http_proxy, proxy options can also by specified with the
          -p switch

          <type> := "http://" | "socks5://"
          <serif> := <username> (":" <password>)? "@"
          <proxy> := "None" | <type>? <string>? <host> (":" <port>)?

          Here are some examples (In case you are unfamiliar with the BNF):

          http://user:password@myproxyserver:8080
          socks5://myproxyserver
          None

        Contact:
          If you experience bugs or want to request new features please visit
          <https://wkhtmltopdf.org/support.html>

     */

    #endregion

    // for later cleanup
    private List<string> _tempFiles = new List<string>( );

    /// <summary>
    /// Cleanup method
    /// </summary>
    public void Cleanup( )
    {
      foreach (var file in _tempFiles) {
        try {
          File.Delete( file );
        }
        catch { }
      }
    }



    // Deployment Ref
    private IDeployment _deployment = null;

    // Exe runner
    private WkHtmlToImageRunner _runner;

    /// <summary>
    /// The Progress and Error Event Source
    /// </summary>
    public IWrapperEvents EventSource => _runner;


    /// <summary>
    /// The command line built from Setup
    /// </summary>
    /// <returns>Commandline for wkhtmltopdf.exe</returns>
    public string CmdLine( )
    {
      if (_document != null) {
        return CmdLineFromDocument( );
      }
      else {
        // TODO add from Binding Interface
        return "";
      }
    }

    /// <summary>
    /// Convert the Request
    /// </summary>
    /// <returns>True when successfull</returns>
    /// <exception cref="InvalidOperationException">If something failed during setup</exception>
    public bool Convert( )
    {
      // sanity
      if (_runner == null) throw new InvalidOperationException( "not able to run" );

      return _runner.Run( );
    }

    #region IDocument API

    // doc to convert
    private readonly HtmlToImageDocument _document;

    /// <summary>
    /// The document to convert
    /// </summary>
    public HtmlToImageDocument Document => _document;

    /// <summary>
    /// cTor: with HtmlToPdfDocument
    /// </summary>
    /// <param name="document">A Document</param>
    /// <param name="deployment">A Deployment</param>
    /// <exception cref="ArgumentNullException">If document or deployment is null</exception>
    public ImageRequest( HtmlToImageDocument document, IDeployment deployment )
    {
      _document = document ?? throw new ArgumentNullException( "Document cannot be null" );
      _deployment = deployment ?? throw new ArgumentNullException( "deployment cannot be null" );

      _runner = new WkHtmlToImageRunner( _deployment, this );
    }

    /// <summary>
    /// Returns the Commandline for this Request
    /// </summary>
    /// <returns></returns>
    private string CmdLineFromDocument( )
    {
      CmdLineBuilder sb = new CmdLineBuilder( );
      // decompose the request
      // wkhtmltoimage [OPTIONS]... <input file> <output file>

      // *** [GLOBAL OPTION]

      // for now get max logging
#if DEBUG
      sb.AppendValue( "--log-level", "info" );
#else
      sb.AppendValue( "--log-level", "warn" ); // warn and error only
#endif
      // --allow <path>  (repeatable)
      // Add to doc if needed

      // --cookie-jar <path> 
      sb.AppendFile( "--cookie-jar", _document.CookieJar );
      if (_document.CropSettings != default) {
        // --crop-x <int>
        sb.AppendValue( "--crop-x", _document.CropSettings.Left );
        // --crop-h <int>
        sb.AppendValue( "--crop-h", _document.CropSettings.Height );
        // --crop-y <int>
        sb.AppendValue( "--crop-y", _document.CropSettings.Top );
        // --crop-w <int> 
        sb.AppendValue( "--crop-w", _document.CropSettings.Width );
      }
      // --format <format> (png, jpg, bmp, svg)
      sb.AppendValue( "--format", _document.Format.ToString( ) );
      // --quality <int>  (default 94)
      sb.AppendValue( "--quality", _document.Quality );
      // --height <int>
      sb.AppendValue( "--height", _document.ScreenHeight );
      // --width <int>  (default 1024)
      sb.AppendValue( "--width", _document.ScreenWidth );
      // --transparent 
      sb.AppendValue( "--transparent", _document.Transparent );

      if (_document.LoadSettings != default) {
        // --disable-local-file-access  (default)
        // --enable-local-file-access
        if (_document.LoadSettings.BlockLocalFileAccess == false) sb.Append( "--enable-local-file-access" );

        // --cookie <name> <value>  (repeatable)
        foreach (var cookie in _document.LoadSettings.Cookies) {
          sb.AppendQuotedValues( "--cookie", cookie.Key, cookie.Value );
        }
        // --custom-header <name> <value>  (repeatable)
        foreach (var cookie in _document.LoadSettings.CustomHeaders) {
          sb.AppendQuotedValues( "--custom-header", cookie.Key, cookie.Value );
        }
        // --no-custom-header-propagation  (default ???)
        // --custom-header-propagation 
        if (_document.LoadSettings.RepeatCustomHeaders == true) sb.Append( "--custom-header-propagation" );
        // --no-debug-javascript  (default)
        // --debug-javascript
        if (_document.LoadSettings.DebugJavascript == true) sb.Append( "--debug-javascript" );
        // --load-error-handling <handler>  abort, ignore or skip (default abort)
        if (_document.LoadSettings.ErrorHandling.HasValue) sb.AppendValue( "--load-error-handling", _document.LoadSettings.ErrorHandling.ToString( ) );

        // --username <username>
        sb.AppendQuotedValue( "--username", _document.LoadSettings.Username );
        // --password <password>
        sb.AppendQuotedValue( "--password", _document.LoadSettings.Password );

        // --post <name> <value>   (repeatable)
        // --post-file <name> <path>   (repeatable)
        foreach (var post in _document.LoadSettings.PostItems) {
          if (post.IsFile) {
            sb.AppendQuotedValues( "--post-file", post.Name, WrapperTools.FixFilePath( post.Value ) );
          }
          else {
            sb.AppendQuotedValues( "--post", post.Name, post.Value );
          }
        }
        // --proxy <proxy>  
        sb.AppendQuotedValue( "--proxy", _document.LoadSettings.Proxy );
        // Library use it seems
        // obj.LoadSettings.RenderDelay;

        // --stop-slow-scripts  (default)
        // --no-stop-slow-scripts 
        if (_document.LoadSettings.StopSlowScript == false) sb.Append( "--no-stop-slow-scripts" );

        // --window-status <windowStatus> 
        sb.AppendValue( "--window-status", _document.LoadSettings.WindowStatus );
        //--zoom <float>  (default 1)
        sb.AppendValue( "--zoom", _document.LoadSettings.ZoomFactor );
      }

      if (_document.WebSettings != default) {
        // --encoding <encoding> 
        sb.AppendValue( "--encoding", _document.WebSettings.DefaultEncoding );
        // --enable-smart-width  (default)
        // --disable-smart-width
        if (_document.WebSettings.EnableIntelligentShrinking == false) sb.Append( "--disable-smart-shrinking" );
        // --enable-javascript  (default)
        // --disable-javascript
        if (_document.WebSettings.EnableJavascript == false) sb.Append( "--disable-javascript" );
        // --disable-plugins  (default)
        // --enable-plugins Enable installed plugins (plugins will likely not work)  ??!!
        if (_document.WebSettings.EnablePlugins == true) sb.Append( "--enable-plugins" );
        // --images  (default)
        // --no-images
        if (_document.WebSettings.LoadImages == false) sb.Append( "--no-images" );
        // --minimum-font-size <int> 
        sb.AppendValue( "--minimum-font-size", _document.WebSettings.MinimumFontSize );
        // --user-style-sheet
        sb.AppendFile( "--user-style-sheet", _document.WebSettings.UserStyleSheet );
      }

      // wkhtmltoimage [OPTIONS]... <input file> <output file>
      // In Out Files
      sb.AppendFile( _document.In );
      sb.AppendFile( _document.Out );

      return sb.ToString( );
    }

    #endregion

  }
}
