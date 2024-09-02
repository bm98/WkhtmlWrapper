using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dNetWkhtmlWrap
{
  // NOTE: Added to Package bm98


  /// <summary>
  /// Build Command Lines with Spacing before
  /// </summary>
  internal class CmdLineBuilder
  {
    private StringBuilder _sb = new StringBuilder( );

    // NOTE: DON'T USE LINEBREAKS FOR THIS COMMANDLINE TOOL


    /// <summary>
    /// Append to Commandline
    /// Command must expect "cmd"
    /// </summary>
    /// <param name="cmd"></param>
    public void Append( string cmd )
    {
      if (!string.IsNullOrWhiteSpace( cmd )) {
        _sb.Append( " " + cmd );
      }
    }

    /// <summary>
    /// Append to Commandline in double quotes
    /// Command must expect "cmd"
    /// </summary>
    /// <param name="cmd"></param>
    public void AppendQuoted( string cmd )
    {
      if (!string.IsNullOrWhiteSpace( cmd )) {
        _sb.Append( " " + WrapperTools.QuotedString( cmd ) );
      }
    }

    /// <summary>
    /// Append part and the value part if defined
    /// Command must expect "cmd value"
    /// </summary>
    /// <param name="cmd">Command part</param>
    /// <param name="value">Value part</param>
    public void AppendValue( string cmd, string value )
    {
      if (!string.IsNullOrWhiteSpace( value )) {
        _sb.Append( " " + cmd );
        _sb.Append( " " + value );
      }
    }
    /// <summary>
    /// Append part and the value and value2 part if defined
    /// Command must expect "cmd value value2"
    /// </summary>
    /// <param name="cmd">Command part</param>
    /// <param name="value1">Value part</param>
    /// <param name="value2">Value2 part</param>
    public void AppendValues( string cmd, string value1, string value2 )
    {
      if (!string.IsNullOrWhiteSpace( value1 )) {
        _sb.Append( " " + cmd );
        _sb.Append( " " + value1 );
        if (!string.IsNullOrWhiteSpace( value2 )) {
          _sb.Append( " " + value2 );
        }
      }
    }

    /// <summary>
    /// Append part and the value part if defined
    /// Command must expect "cmd bool"
    /// </summary>
    /// <param name="cmd">Command part</param>
    /// <param name="value">Value part</param>
    public void AppendValue( string cmd, bool? value )
    {
      if (value.HasValue) {
        _sb.Append( " " + cmd );
        _sb.Append( " " + (value.Value ? "true" : "false") );
      }
    }

    /// <summary>
    /// Append part and the value part if defined
    /// Command must expect "cmd long"
    /// </summary>
    /// <param name="part">Command part</param>
    /// <param name="value">Value part</param>
    public void AppendValue( string part, long? value )
    {
      if (value.HasValue) {
        _sb.Append( " " + part );
        _sb.Append( " " + value.Value.ToString( ) );
      }
    }

    /// <summary>
    /// Append part and the value part if defined
    /// Command must expect "cmd int"
    /// </summary>
    /// <param name="part">Command part</param>
    /// <param name="value">Value part</param>
    public void AppendValue( string part, int? value )
    {
      if (value.HasValue) {
        _sb.Append( " " + part );
        _sb.Append( " " + value.Value.ToString( ) );
      }
    }

    /// <summary>
    /// Append part and the value part if defined (default formatted 0.##)
    /// Command must expect "cmd double"
    /// </summary>
    /// <param name="part">Command part</param>
    /// <param name="value">Value part</param>
    /// <param name="format">Number format to use (optional)</param>
    public void AppendValue( string part, double? value, string format = "0.##" )
    {
      if (value.HasValue) {
        _sb.Append( " " + part );
        _sb.Append( " " + value.Value.ToString( format, CultureInfo.InvariantCulture ) );
      }
    }

    /// <summary>
    /// Append part and the value part if defined (default formatted 0.##)
    /// Command must expect "cmd float"
    /// </summary>
    /// <param name="part">Command part</param>
    /// <param name="value">Value part</param>
    /// <param name="format">Number format to use (optional)</param>
    public void AppendValue( string part, float? value, string format = "0.##" )
    {
      if (value.HasValue) {
        _sb.Append( " " + part );
        _sb.Append( " " + value.Value.ToString( format, CultureInfo.InvariantCulture ) );
      }
    }


    /// <summary>
    /// Append and double quote the value part when adding if defined
    /// Command must expect "cmd string"
    /// </summary>
    /// <param name="part">Command part</param>
    /// <param name="value">Value part (to be quoted)</param>
    public void AppendQuotedValue( string part, string value )
    {
      if (!string.IsNullOrWhiteSpace( value )) {
        _sb.Append( " " + part );
        _sb.Append( " " + WrapperTools.QuotedString( value ) );
      }
    }

    /// <summary>
    /// Append and double quote the value part when adding if defined
    /// Command must expect "cmd value1 value2"
    /// </summary>
    /// <param name="part">Command part</param>
    /// <param name="value1">First Value part (to be quoted)</param>
    /// <param name="value2">Second Value part (to be quoted)</param>
    public void AppendQuotedValues( string part, string value1, string value2 )
    {
      if (!string.IsNullOrWhiteSpace( value1 )) {
        _sb.Append( " " + part );
        _sb.Append( " " + WrapperTools.QuotedString( value1 ) );
        if (!string.IsNullOrWhiteSpace( value2 )) {
          _sb.Append( " " + WrapperTools.QuotedString( value2 ) );
        }
      }
    }

    /// <summary>
    /// Append and double quote and fix the file part when adding if defined
    /// Command must expect "filename"
    /// </summary>
    /// <param name="file">File part (to be quoted)</param>
    public void AppendFile( string file )
    {
      if (!string.IsNullOrWhiteSpace( file )) {
        this.AppendQuoted( WrapperTools.FixFilePath( file ) );
      }
    }

    /// <summary>
    /// Append and double quote and fix the file part when adding if defined
    /// Command must expect "cmd filename"
    /// </summary>
    /// <param name="cmd">Command part</param>
    /// <param name="file">File part (to be quoted)</param>
    public void AppendFile( string cmd, string file )
    {
      if (!string.IsNullOrWhiteSpace( file )) {
        this.AppendQuotedValue( cmd, WrapperTools.FixFilePath( file ) );
      }
    }

    /// <inheritdoc/>
    public override string ToString( )
    {
      return _sb.ToString( );
    }


  }
}
