using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace dNetWkhtmlWrap
{
  // NOTE: Added to Package bm98

  /// <summary>
  /// Wraps the execution of the wkhtmltopdf.exe or wkhtmltoimage.exe
  ///  in a contained execution
  /// </summary>
  public class WkWrapper : IWrapperEvents
  {
    // names of the expected executables
    /// <summary>
    /// Filename of the PDF Converter application binary
    /// </summary>
    public const string PDF_EXE_NAME = "wkhtmltopdf.exe";
    /// <summary>
    /// Filename of the Image Converter application binary
    /// </summary>
    public const string IMG_EXE_NAME = "wkhtmltoimage.exe";

    /// <summary>
    /// Signals Start of conversion run
    /// </summary>
    public event EventHandler<WrapperStartEventArgs> Started;
    private void OnStarted( WrapperStartEventArgs e ) => Started?.Invoke( this, e );

    /// <summary>
    /// Signals PhaseChange, also signals Error (Error) and End (Done)
    /// </summary>
    public event EventHandler<WrapperPhaseChangeEventArgs> PhaseChange;
    private void OnPhaseChange( WrapperPhaseChangeEventArgs e ) => PhaseChange?.Invoke( this, e );

    /// <summary>
    /// Signals End of conversion run
    /// </summary>
    public event EventHandler<WrapperEndEventArgs> Ended;
    private void OnEnded( WrapperEndEventArgs e ) => Ended?.Invoke( this, e );

    /// <summary>
    /// Signals Error
    /// </summary>
    public event EventHandler<WrapperErrorEventArgs> Error;
    private void OnError( WrapperErrorEventArgs e ) => Error?.Invoke( this, e );


    private int _instanceUsers = 0;

    private IDeployment _deployment = null;

    /// <summary>
    /// The used Deployment
    /// </summary>
    public IDeployment Deployment => _deployment;

    /// <summary>
    /// Exit Code from the last run
    /// </summary>
    public int LastExitCode { get; private set; }
    /// <summary>
    /// Error Description from the last run
    /// </summary>
    public string LastErrorDescription { get; private set; }

    // cTor: empty - hidden
    private WkWrapper( ) { }

    // ctor: not available outside - use factory method
    internal WkWrapper( IDeployment deployment )
    {
      _deployment = deployment ?? throw new ArgumentNullException( "deployment cannot be null" );
    }


    /// <summary>
    /// Convert a Document using the appropriate methods by running the executable in a separate process
    /// 
    ///  This method will block until the conversion has finished
    ///  It will return false if attempted to run concurrently
    ///  
    ///  (Use a second Instance for concurrent processing)
    /// </summary>
    /// <param name="document">A Document to convert</param>
    /// <param name="outFile">Returns the created output filename or null</param>
    /// <returns>True if successful</returns>
    public bool Convert( IDocument document, out string outFile )
    {
      outFile = null;
      Interlocked.Increment( ref _instanceUsers );

      // when there would be more than one ...
      if (_instanceUsers > 1) {
        Interlocked.Decrement( ref _instanceUsers ); // not processing
        return false;
      }

      // reset and start processing
      bool ret = false;
      LastExitCode = 0;
      LastErrorDescription = "";

      if (document is HtmlToPdfDocument hDoc) {
        try {
          // we capture all bailouts here and raise when needed
          if (string.IsNullOrWhiteSpace( hDoc.GlobalSettings.OutputFile )) {
            // must have an out file
            string fname = WrapperTools.TempFilename( "pdf" );
            hDoc.GlobalSettings.OutputFile = fname;
          }
          // create a new Request and Runner
          var request = new PdfRequest( hDoc, _deployment );
          request.EventSource.Started += EventSource_Started;
          request.EventSource.PhaseChange += EventSource_Progress;
          request.EventSource.Ended += EventSource_Ended;
          request.EventSource.Error += EventSource_Error;
          // perform conversion 
          var result = request.Convert( );
          outFile = result ? hDoc.GlobalSettings.OutputFile : null;
          ret = result;
        }
        catch (Exception ex) {
          Trace.WriteLine( $"Convert(toPDF) failed with exception\n{ex}" );
          LastExitCode = ex.HResult;
          LastErrorDescription = $"Convert(toPDF) failed with exception\n{ex.Message}";
          Interlocked.Decrement( ref _instanceUsers );
          throw ex;
        }
      }

      else if (document is HtmlToImageDocument iDoc) {
        try {
          // we capture all bailouts here and raise when needed
          if (string.IsNullOrWhiteSpace( iDoc.Out )) {
            // must have an out file
            string fname = WrapperTools.TempFilename( iDoc.Format.ToString( ) );
            iDoc.Out = fname;
          }
          // create a new Request and Runner
          var request = new ImageRequest( iDoc, _deployment );
          request.EventSource.Started += EventSource_Started;
          request.EventSource.PhaseChange += EventSource_Progress;
          request.EventSource.Ended += EventSource_Ended;
          request.EventSource.Error += EventSource_Error;
          // perform conversion 
          var result = request.Convert( );
          outFile = result ? iDoc.Out : null;
          ret = result;
        }
        catch (Exception ex) {
          Trace.WriteLine( $"Convert(toIMAGE) failed with exception\n{ex}" );
          LastExitCode = ex.HResult;
          LastErrorDescription = $"Convert(toIMAGE) failed with exception\n{ex.Message}";
          Interlocked.Decrement( ref _instanceUsers );
          throw ex;
        }
      }
      else {
        Trace.WriteLine( $"Convert(toUNKNOWN) - unsupported document type <{document}>" );
        LastExitCode = 98;
        LastErrorDescription = $"Convert(toUNKNOWN) - unsupported document type <{document}>";
      }

      Interlocked.Decrement( ref _instanceUsers );
      return ret;
    }


    // Forward the callbacks from the Runner
    private void EventSource_Started( object sender, WrapperStartEventArgs e ) => OnStarted( e );
    private void EventSource_Progress( object sender, WrapperPhaseChangeEventArgs e ) => OnPhaseChange( e );
    private void EventSource_Ended( object sender, WrapperEndEventArgs e ) => OnEnded( e );
    private void EventSource_Error( object sender, WrapperErrorEventArgs e )
    {
      LastExitCode = e.ExitCode;
      LastErrorDescription = e.ErrorDescription;
      OnError( e );
    }


  }
}
