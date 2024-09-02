using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dNetWkhtmlWrap.Properties;


namespace dNetWkhtmlWrap
{
  // NOTE: Copied and adapted from TuesPechkin as the rest of this library package

  /// <summary>
  /// Implements a Binary deployment for wkhtmltopdf.exe and wkhtmltoimage.exe
  /// 
  /// </summary>
  [Serializable]
  public class WinEDeploymentAllExe : EmbeddedDeploymentBase
  {
    /// <summary>
    /// cTor: Accepting a deployment 
    ///   Currently Static or TempFolder is available
    ///   Will throw if physical is null
    /// </summary>
    /// <param name="physical">A deployment item which provides the base path</param>
    /// <param name="verifyDeployment">Set to verify and throw if the deployment SHA does not match</param>
    public WinEDeploymentAllExe( IDeployment physical, bool verifyDeployment ) : base( physical, verifyDeployment ) { }

    /// <summary>
    /// Returns the binaries as Stream enumerable
    ///   Key: deployed file name (make sure it is a valid filename and possibly having the correct extension)
    ///   Value: The Binary stream serving the file
    /// </summary>
    /// <returns>Binaries as Stream enumerable</returns>
    protected override IEnumerable<KeyValuePair<string, DeploymentItem>> GetContents( )
    {
      return new KeyValuePair<string, DeploymentItem>[]
      {
        // PDF
        new KeyValuePair<string, DeploymentItem>(
        key: WkWrapper.PDF_EXE_NAME,
        value: new DeploymentItem( ) {
          DeploymentName = WkWrapper.PDF_EXE_NAME,
          BStream = new GZipStream( new MemoryStream( Resources.wkhtmltopdf_exe_64 ), CompressionMode.Decompress ),
          SHA256 = "64d17682320bffd45b2208ed13b136d59139e82573745f14556cf25e95cbd808" // must be lowercase
        } ),

        // IMAGE
        new KeyValuePair<string, DeploymentItem>(
        key: WkWrapper.IMG_EXE_NAME,
        value: new DeploymentItem( ) {
          DeploymentName = WkWrapper.IMG_EXE_NAME,
          BStream = new GZipStream( new MemoryStream( Resources.wkhtmltoimage_exe_64 ), CompressionMode.Decompress ),
          SHA256 = "7e314e81f9bfd4c7d81d79f71afa53e958096be4948bf7d290f70105c38acbce" // must be lowercase
        } ),

         };

    }

  }
}
