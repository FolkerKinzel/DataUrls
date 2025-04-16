[![GitHub](https://img.shields.io/github/license/FolkerKinzel/DataUrls)](https://github.com/FolkerKinzel/DataUrls/blob/master/LICENSE)


## .NET library that supports working with "data" URLs (RFC 2397)

[Project Reference and Release Notes](https://github.com/FolkerKinzel/DataUrls/releases/tag/v2.0.4)

The library contains the static `DataUrl` class that supports the "data" URL scheme ([RFC 2397](https://datatracker.ietf.org/doc/html/rfc2397)), which allows to embed data in a URI. The `DataUrl` class allows 
  - building "data" URL strings from files, byte arrays or text,
  - parsing "data" URL strings as `DataUrlInfo` structs in order to examine their content without having to allocate a lot of sub-strings, and to enable the comparison of "data" URLs for equality,
  - retrieving the embedded data from "data" URL strings,
  - retrieving an appropriate file type extension for the embedded data.

The library is designed to support performance and small heap allocation.

[See code examples on GitHub](https://github.com/FolkerKinzel/DataUrls)

[Version History](https://github.com/FolkerKinzel/DataUrls/releases)



