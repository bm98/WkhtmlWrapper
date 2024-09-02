using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace dNetWkhtmlWrap
{
  // NOTE: Added to Package bm98

  /// <summary>
  /// Runs wkhtmltoimage.exe
  /// </summary>
  internal class WkHtmlToImageRunner : IWrapperEvents
  {
    private readonly IDeployment _deployment;
    private readonly ImageRequest _request;
    private readonly List<string> _outBuffer = new List<string>( );

    /// <summary>
    /// Signals Start of conversion run
    /// </summary>
    public event EventHandler<WrapperStartEventArgs> Started;
    private void OnStarted( ) => Started?.Invoke( this, new WrapperStartEventArgs( ) );

    /// <summary>
    /// Signals Progress
    /// </summary>
    public event EventHandler<WrapperPhaseChangeEventArgs> PhaseChange;
    private void OnPhaseChange( ProcessPhase phase ) => PhaseChange?.Invoke( this, new WrapperPhaseChangeEventArgs( phase, WrapperTools.PhaseDescription( phase ) ) );

    /// <summary>
    /// Signals End of conversion run
    /// </summary>
    public event EventHandler<WrapperEndEventArgs> Ended;
    private void OnEnded( bool withErrors ) => Ended?.Invoke( this, new WrapperEndEventArgs( withErrors ) );

    /// <summary>
    /// Signals Error
    /// </summary>
    public event EventHandler<WrapperErrorEventArgs> Error;
    private void OnError( int exitCode, string errText ) => Error?.Invoke( this, new WrapperErrorEventArgs( exitCode, errText ) );


    /// <summary>
    /// The output of the conversion run
    /// </summary>
    public IEnumerable<string> OutputBuffer => _outBuffer;

    // our exe - valid after deployment is accepted
    private string WorkingExe => Path.Combine( _deployment.Path, WkWrapper.IMG_EXE_NAME );

    // empty cTor is not available
    private WkHtmlToImageRunner( ) { }

    /// <summary>
    /// cTor: Provide the Exe Deployment
    /// </summary>
    /// <param name="deployment">Deployment Object</param>
    /// <param name="request">A Document conversion request</param>
    /// <exception cref="ArgumentNullException">If deployment or request is null</exception>
    public WkHtmlToImageRunner( IDeployment deployment, ImageRequest request )
    {
      _deployment = deployment ?? throw new ArgumentNullException( "Deployment cannot be null" );
      _request = request ?? throw new ArgumentNullException( "Request cannot be null" );

      if (!File.Exists( WorkingExe )) {
        throw new ArgumentException( "deployment does not provide the required exe file" );
      }

    }

    /// <summary>
    /// Run wkhtmltoimage with the Commandline from the Request
    /// </summary>
    /// <returns>True if the process did terminate as expected</returns>
    public bool Run( )
    {
      var cmdLine = _request.CmdLine( );
      if (string.IsNullOrWhiteSpace( cmdLine )) {
        _outBuffer.Add( $"WkHtmlToImage.Run: Calling {WkWrapper.IMG_EXE_NAME} with empty commandline" );
        return false; // this would open the interactive wkhtmltopdf
      }

      OnStarted( );
      _outBuffer.Clear( );

      // Use ProcessStartInfo class
      ProcessStartInfo startInfo = new ProcessStartInfo {
        CreateNoWindow = true,
        UseShellExecute = false, // to use IO redirection
        WorkingDirectory = Path.GetTempPath( ),
        FileName = WorkingExe,
        WindowStyle = ProcessWindowStyle.Hidden,
        Arguments = " " + cmdLine,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        RedirectStandardInput = false,
      };

      try {
        // Start the process with the info we specified.
        // Call WaitForExit and then the using statement will close.
        using (Process exeProcess = Process.Start( startInfo )) {
          OnPhaseChange( ProcessPhase.Begin );

          string errText = "";

          // read StdOut
          while (!exeProcess.StandardOutput.EndOfStream) {
            string line = exeProcess.StandardOutput.ReadLine( );
            _outBuffer.Add( line );
          }
          // read StdErr 
          while (!exeProcess.StandardError.EndOfStream) {
            string line = exeProcess.StandardError.ReadLine( );
            // output is:
            // PhaseText (n/t)
            // [...  progress bar ..  we omit this in the output
            if (line.StartsWith( "[" )) continue;

            _outBuffer.Add( line );
            var phase = WrapperTools.FromAppOutput( line );
            if (phase == ProcessPhase.None) {
              // other not captured text
            }
            else if (phase == ProcessPhase.Error) {
              // trigger Error event 
              errText = line;
              OnPhaseChange( phase );
            }
            else {
              // trigger PhaseChange event 
              OnPhaseChange( phase );
            }
          }

          exeProcess.WaitForExit( );
          if (exeProcess.ExitCode != 0) {
            _outBuffer.Add( $"WkHtmlToImage.Run: {WkWrapper.IMG_EXE_NAME} exit code {exeProcess.ExitCode} - {errText}" );
            OnError( exeProcess.ExitCode, errText );
            OnEnded(true);
            return false;
          }
        }
      }
      catch (Exception ex) {
        _outBuffer.Add( $"WkHtmlToImage.Run: Calling {WkWrapper.IMG_EXE_NAME} failed with Exception" );
        _outBuffer.Add( $"{ex}" );
        OnPhaseChange( ProcessPhase.Error );
        OnError( 99, ex.Message );
        OnEnded(true);
        return false;
      }
      finally {
        _request.Cleanup( );
      }

      OnEnded(false);
      return true;
    }


  }
}
