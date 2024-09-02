using System;
using System.ComponentModel;

namespace dNetWkhtmlWrap
{
  /// <summary>
  /// Margin Settings
  /// </summary>
  [Serializable]
  [TypeConverter( typeof( ExpandableObjectConverter ) )]
  public class MarginSettings
  {
    /// <summary>
    /// cTor: Margins in [mm]
    /// </summary>
    public MarginSettings( )
    {
      this.Unit = Unit.Millimeters;
    }

    /// <summary>
    /// cTor: Margins in [mm]
    /// </summary>
    public MarginSettings( double top, double right, double bottom, double left ) : this( )
    {
      this.Top = top;
      this.Right = right;
      this.Bottom = bottom;
      this.Left = left;
    }
    /// <summary>
    /// Top Margin in selected Units
    /// </summary>
    public double? Top { get; set; }
    
    /// <summary>
    /// Right Margin in selected Units
    /// </summary>
    public double? Right { get; set; }

    /// <summary>
    /// Left Margin in selected Units
    /// </summary>
    public double? Left { get; set; }

    /// <summary>
    /// Bottom Margin in selected Units
    /// </summary>
    public double? Bottom { get; set; }



    /// <summary>
    /// Set all margins the same number in selected Units
    /// </summary>
    public double All {
      set {
        this.Top = this.Right = this.Bottom = this.Left = value;
      }
    }

    /// <summary>
    /// Unit 
    /// </summary>
    public Unit Unit { get; set; }
  }
}