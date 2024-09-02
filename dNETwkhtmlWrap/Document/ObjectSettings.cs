using System;
using System.ComponentModel;

namespace dNetWkhtmlWrap
{
  /// <summary>
  /// Settings for PDF Objects
  /// </summary>
  [Serializable]
  [TypeConverter( typeof( ExpandableObjectConverter ) )]
  public class ObjectSettings : IObject
  {
    /// <summary>
    /// Include the page in the table of contents and outlines(default)
    /// --include-in-outline
    /// 
    /// False:
    /// Do not include the page in the table of contents and outlines
    /// --exclude-from-outline          
    /// </summary>
    public bool? IncludeInOutline { get; set; }

    /// <summary>
    /// Count pages for TOC (no effect so far ???)
    /// </summary>
    public bool? CountPages { get; set; }


    /// <summary>
    /// Define Input as an URI either a local file or an URL
    ///  should be either PageUrl or HtmlText
    /// </summary>
    public string PageUrl { get; set; }

    /// <summary>
    /// Define input as plain Html Code
    ///  should be either PageUrl or HtmlText
    /// </summary>
    public string HtmlText {
      get {
        return System.Text.Encoding.UTF8.GetString( this.data );
      }
      set {
        if (value == null) {
          throw new ArgumentNullException( "value" );
        }
        this.data = System.Text.Encoding.UTF8.GetBytes( value );
      }
    }

    /// <summary>
    /// Do not turn HTML form fields into pdf form fields (default)
    /// --disable-forms
    /// 
    /// True:
    /// Turn HTML form fields into pdf form fields
    /// --enable-forms
    /// </summary>
    public bool? ProduceForms { get; set; }

    /// <summary>
    /// Make links to remote web pages (default)
    /// --enable-external-links
    /// 
    /// False:
    /// Do not make links to remote web pages
    /// --disable-external-links
    /// </summary>
    public bool? ProduceExternalLinks { get; set; }

    /// <summary>
    /// Make local links (default)
    /// --enable-internal-links
    /// 
    /// False:
    /// Do not make local links
    /// --disable-internal-links
    /// </summary>
    public bool? ProduceLocalLinks { get; set; }

    /// <summary>
    /// Define a Header
    /// </summary>
    public HeaderSettings HeaderSettings {
      get {
        return this.header;
      }
      set {
        this.header = value ?? throw new ArgumentNullException( "value" );
      }
    }

    /// <summary>
    /// Define a Footer
    /// </summary>
    public FooterSettings FooterSettings {
      get {
        return this.footer;
      }
      set {
        this.footer = value ?? throw new ArgumentNullException( "value" );
      }
    }
    /// <summary>
    /// Define Object Load Settings
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
    /// Define Web Access Settings
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
    /// Raw Byte data from HTML text 
    ///  TODO check if this is needed when using the application
    /// </summary>
    [Browsable( false )]
    public byte[] RawData {
      get {
        return this.data;
      }
      set {
        if (value == null) {
          throw new ArgumentNullException( "value" );
        }

        this.data = value;
      }
    }

    /// <summary>
    /// Returns the Raw Byte Data from HTML text
    ///  TODO check if this is needed when using the application
    /// </summary>
    /// <returns></returns>
    public byte[] GetData( )
    {
      return RawData;
    }

    /// <summary>
    /// Create default Object using HTML code
    /// </summary>
    /// <param name="html"></param>
    public static implicit operator ObjectSettings( string html )
    {
      return new ObjectSettings { HtmlText = html };
    }

    // stores the HTML text as raw bytes
    private byte[] data = new byte[0];

    // local storage for sub settings
    private HeaderSettings header = new HeaderSettings( );
    private FooterSettings footer = new FooterSettings( );
    private LoadSettings load = new LoadSettings( );
    private WebSettings web = new WebSettings( );


  }
}