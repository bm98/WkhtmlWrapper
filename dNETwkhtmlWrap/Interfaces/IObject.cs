using System;
using System.Collections.Generic;
using System.Text;

namespace dNetWkhtmlWrap
{
  /// <summary>
  /// Interface for HTML Object classes
  /// </summary>

  public interface IObject : ISettings
  {
    /// <summary>
    /// Returns the Html String Rawdata
    /// </summary>
    byte[] GetData( );
  }
}
