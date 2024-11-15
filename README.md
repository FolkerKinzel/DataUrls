# FolkerKinzel.DataUrls
[![NuGet](https://img.shields.io/nuget/v/FolkerKinzel.DataUrls)](https://www.nuget.org/packages/FolkerKinzel.DataUrls/)
[![GitHub](https://img.shields.io/github/license/FolkerKinzel/DataUrls)](https://github.com/FolkerKinzel/DataUrls/blob/master/LICENSE)
[![Stand With Ukraine](https://raw.githubusercontent.com/vshymanskyy/StandWithUkraine/main/badges/StandWithUkraine.svg)](https://stand-with-ukraine.pp.ua)

## .NET library that supports working with "data" URLs (RFC 2397)

[Project Reference](https://folkerkinzel.github.io/DataUrls/reference/)

The library contains the static `DataUrl` class that supports the "data" URL scheme ([RFC 2397](https://datatracker.ietf.org/doc/html/rfc2397)), which allows to embed data in a URI. The `DataUrl` class allows 
  - building "data" URL strings from files, byte arrays or text,
  - parsing "data" URL strings as `DataUrlInfo` structs in order to examine their content without having to allocate a lot of sub-strings, and to enable the comparison of "data" URLs for equality,
  - retrieving the embedded data from "data" URL strings,
  - retrieving an appropriate file type extension for the embedded data.

The library is designed to support performance and small heap allocation.

[Version History](https://github.com/FolkerKinzel/DataUrls/releases)

.
### Example:
Creating and parsing a "data" URL:
```csharp
using System.Diagnostics;
using FolkerKinzel.DataUrls;
using OneOf;

namespace Examples;

public static class DataUrlExample
{
    public static void Example()
    {
        string fotoFilePath = CreatePhotoFile();
        string dataUrl = DataUrl.FromFile(fotoFilePath);
        File.Delete(fotoFilePath);

        // Uncomment this to show the content of the
        // "data" URL in the Microsoft Edge browser.
        // (Make shure you have this browser installed.):
        // ShowPictureInMicrosoftEdge(dataUrl);

        Console.WriteLine(dataUrl);
        Console.WriteLine();

        // Parse the content of the "data" URL:
        _ = DataUrl.TryParse(dataUrl, out DataUrlInfo info);

        Console.WriteLine($"Contains Bytes: {info.ContainsEmbeddedBytes}");
        Console.WriteLine($"Contains Text:  {info.ContainsEmbeddedText}");
        Console.WriteLine($"MIME Type:      {info.MimeType}");
        Console.WriteLine($"File Type Ext.: {info.GetFileTypeExtension()}");
        Console.WriteLine($"Data Encoding:  {info.DataEncoding}");

        if (info.TryGetEmbeddedBytes(out byte[]? bytes))
        {
            Console.WriteLine($"Data Length:    {bytes.Length} Bytes");
        }
    }

    private static string CreatePhotoFile()
    {
        string url = "data:image/jpeg;base64,/9j/4QAYRXhpZgAASUkqAAgAAAAAAAAAAAAAAP/sABFEdWNreQABAAQAAABLAAD/4QM1aHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjYtYzE0NSA3OS4xNjIzMTksIDIwMTgvMDIvMTUtMjA6Mjk6NDMgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bXA6Q3JlYXRvclRvb2w9IkFkb2JlIFBob3Rvc2hvcCBFbGVtZW50cyAxNy4wIChXaW5kb3dzKSIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDoyMzQ2NjhFMTA5OTExMUVDODNGQ0U1Q0EzN0JFMDUxNSIgeG1wTU06RG9jdW1lbnRJRD0ieG1wLmRpZDoyMzQ2NjhFMjA5OTExMUVDODNGQ0U1Q0EzN0JFMDUxNSI+IDx4bXBNTTpEZXJpdmVkRnJvbSBzdFJlZjppbnN0YW5jZUlEPSJ4bXAuaWlkOjIzNDY2OERGMDk5MTExRUM4M0ZDRTVDQTM3QkUwNTE1IiBzdFJlZjpkb2N1bWVudElEPSJ4bXAuZGlkOjIzNDY2OEUwMDk5MTExRUM4M0ZDRTVDQTM3QkUwNTE1Ii8+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+/+4ADkFkb2JlAGTAAAAAAf/bAIQAAwICAgICAwIC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";
        string path = "";

        if (DataUrl.TryGetEmbeddedData(url, out OneOf<string, byte[]> data, out string? fileTypeExtension)
            && data.Value is byte[] bytes)
        {
            path = Path.Combine(Directory.GetCurrentDirectory(), $"{Guid.NewGuid()}{fileTypeExtension}");
            File.WriteAllBytes(path, bytes);
        }
        
        return path;
    }

    private static void ShowPictureInMicrosoftEdge(string dataUrl)
    {
        var process = new Process();
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.FileName = "msedge";
        process.StartInfo.Arguments = dataUrl;
        _ = process.Start();
    }
}

/*
Console Output:

data:image/jpeg;base64,/9j/4QAYRXhpZgAASUkqAAgAAAAAAAAAAAAAAP/sABFEdWNreQABAAQAAABLAAD/4QM1aHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjYtYzE0NSA3OS4xNjIzMTksIDIwMTgvMDIvMTUtMjA6Mjk6NDMgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bXA6Q3JlYXRvclRvb2w9IkFkb2JlIFBob3Rvc2hvcCBFbGVtZW50cyAxNy4wIChXaW5kb3dzKSIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDoyMzQ2NjhFMTA5OTExMUVDODNGQ0U1Q0EzN0JFMDUxNSIgeG1wTU06RG9jdW1lbnRJRD0ieG1wLmRpZDoyMzQ2NjhFMjA5OTExMUVDODNGQ0U1Q0EzN0JFMDUxNSI+IDx4bXBNTTpEZXJpdmVkRnJvbSBzdFJlZjppbnN0YW5jZUlEPSJ4bXAuaWlkOjIzNDY2OERGMDk5MTExRUM4M0ZDRTVDQTM3QkUwNTE1IiBzdFJlZjpkb2N1bWVudElEPSJ4bXAuZGlkOjIzNDY2OEUwMDk5MTExRUM4M0ZDRTVDQTM3QkUwNTE1Ii8+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+/+4ADkFkb2JlAGTAAAAAAf/bAIQAAwICAgICAwIC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

Contains Bytes: True
Contains Text:  False
MIME Type:      image/jpeg
File Type Ext.: .jpg
Data Encoding:  Base64
Data Length:    2472 Bytes
 */
```

