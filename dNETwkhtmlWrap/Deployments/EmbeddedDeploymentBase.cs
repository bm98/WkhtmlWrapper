using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace dNetWkhtmlWrap
{
  /// <summary>
  /// Template class for
  /// 
  /// Streams a deployment from the Embedded Library/Exe DLL within the Filesystem
  ///   Either into Static or Temp location
  ///   Will implicitely stream the binaries when retrieving the Path Property
  ///   Will create the required location if needed
  ///   Will NOT overwrite existing files if there are
  /// </summary>
  [Serializable]
  public abstract class EmbeddedDeploymentBase : IDeployment
  {
    /// <summary>
    /// An item to deploy
    /// </summary>
    protected class DeploymentItem
    {
      /// <summary>
      /// Filename to assign to the deployed file
      /// </summary>
      public string DeploymentName { get; set; } = "";
      /// <summary>
      /// Binary stream to deploy as file
      /// </summary>
      public Stream BStream { get; set; } = null;
      /// <summary>
      /// Original SHA-256 of this file (as HEX STRING LOWERCASE !!!!)
      /// e.g.  "64d17682320bffd45b2208ed13b136d59139e82573745f14556cf25e95cbd808"  (from 7Zip)
      /// </summary>
      public string SHA256 { get; set; } = "";
    }

    // flag
    private bool _deployed;

    /// <summary>
    /// local store of this the underlying Deployment kind (either Static- or TempFolder)
    /// </summary>
    protected IDeployment physical;

    /// <summary>
    /// (Default is false)
    /// Wether or not verify existing files and to throw an exception if the SHA of the deployed file does not match 
    /// the SHA of the Deployment Library
    /// 
    /// To be set before using the deployed files (i.e. accessing Path or run the wrapper)
    /// </summary>
    protected bool _verifyDeployment = false;

    /// <summary>
    /// Base Path for the Deployment Item
    ///   This will deploy binaries if not yet done
    /// </summary>
    /// <exception cref="InvalidOperationException">If SHA does not match</exception>
    public virtual string Path {
      get {
        // extend the path from the given deployment mode with our modifier
        var path = System.IO.Path.Combine( physical.Path, PathModifier ?? string.Empty );

        if (!_deployed) {
          // not yet deployed
          if (!Directory.Exists( path )) {
            Directory.CreateDirectory( path );
          }

          // stream all items in the deployment Library
          foreach (var nameAndContents in GetContents( )) {
            var filename = System.IO.Path.Combine( path, nameAndContents.Value.DeploymentName );

            // is check existing files asked for?
            if (_verifyDeployment && File.Exists( filename )) {
              // check sha256 of the existing file
              string sha256 = GetSHA256( filename );
              // this may fail according to the doc ??
              if (!string.IsNullOrEmpty( sha256 )) {
                bool match = (sha256 == nameAndContents.Value.SHA256);
                if (!match) {
                  // never fail to delete this file
                  try {
                    File.Delete( filename );
                  }
                  catch { }
                }
              }
            }

            // we will not overwrite existing files
            if (!File.Exists( filename )) {
              WriteStreamToFile( filename, nameAndContents.Value.BStream );

              // is check deployed files asked for?
              if (_verifyDeployment) {
                // check sha256 of the file
                string sha256 = GetSHA256( filename );
                // this may fail according to the doc ??
                if (!string.IsNullOrEmpty( sha256 )) {
                  bool match = sha256 == (nameAndContents.Value.SHA256);
                  if (match == false) {
                    throw new InvalidOperationException( "EmbeddedDeployment: SHA-256 of the deployed file does not match" );
                  }
                }
              }
            }

          }

          _deployed = true;
        }

        return path;
      }
    }


    // clean what we created here
    private void LocalCleanEmbeddedDeployment( )
    {
      string path = Path; // avoid recreation of files when using the Path property (murksy..)

      // we remove what we created here
      if (Directory.Exists( path )) {
        // never fail
        try {
          Directory.Delete( path, true );
        }
        catch { }
      }
      _deployed = false; // removed
    }

    /// <summary>
    /// To remove the deployed binaries and created folders (temp only)
    /// </summary>   
    public virtual void CleanEmbeddedDeployment( )
    {
      LocalCleanEmbeddedDeployment( );
      // let the physical deployment take care of the rest
      physical?.CleanEmbeddedDeployment( );
    }

    /// <summary>
    /// A Path modifier (sub folder) which is applied to the BasePath
    /// may help to differentiate versions of binaries
    /// 
    /// Default implementation uses the Implementors Assembly Version number
    /// </summary>
    protected virtual string PathModifier => this.GetType( ).Assembly.GetName( ).Version.ToString( );


    /// <summary>
    /// cTor: Accepting a deployment 
    ///   Currently Static or TempFolder is available
    /// </summary>
    /// <param name="physical">A deployment item which provides the base path</param>
    /// <param name="verifyDeployment">Set to verify and throw if the deployment SHA does not match</param>
    /// <exception cref="ArgumentException">If physical is null</exception>
    public EmbeddedDeploymentBase( IDeployment physical, bool verifyDeployment )
    {
      this.physical = physical ?? throw new ArgumentNullException( "physical cannot be null" );
      _verifyDeployment = verifyDeployment;

    }


    /// <summary>
    /// Returns the binaries as Stream enumerable
    ///   Key: deployed file name (make sure it is a valid filename and possibly having the correct extension)
    ///   Value: The Binary stream serving the file
    /// </summary>
    /// <returns>Binaries as Stream enumerable</returns>
    protected abstract IEnumerable<KeyValuePair<string, DeploymentItem>> GetContents( );


    // write binary to filesystem
    private void WriteStreamToFile( string fileName, Stream stream )
    {
      if (!File.Exists( fileName )) {
        var writeBuffer = new byte[8192];
        var writeLength = 0;

        using (var newFile = File.Open( fileName, FileMode.Create )) {
          while ((writeLength = stream.Read( writeBuffer, 0, writeBuffer.Length )) > 0) {
            newFile.Write( writeBuffer, 0, writeLength );
          }
          newFile.Flush( );
          newFile.Close( );
          // should be usable now...
        }
      }
    }

    // returns a sha or null if it failed
    private string GetSHA256( string filename )
    {
      string sha256 = null;

      using (var mySHA256 = SHA256Managed.Create( )) {
        // Compute and print the hash values for each file in directory.
        using (FileStream fileStream = File.OpenRead( filename )) {
          try {
            // Create a fileStream for the file.
            // Be sure it's positioned to the beginning of the stream.
            fileStream.Position = 0;
            // Compute the hash of the fileStream.
            byte[] hashValue = mySHA256.ComputeHash( fileStream );

            sha256 = ByteArrayToHexViaLookup32_lowercase( hashValue );
          }
          catch (Exception ex) {
            Tracer.Warn( $"GetSHA256: Exception: ", ex );
          }
        }
      }
      return sha256;
    }

    #region HexString

    /// <summary>
    /// THANK YOU: https://stackoverflow.com/questions/311165/how-do-you-convert-a-byte-array-to-a-hexadecimal-string-and-vice-versa/24343727#24343727
    /// To create a Hex string from a bytearray
    /// </summary>
    private static readonly uint[] _lookup32 = CreateLookup32_lowercase( );

    private static uint[] CreateLookup32_lowercase( )
    {
      var result = new uint[256];
      for (int i = 0; i < 256; i++) {
        string s = i.ToString( "x2" );  // lowercase
        result[i] = ((uint)s[0]) + ((uint)s[1] << 16);
      }
      return result;
    }

    private static string ByteArrayToHexViaLookup32_lowercase( byte[] bytes )
    {
      var lookup32 = _lookup32;
      var result = new char[bytes.Length * 2];
      for (int i = 0; i < bytes.Length; i++) {
        var val = lookup32[bytes[i]];
        result[2 * i] = (char)val;
        result[2 * i + 1] = (char)(val >> 16);
      }
      return new string( result );
    }
    #endregion

  }
}
