using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace dNetWkhtmlWrap.Tests
{
  /// <summary>
  /// Workflow Tests for Wrapper and Deployments
  /// incl some Error cases
  /// </summary>
  [TestClass]
  public class WorkflowTests : TestClassBase
  {
    public WorkflowTests( )
    {
      //
      // TODO: Add constructor logic here
      //
    }

    private TestContext testContextInstance;
    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext {
      get {
        return testContextInstance;
      }
      set {
        testContextInstance = value;
      }
    }



    // static path for test deployments
    private string TestBinPath => Path.Combine( Path.GetTempPath( ), "WKTEST_BIN" );


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

    // template to modify if arguments should be tested
    private IDocument SimpleH2Ipng_Document( )
    {
      string inFile = WkWrapperFactory.CreateTempFileFromText( "Hello World" );

      var doc = new HtmlToImageDocument( ) {
        In = inFile,
        Format = HtmlToImageDocument.ImageFormat.png,
      };

      return doc;
    }


    #region Additional test attributes
    //
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use TestInitialize to run code before running each test 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //


    [TestCleanup]
    public void CleanupSuite( )
    {
      // remove tempfiles
      foreach (var f in _tempFiles) DeleteFile( f );
    }

    #endregion

    /// <summary>
    /// Workflow
    ///   Deploy PDF to TempFolder
    ///   Convert simpleDoc to PDF
    ///   Cleanup
    /// </summary>
    [TestMethod]
    public void WfPdfDocument( )
    {
      var wrapper = WkWrapperFactory.Create(
                // Using PDF Deployment in TempLocation
                new  WinEDeploymentPdfExe( new TempFolderDeployment( ), 
                true // Force Verify
                ) );

      Assert.IsNotNull( wrapper );
      Assert.IsNotNull( wrapper.Deployment );
      string path = wrapper.Deployment.Path; // note this will expand the exe

      Assert.IsTrue( Directory.Exists( path ) );

      Assert.IsTrue( wrapper.ConvertChecked( SimpleH2P_Document( ), out string outFile ) );
      // check output
      Assert.IsNotNull( outFile );
      var fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      File.Delete( outFile );

      wrapper.Deployment.CleanEmbeddedDeployment( );
      Assert.IsFalse( Directory.Exists( path ) ); // temp deployment should be gone now
    }


    /// <summary>
    /// Workflow
    ///   Deploy ALL to TempFolder
    ///   Convert simpleDoc to PNG
    ///   Cleanup
    /// </summary>
    [TestMethod]
    public void WfPngDocument( )
    {
      var wrapper = WkWrapperFactory.Create(
                // Using IMAGE Deployment in TempLocation
                new WinEDeploymentAllExe(                  
                  new TempFolderDeployment( ) ,
                  true // Force Verify
                 ) );

      Assert.IsNotNull( wrapper );
      Assert.IsNotNull( wrapper.Deployment );
      string path = wrapper.Deployment.Path; // note this will expand the exe

      Assert.IsTrue( Directory.Exists( path ) );

      Assert.IsTrue( wrapper.ConvertChecked( SimpleH2Ipng_Document( ), out string outFile ) );
      // check output
      Assert.IsNotNull( outFile );
      var fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      File.Delete( outFile );

      wrapper.Deployment.CleanEmbeddedDeployment( );
      Assert.IsFalse( Directory.Exists( path ) ); // temp deployment should be gone now
    }


    /// <summary>
    /// Workflow
    ///   Deploy ALL (PDF and IMAGE) to TempFolder
    ///   Convert simpleDoc to PDF
    ///   Convert simpleDoc to PNG
    ///   Cleanup
    /// </summary>
    [TestMethod]
    public void WfBothDocuments( )
    {
      var wrapper = WkWrapperFactory.Create(
                // Using PDF amd IMAGE Deployment in TempLocation
                new WinEDeploymentAllExe(
                  new TempFolderDeployment( ),
                  true // Force Verify
                ) );

      Assert.IsNotNull( wrapper );
      Assert.IsNotNull( wrapper.Deployment );
      string path = wrapper.Deployment.Path; // note this will expand the exe

      Assert.IsTrue( Directory.Exists( path ) );

      // convert to PDF
      Assert.IsTrue( wrapper.ConvertChecked( SimpleH2P_Document( ), out string outFile ) );
      // check output
      Assert.IsNotNull( outFile );
      var fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      File.Delete( outFile );

      // convert to Image
      Assert.IsTrue( wrapper.ConvertChecked( SimpleH2Ipng_Document( ), out outFile ) );
      // check output
      Assert.IsNotNull( outFile );
      fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      File.Delete( outFile );

      wrapper.Deployment.CleanEmbeddedDeployment( );
      Assert.IsFalse( Directory.Exists( path ) ); // temp deployment should be gone now
    }


    /// <summary>
    /// Workflow
    ///   Deploy IMAGE to TempFolder
    ///   Convert simpleDoc to PNG
    ///   Cleanup
    ///   
    ///   REDeploy IMAGE to TempFolder by using the wrapper
    ///   Convert simpleDoc to PNG
    ///   Cleanup
    /// </summary>
    [TestMethod]
    public void WfCleanAndReuse( )
    {
      var wrapper = WkWrapperFactory.Create(
                // Using IMAGE Deployment in TempLocation
                new WinEDeploymentAllExe(
                  new TempFolderDeployment( ),
                  true  // Force Verify
                ) );

      Assert.IsNotNull( wrapper );
      Assert.IsNotNull( wrapper.Deployment );
      string path = wrapper.Deployment.Path; // note this will expand the exe

      Assert.IsTrue( Directory.Exists( path ) );

      Assert.IsTrue( wrapper.ConvertChecked( SimpleH2Ipng_Document( ), out string outFile ) );
      // check output
      Assert.IsNotNull( outFile );
      var fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      File.Delete( outFile );

      wrapper.Deployment.CleanEmbeddedDeployment( );
      Assert.IsFalse( Directory.Exists( path ) ); // temp deployment should be gone now


      // now we reuse the wrapper and the deployment should be done again
      path = wrapper.Deployment.Path; // note this will expand the exe, same when the wrapper attempts to do it's work

      Assert.IsTrue( Directory.Exists( path ) );

      Assert.IsTrue( wrapper.ConvertChecked( SimpleH2Ipng_Document( ), out outFile ) );
      // check output
      Assert.IsNotNull( outFile );
      fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      File.Delete( outFile );
      // final cleanup
      wrapper.Deployment.CleanEmbeddedDeployment( );
      Assert.IsFalse( Directory.Exists( path ) ); // temp deployment should be gone now
    }



    // *** DEPLOYMENT ERRORS

    /// <summary>
    /// Workflow
    ///   Deploy PDF to StaticFolder
    ///   Convert simpleDoc to PDF
    ///   Convert simpleDoc to PNG -- will fail with Exception
    ///   Cleanup
    /// </summary>
    [TestMethod]
    public void WfBothDocuments_NoImageConverter( )
    {
      var wrapper = WkWrapperFactory.Create(
                // Using PDF Deployment in StaticLocation
                new WinEDeploymentPdfExe( // PDF ONLY will fail to create Images
                  new StaticDeployment( TestBinPath ),
                  true  // Force Verify
                ) );

      Assert.IsNotNull( wrapper );
      Assert.IsNotNull( wrapper.Deployment );
      string path = wrapper.Deployment.Path; // note this will expand the exe

      Assert.IsTrue( Directory.Exists( path ) );

      // convert to PDF
      Assert.IsTrue( wrapper.ConvertChecked( SimpleH2P_Document( ), out string outFile ) );
      // check output
      Assert.IsNotNull( outFile );
      var fi = new FileInfo( outFile );
      Assert.IsTrue( fi.Exists );
      Assert.IsTrue( fi.Length > 10 );

      File.Delete( outFile );

      // convert to Image
      // MUST FAIL WITH ArgumentException
      Assert.ThrowsException<ArgumentException>(
         ( ) => { wrapper.ConvertChecked( SimpleH2Ipng_Document( ), out outFile ); } );

      wrapper.Deployment.CleanEmbeddedDeployment( );
      // StaticDeployment folders are not touched by the cleanup, should exist but empty
      Assert.IsTrue( Directory.Exists( TestBinPath ) ); // temp deployment should be gone now
      Assert.IsTrue( Directory.GetFileSystemEntries( TestBinPath ).Length == 0 ); // temp deployment should be gone now
    }



  }
}
