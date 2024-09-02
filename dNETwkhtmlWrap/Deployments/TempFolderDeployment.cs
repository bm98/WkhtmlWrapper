using System;
using System.Collections.Generic;
using System.Text;

namespace dNetWkhtmlWrap
{
  /// <summary>
  /// Supports a deployment in a Temp Folder within the Filesystem
  ///  Temp Path is created in cTor
  /// </summary>
  [Serializable]
  public sealed class TempFolderDeployment : IDeployment
  {
    /// <summary>
    /// Base Path for the Deployment Item
    /// </summary>
    public string Path { get; private set; }

    // The dedicated Path in UserTemp
    private string _specificPath = "";

    /// <summary>
    /// To remove the Deployed binaries and their folders
    /// </summary>
    public void CleanEmbeddedDeployment( )
    {
      // we remove what we created (or at least proposed to be created)
      if (System.IO.Directory.Exists( Path )) {
        // never fail
        try {
          System.IO.Directory.Delete( Path, true ); // clean regardless
        }
        catch { }
      }
    }

    /// <summary>
    /// cTor:
    /// </summary>
    public TempFolderDeployment( )
    {
      // Path created: <UserTemp>\XXX_XX
      // make sure it is only one subfolder deep, else the cleanup will not remove all parts...
      _specificPath = $"{AppDomain.CurrentDomain.BaseDirectory.GetHashCode( ):X}_{IntPtr.Size:X}";

      // Final Path
      Path = System.IO.Path.Combine(
          System.IO.Path.GetTempPath( ), // User Temp Location
          _specificPath );
    }

  }
}
