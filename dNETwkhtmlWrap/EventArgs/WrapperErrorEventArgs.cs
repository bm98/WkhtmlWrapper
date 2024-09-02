using System;


namespace dNetWkhtmlWrap
{
  /// <summary>
  /// Error Message from Runner
  /// </summary>
  public class WrapperErrorEventArgs : EventArgs
  {
    /// <summary>
    /// Wk application exit code when not 0
    /// 99 indicates an exception was raised
    /// </summary>
    public int ExitCode { get; set; }
    /// <summary>
    /// A description of the error if captured
    /// </summary>
    public string ErrorDescription { get; set; }

    /// <summary>
    /// cTor:
    /// </summary>
    public WrapperErrorEventArgs( int exitCode, string errorDescription )
    {
      ExitCode = exitCode;
      ErrorDescription = errorDescription;
    }

  }

}
