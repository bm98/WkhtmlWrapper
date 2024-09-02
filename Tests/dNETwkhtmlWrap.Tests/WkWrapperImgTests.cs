using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.IO;
using System.Threading;


namespace dNetWkhtmlWrap.Tests
{
  [TestClass]
  public class WkWrapperImgTests : TestClassBase
  {
    private IDeployment _deployment;


    // template to modify if arguments should be tested
    private IDocument SimpleH2I_Document( )
    {
      string inFile = CreateTempFileFromText( "Hello World" );

      var doc = new HtmlToImageDocument( ) {
        In = inFile
      };

      return doc;
    }


    [TestInitialize]
    public void InitSuite( )
    {
      // binaries are copied to bin folder during VS Build (copy if newer)
      _deployment = new StaticDeployment(
                           Path.Combine(
                                AppDomain.CurrentDomain.BaseDirectory,
                                "wk-ver",
                                TEST_WK_VER ) );

    }

    [TestCleanup]
    public void CleanupSuite( )
    {
      // remove tempfiles
      foreach (var f in _tempFiles) DeleteFile( f );
    }



    [TestMethod]
    public void SimpleHtmlDocument( )
    {
      var wrapper = WkWrapperFactory.Create( _deployment );

      Assert.IsTrue( wrapper.ConvertChecked( DocumentPNG( StringObjectPath( "Hello World" ) ), out string outFile ) );
      Assert.IsNotNull( outFile );
      var fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      DeleteFile( outFile );

    }

    [TestMethod]
    public void SimpleHtmlDocumentWithOutfile( )
    {
      var wrapper = WkWrapperFactory.Create( _deployment );

      string fn = Path.Combine( Path.GetTempPath( ), "Test.pdf" );

      var doc = DocumentPNG( StringObjectPath( "Hello World" ) );
      doc.Out = fn;
      Assert.IsTrue( wrapper.ConvertChecked( doc, out string outFile ) );
      Assert.IsNotNull( outFile );
      Assert.AreEqual( fn, outFile );
      var fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      DeleteFile( outFile );

    }

    [TestMethod]
    public void OneConversionFromString( )
    {
      var wrapper = WkWrapperFactory.Create( _deployment );

      Assert.IsTrue( wrapper.ConvertChecked( DocumentPNG( StringObjectPath( ) ), out string outFile ) );
      Assert.IsNotNull( outFile );
      var fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      DeleteFile( outFile );
    }

    [TestMethod]
    public void OneConversionFromUrl( )
    {
      var wrapper = WkWrapperFactory.Create( _deployment );

      Assert.IsTrue( wrapper.ConvertChecked( DocumentPNG( UrlUri( ) ), out string outFile ) );
      Assert.IsNotNull( outFile );
      var fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      DeleteFile( outFile );
    }

    [TestMethod]
    public void OneConversionFromUrlwithSSL( )
    {
      var wrapper = WkWrapperFactory.Create( _deployment );

      Assert.IsTrue( wrapper.ConvertChecked( DocumentPNG( UrlUriSSL( ) ), out string outFile ) );
      Assert.IsNotNull( outFile );
      var fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      DeleteFile( outFile );
    }

    [TestMethod]
    public void TwoSequentialConversionsFromString( )
    {
      var wrapper = WkWrapperFactory.Create( _deployment );

      Logger.LogMessage( "TwoSequentialConversionsFromString- 1. Conversion" );
      Assert.IsTrue( wrapper.ConvertChecked( DocumentPNG( StringObjectPath( ) ), out string outFile ) );
      Assert.IsNotNull( outFile );
      var fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      DeleteFile( outFile );

      Logger.LogMessage( "TwoSequentialConversionsFromString- 2. Conversion" );
      Assert.IsTrue( wrapper.ConvertChecked( DocumentPNG( StringObjectPath( ) ), out outFile ) );
      Assert.IsNotNull( outFile );
      fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      DeleteFile( outFile );

    }

    [TestMethod]
    public void TwoSequentialConversionsFromUrl( )
    {
      var wrapper = WkWrapperFactory.Create( _deployment );

      Logger.LogMessage( "TwoSequentialConversionsFromUrl- 1. Conversion" );
      Assert.IsTrue( wrapper.ConvertChecked( DocumentPNG( UrlUri( ) ), out string outFile ) );
      Assert.IsNotNull( outFile );
      var fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      DeleteFile( outFile );

      Logger.LogMessage( "TwoSequentialConversionsFromUrl- 2. Conversion" );
      Assert.IsTrue( wrapper.ConvertChecked( DocumentPNG( UrlUriSSL( ) ), out outFile ) );
      Assert.IsNotNull( outFile );
      fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      DeleteFile( outFile );
    }

    [TestMethod]
    public void OneConversionFromFile( )
    {
      string html = GetResourceString( "dNETwkhtmlWrap.Tests.Resources.page.html" );

      string fn = string.Format( "{0}.html", Path.GetTempFileName( ) );
      using (StreamWriter sw = File.CreateText( fn )) {
        sw.AutoFlush = true;
        sw.Write( html );
      }

      var wrapper = WkWrapperFactory.Create( _deployment );

      Assert.IsTrue( wrapper.ConvertChecked(
        new HtmlToImageDocument {
          In = fn,
          Format = HtmlToImageDocument.ImageFormat.png,
        },
        out string outFile ) );
      DeleteFile( fn );

      Assert.IsNotNull( outFile );
      var fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      DeleteFile( outFile );
    }

