using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dNetWkhtmlWrap
{
  /// <summary>
  /// Returns a Wrapper instance
  /// </summary>
  public static class WkWrapperFactory
  {

    /// <summary>
    /// Returns a new WkWrapper
    /// </summary>
    /// <param name="deployment">A Deployment to use</param>
    /// <returns>A WkWrapper</returns>
    public static WkWrapper Create( IDeployment deployment )
    {
      // sanity
      if (deployment == null) throw new ArgumentNullException( "deployment cannot be null");

      return new WkWrapper( deployment );
    }

    /// <summary>
    /// Utility for convenience...
    /// 
    /// Create a Tempfile from the string
    /// 
    /// </summary>
    /// <param name="content">The text content</param>
    /// <returns>TempFile Name or null</returns>
    public static string CreateTempFileFromText( string content )
    {
      // sanity 
      if ( string.IsNullOrEmpty( content ) )return null;

      string fname = $"WkTemp_{Guid.NewGuid( ):B}.html"; // {NNNN-.. } format
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

  }
}
