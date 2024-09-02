using System;
using System.Collections.Generic;
using System.Text;

namespace dNetWkhtmlWrap
{
  /// <summary>
  /// Settings for Post Item
  /// </summary>
  public sealed class PostItem
  {
    /// <summary>
    /// Item Name
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Item Value
    /// </summary>
    public string Value { get; set; }
    /// <summary>
    /// True for files (Value is a filename)
    /// </summary>
    public bool IsFile { get; set; }
  }
}
