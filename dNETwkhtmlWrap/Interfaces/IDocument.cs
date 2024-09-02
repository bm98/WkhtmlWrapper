using System;
using System.Collections.Generic;
using System.Text;

namespace dNetWkhtmlWrap
{
  /// <summary>
  /// Interface for Document classes
  /// </summary>
  public interface IDocument : ISettings
  {
    /// <summary>
    /// Returns the contained Objects
    /// </summary>
    IEnumerable<IObject> GetObjects( );
  }
}
