# WkhtmlWrapper

Wraps the standalone **WkHtmlToPdf.exe / WkHtmlToImage.exe** applications for Windows .Net environment  
Implemented as .NetFramework 4.8 Class Library

WkHtmlToX: https://wkhtmltopdf.org/  
Despite retired, WkHtmlToX still delivers an easy to use way to provide conversion of moderate complex HTML code to PDF and various Image formats (png, jpg, bmp, svg).

This work is derived from TuesPechkin (https://github.com/tuespetre/TuesPechkin as of August 2024).  
Items carried over are Document and Deployment; where for the Documents some adaptations for the latest version of WkHtmlToX have been made.

### dNETwkhtmlWrap
The wrapper Library 
- compiled for AnyCPU against .NetFramework 4.8
- built and tested with V 0.12.6r1 WkHtmlToX

As the name suggests the "dNETwkhtmlWrap" library wraps around the applications rather than accessing the native C++ library version. Also updating for the latest available binaries.

Pros: 
- use of the latest available converter binaries (V 0.12.6r1)
- each document conversion is an independent run of the converter program i.e. no side effects from prev. runs, no unload
- no need to care about toolsets or converters
- threadsafe when using Wrapper Instances
- unified interface for PDF and Image conversion

Cons:
- larger deployment (the 2 exe files are about the same  as the native C++ library but still are 2x 40MB)
- no Remoting / IIS support (not implemented)

#### Notable changes to the Tues Version:  

There is currently no support to return the converted document as byte[] array when calling the conversion.
i.e. the the application *will* create a document file.
If you don't specify the target file a temp file is created and the file name/path is returned when the conversion ends.

Comments are added to the Fields and Methods (partly from the converter -H output)

Deployments have a Cleanup method now

### WkWrapper.WkhtmlToX.Mxe
Embedded deployment of the latest available `wkhtmltopdf.exe` AND `wkhtmltoimage.exe` (x64) binary V 0.12.6r1 from the original source  
Source: https://github.com/wkhtmltopdf/packaging/releases/download/0.12.6-1/wkhtmltox-0.12.6-1.mxe-cross-win64.7z

Use this if PDF and IMAGE conversion is required

### WkWrapper.WkhtmlToPdf.Mxe
Embedded deployment of the latest available `wkhtmltopdf.exe` (x64) binary V 0.12.6r1 from the original source  
Source: same as above

Use this if only PDF conversion is required


## Issue Reporting

For issues related to the HTML conversion there is unfortunately no further support available as the WkHtmlToX package has been retired and no follow up is known as of Aug.2024.

For issues related to the Wrapper - please use the Issues feature of this repository

---
## Usage

### Deployment

The library exposes an 'IDeployment' interface to represent the _folder_ where wkhtmltopdf.exe and/or wkhtmltoimage.exe resides. These are the officially supported implementations:

- `EmbeddedDeployment` - this is an abstract class for embedding binaries. The repository provides an implementation of three embedded deployments containing the latest application binaries. (see above)

- `TempFolderDeployment` - this one generates a folder in the current users TEMP directory as base and then adding some version dependent subfolders where the application binaries are placed. 
Recommended for use with any of the `EmbeddedDeployment` implementations.

- `StaticDeployment` - to be used when the exe files are placed in a fixed location on a local drive. The path is given as argument.

Embedded is used with an underlying Static- or TempFolder Deployment defining where to place the binaries.  
Static can be used alone when the application binaries are placed there by other means.  
TempFolder standalone - does not make a lot of sense...  

### Define your Document

Use the corrensponding document type either to convert from HTML to PDF `HtmlToPdfDocument` or to convert from HTML to an image format `HtmlToImageDocument`

### Create a Wrapper

By calling the Factory to do so and sending the Deployment as argument

### Use the Wrapper object to perform conversions

By calling the Wrappers Convert method with the document



---

#### Create a document with options of your choosing.

For PDF conversion:  

```csharp
var myHtmlToPdfDocument = new HtmlToPdfDocument
{
    GlobalSettings =
    {
        ProduceOutline = true,
        DocumentTitle = "Pretty Websites",
        PaperSize = PaperKind.A4,
        Margins =
        {
            All = 1.375,
            Unit = Unit.Centimeters
		}
	},
    Objects = {
        new ObjectSettings { HtmlText = "<h1>Pretty Websites</h1><p>This might take a bit to convert!</p>" },
        new ObjectSettings { PageUrl = "www.google.com" },
        new ObjectSettings { PageUrl = "www.microsoft.com" },
        new ObjectSettings { PageUrl = "www.github.com" }
    }
};

// .. process

```
this will create a converted temp document as no `OutputFile =` field is defined
   

For IMAGE conversion: (only one input can be provided and it must be a file or URL)  

```csharp
    var myHtmlToImageDocument = new HtmlToImageDocument( ) {
    In = "www.github.com",
    Format = HtmlToImageDocument.ImageFormat.png
    Out = @"D:\MyFolder\Image.png",
};

// .. process

```
  
For HTML code use the convenience method provided by the Factory to create a file.

```csharp
string inFile = WkWrapperFactory.CreateTempFileFromText( "Hello World" );

var myHtmlToImageDocument = new HtmlToImageDocument( ) {
    In = inFile,
    Format = HtmlToImageDocument.ImageFormat.jpg,
    Quality = 70,
};

// .. process

// delete the inFile when done...
```
this will create a converted temp document as no `Out =` field is defined

### Convert to PDF

```csharp
var wrapper = WkWrapperFactory.Create(
    new WinEmbeddedDeploymentPdfExe(
        new TempFolderDeployment( ) ) );

if (wrapper.Convert( myHtmlToPdfDocument, out string outFile )) {
    // collect the outFile and do something
}
else {
    // wrapper.LastErrorDescription  provides information about the cause of failing
}
```
### Convert to IMAGE

```csharp
var wrapper = WkWrapperFactory.Create(
    new WinEmbeddedDeploymentImageExe(
        new TempFolderDeployment( ) ) );

if (wrapper.Convert( myHtmlToImageDocument, out string outFile )) {
    // collect the outFile and do something
}
else {
    // wrapper.LastErrorDescription  provides information about the cause of failing
}
```

### Using a Static Deployment

Where the exe files are placed by other means in `D:\WkHtmlToX\Binaries`
```csharp
var wrapper = WkWrapperFactory.Create(
    new StaticDeployment(
        @"D:\WkHtmlToX\Binaries" ) );

if (wrapper.Convert( myHtmlToImageDocument, out string outFile )) {
    // collect the outFile and do something
}
else {
    // wrapper.LastErrorDescription  provides information about the cause of failing
}
```

### Remove a TempFolderDeployment

```csharp
var wrapper = WkWrapperFactory.Create(
    new WinEmbeddedDeploymentImageExe(
        new TempFolderDeployment( ) ) );

// do something 

// this will remove the deployed files 
wrapper.Deployment.CleanEmbeddedDeployment( );

// note the items will be redeployed when accessing the Path property
// i.e. by using the wrapper again after cleanup
```

### WkWrapper Error Handling 

Exceptions are thrown for invalid arguments or null arguments  
For Conversions returning 'false' the wrapper provides: 
- LastErrorDescription
- LastExitCode (should be 0 for successfull conversions)

The Wrapper also fires Events for 
- Begin
- PhaseChange
- End
- Error 

Note: there is little to no checking for the validity of arguments used for Documents  
The converter application will fail if not able to process a document, check the LastErrorDescription for hints

## Further reading

Visit the TestCases for more examples

---


License
-------
Creative Commons Attribution 3.0 license

Copyright (c) 2024 bm98 (M.Burri)

This work, "WkhtmlWrapper", is a derivative of "TuesPechkin" used under the Creative Commons Attribution 3.0 license. 
This work is made available under the terms of the Creative Commons Attribution 3.0 license (viewable at http://creativecommons.org/licenses/by/3.0/) by bm98 (M.Burri)

TuesPechkin: https://github.com/tuespetre/TuesPechkin as of August 2024

---

Built with VisualStudio 2022 Community free version


EOD
