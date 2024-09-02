using System;
using System.Collections.Generic;
using System.Text;

namespace dNetWkhtmlWrap
{
  /// <summary>
  /// Interface for Deployment classes
  /// </summary>
  public interface IDeployment
  {
    /// <summary>
    /// Represent a path to a folder that contains the wkhtmltopdf.exe / wkhtmltoimage.exe 
    /// library and any dependencies it may have.
    /// </summary>
    string Path { get; }

    /// <summary>
    /// To remove the deployed binaries and created folders (temp only)
    /// </summary>
    void CleanEmbeddedDeployment( );

  }
}
