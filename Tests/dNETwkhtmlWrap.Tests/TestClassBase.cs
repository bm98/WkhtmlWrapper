using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace dNetWkhtmlWrap.Tests
{
  /// <summary>
  /// Common Test Items
  ///  This section is mostly derived from TuesPechkin Tests
  /// 
  /// NOTE RE FAILED TESTCASES:
  /// 
  /// URLs do sometimes not respond while testing...
  /// in order to investigate failed testcases visit the Log and check if it was indeed a failed loading reported from the application
  /// usually a second run then succeeds
  ///
  /// I did not found reliable websites for testing 
  ///   some security site protocols may consider extensive requests as offensive and block them
  ///   The used WebSites are somehow designed for such purposes but do sometimes fail to respond
  /// 
  /// </summary>
  public abstract class TestClassBase
  {
    // Current Version of the application (related to the binaries included in the wk-ver folder)
    protected const string TEST_WK_VER = "0.12.6";

    // Test URLs 
    // The used WebSites are somehow designed for such purposes but do sometimes fail to respond
    protected const string TEST_URL = "neverSSL.com";  // non SSL site for tesing URL access
    protected const string TEST_URL_SSL = "https://example.net/";  // SSL site for tesing URL access


    // collects names of tempfiles created with CreateTempFileFromText()
    protected List<string> _tempFiles = new List<string>( );

    // create and register a tempfile for input / Image conversion cannot handle direc text input
    // will be cleaned up on exit
    protected string CreateTempFileFromText( string text )
    {
      var fPath = WkWrapperFactory.CreateTempFileFromText( text );
      _tempFiles.Add( fPath );
      return fPath;
    }

    // will not complain if the file does not exist
    protected void DeleteFile( string path )
    {
      if (string.IsNullOrEmpty( path )) return;
      try {
        File.Delete( path );
      }
      catch { }
    }


    // returns Obj with HtmlText set 
    protected ObjectSettings StringObject( )
    {
      var html = GetResourceString( "dNetWkhtmlWrap.Tests.Resources.page.html" );

      return new ObjectSettings { HtmlText = html };
    }

    // returns a file containing the HtmlCode given
    protected string StringObjectPath( string htmlCode )
    {
      var fPath = CreateTempFileFromText( htmlCode );
      _tempFiles.Add( fPath );
      return fPath;
    }

    // returns a file containing the Html 
    protected string StringObjectPath( )
    {
      return StringObjectPath( GetResourceString( "dNetWkhtmlWrap.Tests.Resources.page.html" ) );
    }

    // returns string with PageUrl
    protected static string UrlUri( ) => TEST_URL;

    // returns string with PageUrl
    protected static string UrlUriSSL( ) => TEST_URL_SSL;

    // returns Obj with PageUrl set
    protected static ObjectSettings UrlObject( ) => new ObjectSettings { PageUrl = UrlUri( ) };


    // returns Obj with PageUrl set
    protected static ObjectSettings UrlObjectSSL( ) => new ObjectSettings { PageUrl = UrlUriSSL( ) };



    // returns the given resource string
    protected string GetResourceString( string name )
    {
      if (name == null) {
        return null;
      }

      Stream s = Assembly.GetExecutingAssembly( ).GetManifestResourceStream( name );

      if (s == null) {
        return null;
      }

      return new StreamReader( s ).ReadToEnd( );
    }

    protected string GetDeploymentPath( )
    {
      return Path.Combine(
          AppDomain.CurrentDomain.BaseDirectory,
          "wk-ver",
          TEST_WK_VER );
    }

    protected HtmlToPdfDocument Document( params ObjectSettings[] objects )
    {
      var doc = new HtmlToPdfDocument( );
      doc.Objects.AddRange( objects );

      return doc;
    }

    protected HtmlToImageDocument DocumentPNG( string objectURI )
    {
      var doc = new HtmlToImageDocument( ) { In = objectURI, Format = HtmlToImageDocument.ImageFormat.png };

      return doc;
    }

    protected HtmlToImageDocument DocumentJPG( string objectURI )
    {
      var doc = new HtmlToImageDocument( ) { In = objectURI, Format = HtmlToImageDocument.ImageFormat.jpg, Quality = 70 };

      return doc;
    }


  }

  /// <summary>
  /// Extending the Wrapper for Test Runs
  /// </summary>
  public static class ExtWrapper
  {

    // runs the Wrapper and logs application errors if needed, propagate Exceptions
    public static bool ConvertChecked( this WkWrapper wrapper, IDocument document, out string outFile )
    {
      try {
        bool result = wrapper.Convert( document, out outFile );
        if (!result) {
          Logger.LogMessage( "Convert returned false: <{0}> {1}", wrapper.LastExitCode, wrapper.LastErrorDescription );
        }
        return result;
      }
      catch (Exception ex) {
        Logger.LogMessage( "Convert threw exception: <{0}> {1}", wrapper.LastExitCode, wrapper.LastErrorDescription );
        throw ex;
      }

    }
  }

}
