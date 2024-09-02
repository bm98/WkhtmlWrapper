using System;
using System.ComponentModel;
using System.Globalization;

namespace dNetWkhtmlWrap
{
  /// <summary>
  /// Global Settings Record
  /// </summary>
  [Serializable]
  [TypeConverter( typeof( ExpandableObjectConverter ) )]
  public class GlobalSettings : ISettings
  {
    private MarginSettings margins = new MarginSettings( );

    /// <summary>
    /// Colormode of the Document
    /// </summary>
    public enum DocumentColorMode
    {
      /// <summary>
      /// Color mode
      /// </summary>
      Color,
      /// <summary>
      /// Grayscale mode
      /// </summary>
      Grayscale
    }
    /// <summary>
    /// Output format for the document conversion
    /// </summary>
    public enum DocumentOutputFormat
    {
      /// <summary>
      /// PDF format
      /// </summary>
      PDF,
      /// <summary>
      /// Postscript format
      /// </summary>
      PS
    }

    /// <summary>
    /// Paper orientation
    /// </summary>
    public enum PaperOrientation
    {
      /// <summary>
      /// Portrait orientation
      /// </summary>
      Portrait,
      /// <summary>
      /// Landscape orientation
      /// </summary>
      Landscape
    }

    /// <summary>
    /// Collate when printing multiple copies (default)
    /// --collate
    /// 
    /// False:
    /// Do not collate when printing multiple copies
    /// --no-collate
    /// </summary>
    public bool? Collate { get; set; }

    /// <summary>
    /// Whether to print in color or grayscale. (Default: color)
    /// 
    /// True:
    /// PDF will be generated in grayscale
    /// --grayscale
    /// </summary>
    public DocumentColorMode? ColorMode { get; set; }

    /// <summary>
    /// Read and write cookies from and to the supplied cookie jar file
    /// --cookie-jar 'path'
    /// </summary>
    public string CookieJar { get; set; }

    /// <summary>
    /// Number of copies to print into the pdf file (default 1)
    /// --copies 'number'
    /// </summary>
    public int? Copies { get; set; }

    /// <summary>
    /// The title of the generated pdf file (The title of the first document is used if not specified)
    /// --title 'text'
    /// </summary>
    public string DocumentTitle { get; set; }

    /// <summary>
    /// Change the dpi explicitly (default 96)
    /// --dpi 'dpi'
    /// </summary>
    public int? DPI { get; set; }

    /// <summary>
    /// Put an outline into the pdf (default)
    /// --outline
    /// 
    /// False:
    /// Do not put an outline into the pdf
    /// --no-outline
    /// </summary>
    public bool? ProduceOutline { get; set; }

    /// <summary>
    /// Set the depth of the outline (default 4)
    /// --outline-depth 'level'
    /// </summary>
    public int? OutlineDepth { get; set; }

    /// <summary>
    /// Dump the outline to a file
    /// --dump-outline 'file'
    /// </summary>
    public string DumpOutline { get; set; }

    /// <summary>
    /// When embedding images scale them down to this dpi (default 600)
    /// --image-dpi 'integer'
    /// </summary>
    public int? ImageDPI { get; set; }
    /// <summary>
    /// When jpeg compressing images use this quality (default 94)
    /// --image-quality 'integer'
    /// </summary>
    public int? ImageQuality { get; set; }

    /// <summary>
    /// Set orientation to Landscape or Portrait (default Portrait)
    /// --orientation 'orientation'
    /// </summary>
    public PaperOrientation? Orientation { get; set; }

    /// <summary>
    /// A file to output the converted document to.
    /// </summary>
    public string OutputFile { get; set; }

    /// <summary>
    /// Whether to output PDF or PostScript. (Default: PDF)
    ///  MAY NOT HAVE AN EFFECT (Pdf always)
    /// </summary>
    public DocumentOutputFormat? OutputFormat { get; set; }

    /// <summary>
    /// A number that is added to all page numbers when printing headers, footers and table of content.
    ///  MAY NOT HAVE AN EFFECT
    /// </summary>
    public int? PageOffset { get; set; }

    /// <summary>
    /// Set paper size to: A4, Letter, etc. (default A4)
    /// --page-size 'Size'
    /// 
    /// Will be translated into:
    /// Page height --page-height 'unitreal'
    /// Page width  --page-width 'unitreal'
    /// </summary>
    public PechkinPaperSize PaperSize { get; set; }

    /// <summary>
    /// PDF will use lossless compression (default)
    /// 
    /// False:
    /// Do not use lossless compression on pdf objects
    /// --no-pdf-compression
    /// </summary>
    public bool? UseCompression { get; set; }


    /// <summary>
    /// The margins to use throughout the document.
    /// </summary>
    public MarginSettings Margins {
      get {
        return this.margins;
      }
      set {
        if (value == null) {
          throw new ArgumentNullException( "value" );
        }

        this.margins = value;
      }
    }

    internal string MarginTop => this.GetMarginValue( this.margins.Top );
    internal string MarginLeft =>this.GetMarginValue( this.margins.Left );
    internal string MarginRight => this.GetMarginValue( this.margins.Right );
    internal string MarginBottom => this.GetMarginValue( this.margins.Bottom );

    /// <summary>
    /// The height of the output document, e.g. "12in".
    /// </summary>
    internal string PaperHeight {
      get {
        return this.PaperSize == null ? null : this.PaperSize.Height;
      }
    }

    /// <summary>
    /// The with of the output document, e.g. "4cm".
    /// </summary>
    internal string PaperWidth {
      get {
        return this.PaperSize == null ? null : this.PaperSize.Width;
      }
    }

    private string GetMarginValue( double? value )
    {
      if (!value.HasValue) {
        return null;
      }

      var strUnit = "in";

      switch (this.margins.Unit) {
        case (Unit.Centimeters):
          strUnit = "cm";
          break;
        case (Unit.Millimeters):
          strUnit = "mm";
          break;
      }

      return String.Format( "{0}{1}", value.Value.ToString( "0.##", CultureInfo.InvariantCulture ), strUnit );
    }


  }
}