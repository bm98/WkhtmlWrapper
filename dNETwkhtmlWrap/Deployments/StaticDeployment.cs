using System;
using System.Collections.Generic;
using System.Text;

namespace dNetWkhtmlWrap
{
  /// <summary>
  /// Supports a user defined static deployment within the Filesystem
  /// </summary>
  [Serializable]
  public sealed class StaticDeployment : IDeployment
  {
    /// <summary>
    /// Base Path for the Deployment Item
    /// </summary>
    public string Path { get; private set; }

    /// <summary>
    /// We shall not remove anything from the static path given by the user
    /// We also not created or proposed anything here
    /// </summary>
    public void CleanEmbeddedDeployment( ) { }


    /// <summary>
    /// cTor: 
    ///   no checks for validity of the Path are applied
    /// </summary>
    /// <param name="path">Base Path for the Deployment Item</param>
    /// <exception cref="ArgumentNullException">If path is null</exception>
    public StaticDeployment( string path )
    {
      Path = path ?? throw new ArgumentNullException( "path" );
    }

  }
}
