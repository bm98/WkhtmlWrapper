using System;
using System.Collections.Generic;
using System.Text;

namespace dNetWkhtmlWrap
{
  /// <summary>
  /// Document to convert HTML to an Image
  /// </summary>
  public class HtmlToImageDocument : IDocument
  {
    /// <summary>
    /// The path of a file used to store cookies.
    /// </summary>
    public string CookieJar { get; set; }

    /// <summary>
    /// Height in pixel
    /// </summary>
    public double? ScreenHeight { get; set; }
    /// <summary>
    /// Width in pixel
    /// </summary>

    public double? ScreenWidth { get; set; }
    /// <summary>
    /// Quality setting for Jpeg conversion ..100 %
    /// </summary>

    public double? Quality { get; set; }

    /// <summary>
    /// Supported Image conversion formats
    /// </summary>
    public enum ImageFormat
    {
      /// <summary>
      /// JPG Image, Quality applies as well
      /// </summary>
      jpg,
      /// <summary>
      /// PNG Image, Transparency applies
      /// </summary>
      png,
      /// <summary>
      /// BMP image (don't know which detailed version it creates...)
      /// </summary>
      bmp,
      /// <summary>
      /// SVG Image, Transparency applies
      /// </summary>
      svg,
    }

    /// <summary>
    /// Ouput format  "jpg", "png", "bmp" or "svg".
    /// </summary>
    public ImageFormat Format { get; set; }

    /// <summary>
    /// The path of the output file. (Mandatory)
    /// </summary>
    public string Out { get; set; }
    /// <summary>
    /// The URL or path of the input file
    /// </summary>
    public string In { get; set; }
    /// <summary>
    /// When outputting a PNG or SVG, make the white background transparent. Must be either "true" or "false"
    /// </summary>
    public bool? Transparent { get; set; }
    /// <summary>
    /// Coordinates of the window to capture in pixels
    /// </summary>
    public CropSettings CropSettings {
      get {
        return this.crop;
      }
      set {
        if (value == null) {
          throw new ArgumentNullException( "value" );
        }

        this.crop = value;
      }
    }

    /// <summary>
    /// LoadSettings Record
    ///  Page specific settings related to loading content
    /// </summary>
    public LoadSettings LoadSettings {
      get {
        return this.load;
      }
      set {
        if (value == null) {
          throw new ArgumentNullException( "value" );
        }

        this.load = value;
      }
    }
    /// <summary>
    /// WebSettings Record
    ///  Web capture settings related to http retrieval
    /// </summary>
    public WebSettings WebSettings {
      get {
        return this.web;
      }
      set {
        if (value == null) {
          throw new ArgumentNullException( "value" );
        }

        this.web = value;
      }
    }
    /// <summary>
    /// Not used, will return an empty Object
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IObject> GetObjects( )
    {
      return new IObject[0];
    }


    private CropSettings crop = new CropSettings( );

    private LoadSettings load = new LoadSettings( );

    private WebSettings web = new WebSettings( );
  }
}
