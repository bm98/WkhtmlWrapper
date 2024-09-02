using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace dNetWkhtmlWrap.Tests
{
  [TestClass]
  public class WkWrapperPdfTests : TestClassBase
  {

    private IDeployment deployment;


    // template to modify if arguments should be tested
    private IDocument SimpleH2P_Document( )
    {
      var doc = new HtmlToPdfDocument( ) {
        GlobalSettings = new GlobalSettings( ) {
          DocumentTitle = "Test Document",
          ProduceOutline = false,
        },
        Objects = {
          new ObjectSettings( ) {
             HtmlText="Hello World",
          }
        }

      };

      return doc;
    }


    [TestInitialize]
    public void InitSuite( )
    {
      // binaries are copied to bin folder during VS Build (copy if newer)
      deployment = new StaticDeployment(
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


    /// <summary>
    /// Used to test varous stuff and strange behaviors
    /// </summary>
    [TestMethod]
    public void DEB_TEST( )
    {
      var wrapper = WkWrapperFactory.Create( deployment );
      var doc = Document( "Hello World" );
      doc.GlobalSettings.DocumentTitle = "";
      doc.Objects[0].HeaderSettings.LeftText = "PAGE HEADER";

      doc.Objects[0].HeaderSettings.FontName = "Arial";
      doc.Objects[0].HeaderSettings.FontSize = 19;

      wrapper.ConvertChecked( doc, out string outFile );
      var fi = new FileInfo( outFile );

      DeleteFile( outFile );

    }


    [TestMethod]
    public void SimpleHtmlDocument( )
    {
      var wrapper = WkWrapperFactory.Create( deployment );

      Assert.IsTrue( wrapper.ConvertChecked( Document( "Hello World" ), out string outFile ) );
      Assert.IsNotNull( outFile );
      var fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      DeleteFile( outFile );

    }

    [TestMethod]
    public void SimpleHtmlDocumentWithOutfile( )
    {
      var wrapper = WkWrapperFactory.Create( deployment );

      string fn = Path.Combine( Path.GetTempPath( ), "Test.pdf" );

      var doc = Document( "Hello World" );
      doc.GlobalSettings.OutputFile = fn;
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
      var wrapper = WkWrapperFactory.Create( deployment );

      Assert.IsTrue( wrapper.ConvertChecked( Document( StringObject( ) ), out string outFile ) );
      Assert.IsNotNull( outFile );
      var fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      DeleteFile( outFile );
    }

    [TestMethod]
    public void OneConversionFromUrl( )
    {
      var wrapper = WkWrapperFactory.Create( deployment );

      Assert.IsTrue( wrapper.ConvertChecked( Document( UrlObject( ) ), out string outFile ) );
      Assert.IsNotNull( outFile );
      var fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      DeleteFile( outFile );
    }

    [TestMethod]
    public void OneConversionFromUrlwithSSL( )
    {
      var wrapper = WkWrapperFactory.Create( deployment );

      Assert.IsTrue( wrapper.ConvertChecked( Document( UrlObjectSSL( ) ), out string outFile ) );
      Assert.IsNotNull( outFile );
      var fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      DeleteFile( outFile );
    }

    [TestMethod]
    public void TwoSequentialConversionsFromString( )
    {
      var wrapper = WkWrapperFactory.Create( deployment );

      Logger.LogMessage( "TwoSequentialConversionsFromString- 1. Conversion" );
      Assert.IsTrue( wrapper.ConvertChecked( Document( StringObject( ) ), out string outFile ) );
      Assert.IsNotNull( outFile );
      var fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      DeleteFile( outFile );

      Logger.LogMessage( "TwoSequentialConversionsFromString- 2. Conversion" );
      Assert.IsTrue( wrapper.ConvertChecked( Document( StringObject( ) ), out outFile ) );
      Assert.IsNotNull( outFile );
      fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      DeleteFile( outFile );

    }

    [TestMethod]
    public void TwoSequentialConversionsFromUrl( )
    {
      var wrapper = WkWrapperFactory.Create( deployment );

      Logger.LogMessage( "TwoSequentialConversionsFromUrl- 1. Conversion" );
      Assert.IsTrue( wrapper.ConvertChecked( Document( UrlObject( ) ), out string outFile ) );
      Assert.IsNotNull( outFile );
      var fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      DeleteFile( outFile );

      Logger.LogMessage( "TwoSequentialConversionsFromUrl- 2. Conversion" );
      Assert.IsTrue( wrapper.ConvertChecked( Document( UrlObjectSSL( ) ), out outFile ) );
      Assert.IsNotNull( outFile );
      fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      DeleteFile( outFile );
    }

    [TestMethod]
    public void MultipleObjectConversionFromString( )
    {
      var wrapper = WkWrapperFactory.Create( deployment );

      Assert.IsTrue( wrapper.ConvertChecked( Document( "First Object as HTML", "Second Object as HTML" ), out string outFile ) );
      Assert.IsNotNull( outFile );
      var fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      DeleteFile( outFile );
    }

    [TestMethod]
    public void MultipleObjectConversionFromUrl( )
    {
      var wrapper = WkWrapperFactory.Create( deployment );

      Assert.IsTrue( wrapper.ConvertChecked( Document( UrlObject( ), UrlObjectSSL( ) ), out string outFile ) );
      Assert.IsNotNull( outFile );
      var fi = new FileInfo( outFile );
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

      var wrapper = WkWrapperFactory.Create( deployment );

      Assert.IsTrue( wrapper.ConvertChecked(
        new HtmlToPdfDocument {
          Objects = {
                    new ObjectSettings { PageUrl = fn }
          }
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
        var wrapper = WkWrapperFactory.Create( deployment );
        Logger.LogMessage( "HandlesConcurrentThreads- for task {0} Conversion", i + 1 );
        // using only local resources to not depend on WebSite loading 
        Assert.IsTrue( wrapper.ConvertChecked( Document( StringObject( ) ), out string outFile ) );
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
      var wrapper = WkWrapperFactory.Create( deployment );

      string fn = Path.Combine( Path.GetTempPath( ), "NOT_EXISTING_PATH", "Test.pdf" );

      // using local resources to not depend on WebSite loading 
      var doc = Document( "Hello World" );
      doc.GlobalSettings.OutputFile = fn; // setting the not existing path for test
      Assert.IsFalse( wrapper.ConvertChecked( doc, out string outFile ) );
      Assert.IsNull( outFile );
      Assert.AreNotEqual( fn, outFile );
    }

    [TestMethod]
    public void OneConversionFromUrl_InvalidURL( )
    {
      var wrapper = WkWrapperFactory.Create( deployment );

      Assert.IsFalse( wrapper.ConvertChecked(
            Document( new ObjectSettings {
              PageUrl = "nonexistent.website.com",
              LoadSettings =
              {
                 ErrorHandling = LoadSettings.ContentErrorHandling.Abort
              }
            } )
            , out string outFile ) );
      Assert.IsNull( outFile );
    }

    [TestMethod]
    public void NullDeployment( )
    {
      WkWrapper wrapper;
      // will throw in cTor of WkWrapper
      Assert.ThrowsException<ArgumentNullException>(
        ( ) => { wrapper = WkWrapperFactory.Create( null ); } );
    }

    [TestMethod]
    public void InvalidDeployment( )
    {
      var invalid_deployment = new StaticDeployment(
                               Path.Combine(
                                    AppDomain.CurrentDomain.BaseDirectory,
                                    "FOLDER_DOES_NOT_EXIST",
                                    TEST_WK_VER ) );

      var wrapper = WkWrapperFactory.Create( invalid_deployment );
      // using local resources to not depend on WebSite loading 
      // will throw in cTor of the Runner due to not existing exe file and then propagated to the caller
      Assert.ThrowsException<ArgumentException>(
        ( ) => { wrapper.ConvertChecked( Document( StringObject( ) ), out string outFile ); } );
    }



    // **** EVENT HANDLING

    private void RunConversion( bool expected, Action<WkWrapper> subscribe, IDocument document = null )
    {
      var wrapper = WkWrapperFactory.Create( deployment );

      subscribe( wrapper );
      // defaults to using only local resources to not depend on WebSite loading 
      Assert.AreEqual( expected, wrapper.ConvertChecked( document ?? Document( "Hello World", StringObject( ) ), out string outFile ) );
      DeleteFile( outFile );
    }

    [TestMethod]
    public void CaptureStartEvent( )
    {
      var count = 0;

      RunConversion( true, c => c.Started += ( s, a ) => {
        count++;
        Logger.LogMessage( "CaptureStartEvent: " );
      } );

      Assert.IsTrue( count > 0 );
    }

    [TestMethod]
    public void CapturePhaseChangeEvents( )
    {
      var count = 0;

      RunConversion( true, c => c.PhaseChange += ( s, a ) => {
        count++;
        Logger.LogMessage( "CapturePhaseChangeEvents: {0} - {1}", a.Phase, a.PhaseDescription );
      } );

      Assert.IsTrue( count > 0 );
    }

    [TestMethod]
    public void CaptureEndEvent( )
    {
      var count = 0;

      RunConversion( true, c => c.Ended += ( s, a ) => {
        count++;
        Logger.LogMessage( "CaptureEndEvent: with Errors <{0}>", a.WithErrors );
      } );

      Assert.IsTrue( count > 0 );
    }

    [TestMethod]
    public void CaptureErrorEvents( )
    {
      var count = 0;

      RunConversion( false,
          c => c.Error += ( s, a ) => {
            count++;
            Logger.LogMessage( "CaptureErrorEvents: {0} - {1}", a.ExitCode, a.ErrorDescription );
          },
          Document( new ObjectSettings {
            PageUrl = "nonexistent.website.com",
            LoadSettings =
              {
                  ErrorHandling = LoadSettings.ContentErrorHandling.Abort
              }
          } ) );

      Assert.IsTrue( count > 0 );
    }

    [TestMethod]
    public void CaptureEndEventWithError( )
    {
      var count = 0;

      RunConversion( false,
          c => c.Ended += ( s, a ) => {
            count++;
            Logger.LogMessage( "CaptureEndEvent: with Errors <{0}>", a.WithErrors );
          },
          Document( new ObjectSettings {
            PageUrl = "nonexistent.website.com",
            LoadSettings =
              {
                  ErrorHandling = LoadSettings.ContentErrorHandling.Abort
              }
          } ) );

      Assert.IsTrue( count > 0 );
    }


  }
}
