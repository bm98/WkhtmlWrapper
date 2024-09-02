using System;


namespace dNetWkhtmlWrap
{
  /// <summary>
  /// Progress Message from Runner
  /// </summary>
  public class WrapperPhaseChangeEventArgs : EventArgs
  {
    /// <summary>
    /// Current Phase
    /// </summary>
    public ProcessPhase Phase { get; set; }
    /// <summary>
    /// The Phase Description
    /// </summary>
    public string PhaseDescription { get; set; }

    /// <summary>
    /// cTor:
    /// </summary>
    public WrapperPhaseChangeEventArgs( ProcessPhase phase, string phaseDescription )
    {
      Phase = phase;
      PhaseDescription = phaseDescription;
    }

  }

}
