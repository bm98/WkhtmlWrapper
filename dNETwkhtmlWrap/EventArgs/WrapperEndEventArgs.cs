using System;


namespace dNetWkhtmlWrap
{
  /// <summary>
  /// End Message from Runner
  /// </summary>
  public class WrapperEndEventArgs : EventArgs
  {
    /// <summary>
    /// Will be true when errors have been encountered
    /// Out file is not valid
    /// </summary>
    public bool WithErrors { get; set; }

    /// <summary>
    /// cTor:
    /// </summary>
    public WrapperEndEventArgs( bool withErrors )
    {
      WithErrors = withErrors;
    }

  }

}
