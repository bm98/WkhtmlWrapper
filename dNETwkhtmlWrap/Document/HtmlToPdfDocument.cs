using System;
using System.Collections.Generic;
namespace dNetWkhtmlWrap
{
  /// <summary>
  /// Document to convert HTML to a PDF or PS document
  /// </summary>
  [Serializable]
  public class HtmlToPdfDocument : IDocument
  {
    /// <summary>
    /// cTor: empty
    /// </summary>
    public HtmlToPdfDocument( )
    {
      this.Objects = new List<ObjectSettings>( );
    }
    /// <summary>
    /// cTor: From HTML code
    /// </summary>
    /// <param name="html">HTML code line</param>
    public HtmlToPdfDocument( string html ) : this( )
    {
      this.Objects.Add( new ObjectSettings { HtmlText = html } );
    }

    /// <summary>
    /// List of Objects (ObjectSettings)
    /// </summary>
    public List<ObjectSettings> Objects { get; private set; }

    /// <summary>
    /// <summary>
    /// List of Objects to convert, either HTML code or HTML files/urls
    /// </summary>
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IObject> GetObjects( )
    {
      return Objects.ToArray( );
    }

    private GlobalSettings global = new GlobalSettings( );

    /// <summary>
    /// GlobalSettings Record
    ///  Generic settings related to loading content
    /// </summary>
    public GlobalSettings GlobalSettings {
      get {
        return this.global;
      }
      set {
        if (value == null) {
          throw new ArgumentNullException( "value" );
        }

        this.global = value;
      }
    }

    /// <summary>
    /// Shortcut to convert a HTML codeline to a Converter Document
    /// </summary>
    /// <param name="html"></param>
    public static implicit operator HtmlToPdfDocument( string html )
    {
      return new HtmlToPdfDocument( html );
    }

  }
}