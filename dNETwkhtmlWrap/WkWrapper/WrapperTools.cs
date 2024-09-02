using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dNetWkhtmlWrap
{
  // NOTE: Added to Package bm98

  /// <summary>
  /// Some tools used in WkWrapper
  /// </summary>
  internal static class WrapperTools
  {
    /// <summary>
    /// Returns the quoted version of the argument
    ///  also escapes existing quotes on the input
    /// </summary>
    /// <param name="unquoted">argument</param>
    /// <returns>Quoted argument</returns>
    public static string QuotedString( string unquoted )
    {
      string ret = unquoted.Replace( "\"", "\\\"" ); // escape existing double quotes
      ret = "\"" + ret + "\""; // quote the former
      return ret;
    }

    /// <summary>
    /// Replaces Window Path delimiter with Unix ones or just returns the input
    ///   Compiletime Setting
    /// </summary>
    /// <param name="winPath">A Windows FilePath</param>
    /// <returns>FilePath for app use</returns>
    public static string FixFilePath( string winPath )
    {
      // when needed
      //return winPath.Replace( @"\", "/" );
      // when not needed
      return winPath; // Path delimiter replacement NOT NEEDED
    }

    /// <summary>
    /// Create a Tempfile from the string
    /// </summary>
    /// <param name="content">The text content</param>
    /// <returns>TempFile Name or null</returns>
    public static string CreateTempFileFromText( string content )
    {
      string fname = TempFilename( "html" );
      string fFull = Path.Combine( Path.GetTempPath( ), fname );
      try {
        using (StreamWriter sw = File.CreateText( fFull )) {
          sw.AutoFlush = true;
          sw.Write( content );
        }
        return fFull;
      }
      catch (Exception) {
        return null;
      }
    }

    /// <summary>
    /// Return a unique filename with the asked extension (default is .pdf)
    /// </summary>
    /// <returns>A PDF filename</returns>
    public static string TempFilename( string extension = "pdf" )
    {
      return Path.Combine( Path.GetTempPath( ), $"WkTemp_{Guid.NewGuid( ):B}.{extension}" );  // {NNNN-.. } format
    }


    #region Phase Support

    /*
    Loading pages (1/6) (1/2)
    Counting pages (2/6)
    Loading TOC (3/6)
    Resolving links (4/6)
    Loading headers and footers (5/6)
    Printing pages (6/6)
    Rendering (2/2)
    Done               
*/
    // should match the Enum 
    private static string[] _phaseDescr = new string[] {
      "Begin",
      "Loading pages",
      "Counting pages",
      "Loading TOC",
      "Resolving links",
      "Loading headers and footers",
      "Printing pages",
      "Rendering",
      "Done",
      "Error",
      "None",
    };
    // catches all with ..(n/m)
    private static Regex xPhase = new Regex( @"^(?<text>[^\(]*)(\((?<phase>\d)\/(?<nphases>\d)\))", RegexOptions.Compiled );

    /// <summary>
    /// Returns the PdfPhase from the StdErr progress of the application
    /// </summary>
    /// <param name="output">An output line</param>
    /// <returns>A PdfPhase enum</returns>
    public static ProcessPhase FromAppOutput( string output )
    {
      string text = "";
      int phase = (int)ProcessPhase.None;
      int nPhases = 0;

      Match match = xPhase.Match( output );
      if (match.Success) {
        if (match.Groups["text"].Success) { text = match.Groups["text"].Value.TrimEnd( ); }
        if (match.Groups["phase"].Success) { phase = int.Parse( match.Groups["phase"].Value ); }
        if (match.Groups["nphases"].Success) {
          nPhases = int.Parse( match.Groups["nphases"].Value );
          if (nPhases == 2) phase = (phase == 2) ? (int)ProcessPhase.Rendering : phase; // Image has Rendering as (2/2)
        }
      }
      // can be Done
      else if (output.StartsWith( "Done" )) {
        phase = (int)ProcessPhase.Done;
      }
      // can be Error
      else if (output.StartsWith( "Error" )) {
        phase = (int)ProcessPhase.Error;
      }

      return (ProcessPhase)phase;
    }

    /// <summary>
    /// Returns a Phase Description
    /// </summary>
    /// <param name="phase">A PdfPhase</param>
    /// <returns>A string</returns>
    public static string PhaseDescription( ProcessPhase phase )
    {
      if (Enum.IsDefined( typeof( ProcessPhase ), phase )) {
        return _phaseDescr[(int)phase];
      }
      return "Unknown Progress step";
    }

    #endregion

  }
}
