using System;
using System.Collections.Generic;
using System.Text;

namespace dNetWkhtmlWrap
{
  /// <summary>
  /// CropSettings Interface definition
  /// </summary>
  public class CropSettings : ISettings
  {
    /// <summary>
    /// Left/x coordinate of the window to capture in pixels. E.g. "200"
    /// </summary>
    public double? Left { get; set; }

    /// <summary>
    /// Top/y coordinate of the window to capture in pixels. E.g. "200"
    /// </summary>
    public double? Top { get; set; }

    /// <summary>
    /// Width of the window to capture in pixels. E.g. "200"
    /// </summary>
    public double? Width { get; set; }

    /// <summary>
    /// Height of the window to capture in pixels. E.g. "200"
    /// </summary>
    public double? Height { get; set; }
  }
}