    [TestMethod]
    public void HandlesConcurrentThreads( )
    {
      int numberOfTasks = 10;
      int completed = 0;

      var tasks = Enumerable.Range( 0, numberOfTasks ).Select( i => new Task( ( ) => {
        Debug.WriteLine( string.Format( "#{0} started", i + 1 ) );

        // using one wrapper instance per thread created
        var wrapper = WkWrapperFactory.Create( _deployment );
        Logger.LogMessage( "HandlesConcurrentThreads- for task {0} Conversion", i + 1 );
        // using only local resources to not depend on WebSite loading 
        Assert.IsTrue( wrapper.ConvertChecked( DocumentPNG( StringObjectPath( ) ), out string outFile ) );
        Assert.IsNotNull( outFile );
        var fi = new FileInfo( outFile );
        Assert.IsTrue( fi.Exists );
        Assert.IsTrue( fi.Length > 10 );

        DeleteFile( outFile );

        completed++;
        Debug.WriteLine( string.Format( "#{0} completed", i + 1 ) );
      } ) );

      Parallel.ForEach( tasks, task => task.Start( ) );

      while (completed < numberOfTasks) {
        // tried using Task.WaitAll but it blocked the test engine
        Thread.Sleep( 100 );
      }
    }

    // **** ERROR HANDLING

    [TestMethod]
    public void SimpleHtmlDocumentWithOutfile_InvalidPath( )
    {
      var wrapper = WkWrapperFactory.Create( _deployment );

      string fn = Path.Combine( Path.GetTempPath( ), "NOT_EXISTING_PATH", "Test.pdf" );

      // using only local resources to not depend on WebSite loading 
      var doc = DocumentPNG( "Hello World" );
      doc.Out = fn; // setting the not existing path for test
      Assert.IsFalse( wrapper.ConvertChecked( doc, out string outFile ) );
      Assert.IsNull( outFile );
      Assert.AreNotEqual( fn, outFile );
    }

    [TestMethod]
    public void OneConversionFromUrl_InvalidURL( )
    {
      var wrapper = WkWrapperFactory.Create( _deployment );

      Assert.IsFalse( wrapper.ConvertChecked(
            DocumentPNG( "nonexistent.website.com" ),
            out string outFile ) );
      Assert.IsNull( outFile );
    }

    // **** EVENT HANDLING

    private void RunConversionPNG( bool expected, Action<WkWrapper> subscribe, IDocument document = null )
    {
      var wrapper = WkWrapperFactory.Create( _deployment );

      subscribe( wrapper );
      // defaults to using only local resources to not depend on WebSite loading 
      Assert.AreEqual( expected, wrapper.ConvertChecked( document ?? DocumentPNG( StringObjectPath( ) ), out string outFile ) );
      DeleteFile( outFile );
    }

    private void RunConversionJPG( bool expected, Action<WkWrapper> subscribe, IDocument document = null )
    {
      var wrapper = WkWrapperFactory.Create( _deployment );

      subscribe( wrapper );
      // defaults to using only local resources to not depend on WebSite loading 
      Assert.AreEqual( expected, wrapper.ConvertChecked( document ?? DocumentJPG( StringObjectPath( ) ), out string outFile ) );
      DeleteFile( outFile );
    }

    [TestMethod]
    public void CaptureStartEvent( )
    {
      var count = 0;

      RunConversionPNG( true, c => c.Started += ( s, a ) => {
        count++;
        Logger.LogMessage( "CaptureStartEvent: " );
      } );

      Assert.IsTrue( count > 0 );
    }

    [TestMethod]
    public void CapturePhaseChangeEvents_PNG( )
    {
      var count = 0;

      RunConversionPNG( true, c => c.PhaseChange += ( s, a ) => {
        count++;
        Logger.LogMessage( "CapturePhaseChangeEvents_PNG: {0} - {1}", a.Phase, a.PhaseDescription );
      } );

      Assert.IsTrue( count > 0 );
    }

    [TestMethod]
    public void CapturePhaseChangeEvents_JPG( )
    {
      var count = 0;

      RunConversionJPG( true, c => c.PhaseChange += ( s, a ) => {
        count++;
        Logger.LogMessage( "CapturePhaseChangeEvents_JPG: {0} - {1}", a.Phase, a.PhaseDescription );
      } );

      Assert.IsTrue( count > 0 );
    }

    [TestMethod]
    public void CaptureEndEvent( )
    {
      var count = 0;

      RunConversionJPG( true, c => c.Ended += ( s, a ) => {
        count++;
        Logger.LogMessage( "CaptureEndEvent: with Errors <{0}>", a.WithErrors );
      } );

      Assert.IsTrue( count > 0 );
    }


    [TestMethod]
    public void CaptureErrorEvents( )
    {
      var count = 0;

      RunConversionPNG( false,
          c => c.Error += ( s, a ) => {
            count++;
            Logger.LogMessage( "CaptureErrorEvents: {0} - {1}", a.ExitCode, a.ErrorDescription );
          },
          DocumentPNG( "nonexistent.website.com" )
        );

      Assert.IsTrue( count > 0 );
    }

    [TestMethod]
    public void CaptureEndEventWithError( )
    {
      var count = 0;

      RunConversionPNG( false,
          c => c.Ended += ( s, a ) => {
            count++;
            Logger.LogMessage( "CaptureEndEvent: with Errors <{0}>", a.WithErrors );
          },
          DocumentPNG( "nonexistent.website.com" )
        );

      Assert.IsTrue( count > 0 );
    }

  }
}
