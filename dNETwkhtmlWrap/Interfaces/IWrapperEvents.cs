using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dNetWkhtmlWrap
{
  /// <summary>
  /// The Process Phases
  /// </summary>
  public enum ProcessPhase
  {
    /// <summary>
    /// When the app runs
    /// </summary>
    Begin = 0,
    /// <summary>
    /// WkHtmlToX reported Phase
    /// </summary>
    Loading,
    /// <summary>
    /// WkHtmlToX reported Phase
    /// </summary>
    Counting,
    /// <summary>
    /// WkHtmlToX reported Phase
    /// </summary>
    Toc,
    /// <summary>
    /// WkHtmlToX reported Phase
    /// </summary>
    Resolving,
    /// <summary>
    /// WkHtmlToX reported Phase
    /// </summary>
    Headers,
    /// <summary>
    /// WkHtmlToX reported Phase
    /// </summary>
    Printing,
    /// <summary>
    /// WkHtmlToX reported Phase
    /// </summary>
    Rendering,

    /// <summary>
    /// WkHtmlToX reported Done
    /// </summary>
    Done,

    /// <summary>
    /// On any error or Exception while running the converter
    /// </summary>
    Error,

    /// <summary>
    /// Internal event, No Phase change, can be ignored
    /// </summary>
    None,
  }

  /// <summary>
  /// Events published by WkWrapper Library
  /// </summary>
  public interface IWrapperEvents
  {
    /// <summary>
    /// Signals Start of conversion run
    /// </summary>
    event EventHandler<WrapperStartEventArgs> Started;

    /// <summary>
    /// Signals PhaseChange, also signals Error (Error) and End (Done)
    /// </summary>
    event EventHandler<WrapperPhaseChangeEventArgs> PhaseChange;

    /// <summary>
    /// Signals End of conversion run
    /// </summary>
    event EventHandler<WrapperEndEventArgs> Ended;

    /// <summary>
    /// Signals Error
    /// </summary>
    event EventHandler<WrapperErrorEventArgs> Error;

  }
}
