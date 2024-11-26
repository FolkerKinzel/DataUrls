using FolkerKinzel.DataUrls.Intls;
using FolkerKinzel.DataUrls.Properties;

namespace FolkerKinzel.DataUrls;

/// <summary>
/// Provides methods to handle <see cref="string"/>s that represent "data" URLs (RFC 2397).
/// </summary>
/// <example>
/// <note type="note">
/// For the sake of better readability, exception handling is ommitted in the example.
/// </note>
/// <para>
/// Creating and parsing a "data" URL:
/// </para>
/// <code language="c#" source="./../Examples/DataUrlExample.cs"/>
/// </example>
public static class DataUrl
{
    /// <summary>
    /// The scheme that indicates a "data" Url (RFC 2397).
    /// </summary>
    public const string Scheme = "data:";

    /// <summary>
    /// The default Internet Media Type for a "data" Url (RFC 2397) if no other is specified.
    /// </summary>
    public const string DefaultMediaType = "text/plain";

    internal const string Base64 = ";base64";

    /// <summary>
    /// Embeds text URL-encoded in a "data" URL (RFC 2397).
    /// </summary>
    /// <param name="text">The text to embed in the "data" URL. <paramref name="text"/> MUST 
    /// not be passed URL-encoded.</param>
    /// <param name="mimeType">The Internet Media Type of the <paramref name="text"/>, or 
    /// <c>null</c> for <see cref="DataUrl.DefaultMediaType"/>.</param>
    /// <param name="encoding">The encoding to use to embed the <paramref name="text"/>.</param>
    /// 
    /// <returns>A "data" URL, into which <paramref name="text"/> is embedded.</returns>
    /// 
    /// <exception cref="FormatException"><paramref name="text"/> could not URL-encoded.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string FromText(string? text,
                                  string? mimeType = DataUrl.DefaultMediaType,
                                  DataEncoding encoding = DataEncoding.Url)
        => AppendTextTo(new StringBuilder(), text, mimeType, encoding)
           .ToString();

    /// <summary>
    /// Embeds text URL-encoded in a "data" URL (RFC 2397).
    /// </summary>
    /// <param name="text">The text to embed in the "data" URL. <paramref name="text"/> MUST not 
    /// be passed URL-encoded.</param>
    /// <param name="mimeType">The <see cref="MimeType"/> of the <paramref name="text"/>.</param>
    /// <param name="encoding">The encoding to use to embed the <paramref name="text"/>.</param>
    /// 
    /// <returns>A "data" URL, into which <paramref name="text"/> is embedded.</returns>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="mimeType"/> is <c>null</c>.</exception>
    /// <exception cref="FormatException"><paramref name="text"/> could not URL-encoded.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string FromText(string? text,
                                  MimeType mimeType,
                                  DataEncoding encoding = DataEncoding.Url)
        => AppendTextTo(new StringBuilder(), text, mimeType, encoding)
           .ToString();

    /// <summary>
    /// Embeds binary data in a "data" URL (RFC 2397).
    /// </summary>
    /// <param name="bytes">The binary data to embed in the "data" URL, or <c>null</c>.</param>
    /// <param name="mimeType">The Internet Media Type of the <paramref name="bytes"/>, or <c>null</c> 
    /// for <see cref="MimeString.OctetStream"/>.</param>
    /// <param name="encoding">The encoding to use to embed the <paramref name="bytes"/>.</param>
    /// 
    /// <returns>A "data" URL, into which the binary data provided by the parameter 
    /// <paramref name="bytes"/> is embedded.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string FromBytes(byte[]? bytes,
                                   string? mimeType = MimeString.OctetStream,
                                   DataEncoding encoding = DataEncoding.Base64)
        => AppendBytesTo(new StringBuilder(), bytes.AsSpan(), mimeType, encoding).ToString();

    /// <summary>
    /// Embeds binary data in a "data" URL (RFC 2397).
    /// </summary>
    /// <param name="bytes">The binary data to embed in the "data" URL, or <c>null</c>.</param>
    /// <param name="mimeType">The Internet Media Type of the <paramref name="bytes"/>, or <c>null</c> for 
    /// <see cref="MimeString.OctetStream"/>.</param>
    /// <param name="encoding">The encoding to use to embed the <paramref name="bytes"/>.</param>
    /// 
    /// <returns>A "data" URL, into which the binary data provided by the parameter <paramref name="bytes"/>
    /// is embedded.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string FromBytes(IEnumerable<byte>? bytes,
                                   string? mimeType = MimeString.OctetStream,
                                   DataEncoding encoding = DataEncoding.Base64)
        => AppendBytesTo(new StringBuilder(), bytes.AsSpan(), mimeType, encoding).ToString();

    /// <summary>
    /// Embeds binary data in a "data" URL (RFC 2397).
    /// </summary>
    /// <param name="bytes">The binary data to embed in the "data" URL.</param>
    /// <param name="mimeType">The Internet Media Type of the <paramref name="bytes"/>, or <c>null</c> for 
    /// <see cref="MimeString.OctetStream"/>.</param>
    /// <param name="encoding">The encoding to use to embed the <paramref name="bytes"/>.</param>
    /// 
    /// <returns>A "data" URL, into which the binary data provided by the parameter <paramref name="bytes"/>
    /// is embedded.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string FromBytes(ReadOnlySpan<byte> bytes,
                                   string? mimeType = MimeString.OctetStream,
                                   DataEncoding encoding = DataEncoding.Base64)
        => AppendBytesTo(new StringBuilder(), bytes, mimeType, encoding).ToString();

    /// <summary>
    /// Embeds binary data in a "data" URL (RFC 2397).
    /// </summary>
    /// <param name="bytes">The binary data to embed in the "data" URL, or <c>null</c>.</param>
    /// <param name="mimeType">The <see cref="MimeType"/> of the <paramref name="bytes"/>.</param>
    /// <param name="encoding">The encoding to use to embed the <paramref name="bytes"/>.</param>
    /// 
    /// <returns>A "data" URL, into which the binary data provided by the parameter <paramref name="bytes"/>
    /// is embedded.</returns>
    /// 
    /// <exception cref="ArgumentException"><paramref name="mimeType"/> is an empty struct.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string FromBytes(byte[]? bytes,
                                   in MimeTypeInfo mimeType,
                                   DataEncoding encoding = DataEncoding.Base64)
        => AppendBytesTo(new StringBuilder(), bytes.AsSpan(), in mimeType, encoding).ToString();

    /// <summary>
    /// Embeds binary data in a "data" URL (RFC 2397).
    /// </summary>
    /// <param name="bytes">The binary data to embed in the "data" URL, or <c>null</c>.</param>
    /// <param name="mimeType">The <see cref="MimeType"/> of the <paramref name="bytes"/>.</param>
    /// <param name="encoding">The encoding to use to embed the <paramref name="bytes"/>.</param>
    /// 
    /// <returns>A "data" URL, into which the binary data provided by the parameter <paramref name="bytes"/> 
    /// is embedded.</returns>
    /// 
    /// <exception cref="ArgumentException"><paramref name="mimeType"/> is an empty struct.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string FromBytes(IEnumerable<byte>? bytes,
                                   in MimeTypeInfo mimeType,
                                   DataEncoding encoding = DataEncoding.Base64)
        => AppendBytesTo(new StringBuilder(), bytes.AsSpan(), in mimeType, encoding).ToString();

    /// <summary>
    /// Embeds binary data in a "data" URL (RFC 2397).
    /// </summary>
    /// <param name="bytes">The binary data to embed in the "data" URL.</param>
    /// <param name="mimeType">The <see cref="MimeType"/> of the <paramref name="bytes"/>.</param>
    /// <param name="encoding">The encoding to use to embed the <paramref name="bytes"/>.</param>
    /// 
    /// <returns>A "data" URL, into which the binary data provided by the parameter <paramref name="bytes"/>
    /// is embedded.</returns>
    /// 
    /// <exception cref="ArgumentException"><paramref name="mimeType"/> is an empty struct.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string FromBytes(ReadOnlySpan<byte> bytes,
                                   in MimeTypeInfo mimeType,
                                   DataEncoding encoding = DataEncoding.Base64)
        => AppendBytesTo(new StringBuilder(), bytes, in mimeType, encoding).ToString();

    /// <summary>
    /// Embeds the content of a file in a "data" URL (RFC 2397).
    /// </summary>
    /// <param name="filePath">Path to the file whose content is to embed in the "data" URL.</param>
    /// <param name="mimeType">The Internet Media Type ("MIME type") of the file whose content is to embed, 
    /// or <c>null</c> 
    /// to let the method automatically
    /// retrieve the <paramref name="mimeType"/> from the file type extension.</param>
    /// <param name="encoding">The encoding to use to embed the file content.</param>
    /// 
    /// <returns>A "data" URL, into which the content of the file provided by the parameter 
    /// <paramref name="filePath"/> is embedded.</returns>
    /// 
    /// <example>
    /// <note type="note">
    /// For the sake of better readability, exception handling is ommitted in the example.
    /// </note>
    /// <para>
    /// Creating and parsing a "data" URL:
    /// </para>
    /// <code language="c#" source="./../Examples/DataUrlExample.cs"/>
    /// </example>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="filePath"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="filePath"/> is not a valid file path.</exception>
    /// <exception cref="IOException">I/O error.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string FromFile(string filePath,
                                  string? mimeType = null,
                                  DataEncoding encoding = DataEncoding.Base64)
        => AppendFileTo(new StringBuilder(), filePath, mimeType, encoding)
           .ToString();

    /// <summary>
    /// Embeds the content of a file in a "data" URL (RFC 2397).
    /// </summary>
    /// <param name="filePath">Path to the file whose content is to embed in the "data" URL.</param>
    /// <param name="mimeType">The Internet media Type ("MIME type") of the file whose content is to embed.</param>
    /// <param name="encoding">The encoding to use to embed the file content.</param>
    /// 
    /// <returns>A "data" URL into which the content of the file provided by the parameter <paramref name="filePath"/> is embedded.</returns>
    /// 
    ///<exception cref="ArgumentNullException"><paramref name="filePath"/> or <paramref name="mimeType"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">
    /// <para>
    /// <paramref name="filePath"/> is not a valid file path
    /// </para>
    /// <para>- or -</para>
    /// <para><paramref name="mimeType"/> is an empty struct.</para></exception>
    /// <exception cref="IOException">I/O error.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string FromFile(string filePath,
                                  in MimeTypeInfo mimeType,
                                  DataEncoding encoding = DataEncoding.Base64)
        => AppendFileTo(new StringBuilder(), filePath, in mimeType, encoding)
           .ToString();

    /// <summary>
    /// Appends embedded text as "data" URL (RFC 2397) to the end of a <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to which a "data" URL is appended.</param>
    /// <param name="text">The text to embed in the "data" URL. <paramref name="text"/> MUST not be 
    /// passed URL-encoded.</param>
    /// <param name="mimeType">The Internet Media Type of the <paramref name="text"/>, or <c>null</c> for 
    /// <see cref="DataUrl.DefaultMediaType"/>.</param>
    /// <param name="encoding">The encoding to use to embed the <paramref name="text"/>.</param>
    /// 
    /// <returns>A reference to <paramref name="builder"/>.</returns>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="builder"/>
    /// is <c>null</c>.</exception>
    /// <exception cref="FormatException"><paramref name="text"/> could not URL-encoded.</exception>
    public static StringBuilder AppendTextTo(StringBuilder builder,
                                             string? text,
                                             string? mimeType = DataUrl.DefaultMediaType,
                                             DataEncoding encoding = DataEncoding.Url)
        => MimeType.TryParse(string.IsNullOrWhiteSpace(mimeType) ? DataUrl.DefaultMediaType
                                                                 : mimeType,
                             out MimeType? mimeTypeObject)
                ? AppendTextTo(builder, text, mimeTypeObject, encoding)
                : AppendTextTo(builder, text, DataUrl.DefaultMediaType, encoding);

    /// <summary>
    /// Appends embedded text as "data" URL (RFC 2397) to the end of a <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to which a "data" URL is appended.</param>
    /// <param name="text">The text to embed in the "data" URL. <paramref name="text"/> MUST not be 
    /// passed URL-encoded.</param>
    /// <param name="mimeType">The <see cref="MimeType"/> of the <paramref name="text"/>.</param>
    /// <param name="encoding">The encoding to use to embed the <paramref name="text"/>.</param>
    /// 
    /// <returns>A reference to <paramref name="builder"/>.</returns>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> or <paramref name="mimeType"/> 
    /// is <c>null</c>.</exception>
    /// <exception cref="FormatException"><paramref name="text"/> could not URL-encoded.</exception>
    public static StringBuilder AppendTextTo(StringBuilder builder,
                                             string? text,
                                             MimeType mimeType,
                                             DataEncoding encoding = DataEncoding.Url) =>
        builder is null
            ? throw new ArgumentNullException(nameof(builder))
            : mimeType is null
                ? throw new ArgumentNullException(nameof(mimeType))
                : builder.AppendEmbeddedTextInternal(text, mimeType, encoding);

    /// <summary>
    /// Appends binary data as "data" URL (RFC 2397) to the end of a <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to which a "data" URL is appended.</param>
    /// <param name="bytes">The binary data to embed in the "data" URL, or <c>null</c>.</param>
    /// <param name="mimeType">The Internet Media Type of the <paramref name="bytes"/>, or <c>null</c> for 
    /// <see cref="MimeString.OctetStream"/>.</param>
    /// <param name="encoding">The encoding to use to embed the <paramref name="bytes"/>.</param>
    /// 
    /// <returns>A reference to <paramref name="builder"/>.</returns>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendBytesTo(StringBuilder builder,
                                              byte[]? bytes,
                                              string? mimeType = MimeString.OctetStream,
                                              DataEncoding encoding = DataEncoding.Base64)
        => AppendBytesTo(builder, bytes.AsSpan(), mimeType, encoding);

    /// <summary>
    /// Appends binary data as "data" URL (RFC 2397) to the end of a <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to which a "data" URL is appended.</param>
    /// <param name="bytes">The binary data to embed in the "data" URL, or <c>null</c>.</param>
    /// <param name="mimeType">The Internet Media Type of the <paramref name="bytes"/>, or <c>null</c> for 
    /// <see cref="MimeString.OctetStream"/>.</param>
    /// <param name="encoding">The encoding to use to embed the <paramref name="bytes"/>.</param>
    /// 
    /// <returns>A reference to <paramref name="builder"/>.</returns>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendBytesTo(StringBuilder builder,
                                              IEnumerable<byte>? bytes,
                                              string? mimeType = MimeString.OctetStream,
                                              DataEncoding encoding = DataEncoding.Base64)
        => AppendBytesTo(builder, bytes.AsSpan(), mimeType, encoding);

    /// <summary>
    /// Appends binary data as "data" URL (RFC 2397) to the end of a <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to which a "data" URL is appended.</param>
    /// <param name="bytes">The binary data to embed in the "data" URL.</param>
    /// <param name="mimeType">The Internet Media Type of the <paramref name="bytes"/>, or <c>null</c> for 
    /// <see cref="MimeString.OctetStream"/>.</param>
    /// <param name="encoding">The encoding to use to embed the <paramref name="bytes"/>.</param>
    /// 
    /// <returns>A reference to <paramref name="builder"/>.</returns>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <c>null</c>.</exception>
    public static StringBuilder AppendBytesTo(StringBuilder builder,
                                              ReadOnlySpan<byte> bytes,
                                              string? mimeType = MimeString.OctetStream,
                                              DataEncoding encoding = DataEncoding.Base64)

        => MimeTypeInfo.TryParse(string.IsNullOrWhiteSpace(mimeType) ? MimeString.OctetStream
                                                                     : mimeType,
                                 out MimeTypeInfo mimeTypeInfo)
                ? builder?.AppendEmbeddedBytesIntl(bytes, in mimeTypeInfo, encoding)
                         ?? throw new ArgumentNullException(nameof(builder))
                : AppendBytesTo(builder, bytes, MimeString.OctetStream, encoding);

    /// <summary>
    /// Appends binary data as "data" URL (RFC 2397) to the end of a <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to which a "data" URL is appended.</param>
    /// <param name="bytes">The binary data to embed in the "data" URL, or <c>null</c>.</param>
    /// <param name="mimeType">The Internet Media Type of the <paramref name="bytes"/>.</param>
    /// <param name="encoding">The encoding to use to embed the <paramref name="bytes"/>.</param>
    /// 
    /// <returns>A reference to <paramref name="builder"/>.</returns>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="mimeType"/> is an empty struct.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendBytesTo(StringBuilder builder,
                                              byte[]? bytes,
                                              in MimeTypeInfo mimeType,
                                              DataEncoding encoding = DataEncoding.Base64)
        => AppendBytesTo(builder, bytes.AsSpan(), in mimeType, encoding);

    /// <summary>
    /// Appends binary data as "data" URL (RFC 2397) to the end of a <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to which a "data" URL is appended.</param>
    /// <param name="bytes">The binary data to embed in the "data" URL, or <c>null</c>.</param>
    /// <param name="mimeType">The Internet Media Type of the <paramref name="bytes"/>, or <c>null</c> for 
    /// <see cref="MimeString.OctetStream"/>.</param>
    /// <param name="encoding">The encoding to use to embed the <paramref name="bytes"/>.</param>
    /// 
    /// <returns>A reference to <paramref name="builder"/>.</returns>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendBytesTo(StringBuilder builder,
                                              IEnumerable<byte>? bytes,
                                              in MimeTypeInfo mimeType,
                                              DataEncoding encoding = DataEncoding.Base64)
        => AppendBytesTo(builder, bytes.AsSpan(), in mimeType, encoding);

    /// <summary>
    /// Appends binary data as "data" URL (RFC 2397) to the end of a <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to which a "data" URL is appended.</param>
    /// <param name="bytes">The binary data to embed in the "data" URL.</param>
    /// <param name="mimeType">The Internet Media Type of the <paramref name="bytes"/>.</param>
    /// <param name="encoding">The encoding to use to embed the <paramref name="bytes"/>.</param>
    /// 
    /// <returns>A reference to <paramref name="builder"/>.</returns>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="mimeType"/> is an empty struct.</exception>
    public static StringBuilder AppendBytesTo(StringBuilder builder,
                                              ReadOnlySpan<byte> bytes,
                                              in MimeTypeInfo mimeType,
                                              DataEncoding encoding = DataEncoding.Base64)
        => builder is null
            ? throw new ArgumentNullException(nameof(builder))
            : mimeType.IsEmpty
                ? throw new ArgumentException(Res.EmptyStruct, nameof(mimeType))
                : builder.AppendEmbeddedBytesIntl(bytes, in mimeType, encoding);

    /// <summary>
    /// Appends the content of a file as "data" URL (RFC 2397) to the end of a <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to which a "data" URL is appended.</param>
    /// <param name="filePath">Abolute path to the file whose content is to embed in the "data" URL.</param>
    /// <param name="mimeType">The Internet Media Type ("MIME type") of the file whose content is to embed, or <c>null</c> 
    /// to let the method automatically retrieve the <see cref="MimeType"/> from the file type extension.</param>
    /// <param name="encoding">The encoding to use to embed the file content.</param>
    /// 
    /// <returns>A reference to <paramref name="builder"/>.</returns>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> or <paramref name="filePath"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="filePath"/> is not a valid file path.</exception>
    /// <exception cref="IOException">I/O error.</exception>
    public static StringBuilder AppendFileTo(StringBuilder builder,
                                             string filePath,
                                             string? mimeType = null,
                                             DataEncoding encoding = DataEncoding.Base64)
        => string.IsNullOrWhiteSpace(mimeType)
              ? AppendFileTo(builder, filePath, MimeString.FromFileName(filePath), encoding)
              : MimeTypeInfo.TryParse(mimeType, out MimeTypeInfo mimeTypeInfo)
                  ? AppendFileTo(builder, filePath, in mimeTypeInfo, encoding)
                  : AppendFileTo(builder, filePath, (string?)null, encoding);

    /// <summary>
    /// Appends the content of a file as "data" URL (RFC 2397) to the end of a <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to which a "data" URL is appended.</param>
    /// <param name="filePath">Abolute path to the file whose content is to embed in the "data" URL.</param>
    /// <param name="mimeType">The Internet Media Type ("MIME type") of the file whose content is to embed.</param>
    /// <param name="encoding">The encoding to use to embed the file content.</param>
    /// 
    /// <returns>A reference to <paramref name="builder"/>.</returns>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> or <paramref name="filePath"/>, 
    /// is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">
    /// <para>
    /// <paramref name="filePath"/> is not a valid file path
    /// </para>
    /// <para>- or -</para>
    /// <para><paramref name="mimeType"/> is an empty struct.</para></exception>
    /// <exception cref="IOException">I/O error.</exception>
    public static StringBuilder AppendFileTo(StringBuilder builder,
                                             string filePath,
                                             in MimeTypeInfo mimeType,
                                             DataEncoding encoding = DataEncoding.Base64)
        => builder is null
            ? throw new ArgumentNullException(nameof(builder))
            : filePath is null
                ? throw new ArgumentNullException(nameof(filePath))
                : mimeType.IsEmpty
                     ? throw new ArgumentException(Res.EmptyStruct, nameof(mimeType))
                     : builder.AppendFileContentInternal(filePath, in mimeType, encoding);

    /// <summary>
    /// Tries to retrieve the embedded data as a union that contains either a <see cref="string"/> 
    /// or an array of <see cref="byte"/>s, and also an appropriate file type extension for this data.
    /// </summary>
    /// <param name="dataUrl">A "data" URL according to RFC 2397.</param>
    /// <param name="data">If the method returns <c>true</c>, the parameter contains the embedded
    /// data as an <see cref="EmbeddedData"/> union. The parameter is passed uninitialized.</param>
    /// <param name="fileTypeExtension">If the method returns <c>true</c>, the parameter contains an 
    /// appropriate file type extension for <paramref name="data"/>. The extension starts with 
    /// the period ".". The parameter is passed uninitialized.</param>
    /// 
    /// <returns><c>true</c> if <paramref name="dataUrl"/> is a valid "data" URL, otherwise <c>false</c>.
    /// </returns>
    /// 
    /// <remarks>Use this method if you want to use the data in the application.</remarks>
    /// 
    /// <example>
    /// <note type="note">
    /// For the sake of better readability, exception handling is ommitted in the example.
    /// </note>
    /// <para>
    /// Creating and parsing a "data" URL:
    /// </para>
    /// <code language="c#" source="./../Examples/DataUrlExample.cs"/>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetData(string? dataUrl,
                                  out EmbeddedData data,
                                  [NotNullWhen(true)] out string? fileTypeExtension)
        => TryGetData(dataUrl.AsMemory(), out data, out fileTypeExtension);

    /// <summary>
    /// Tries to retrieve the embedded data as a union that contains either a <see cref="string"/> 
    /// or an array of <see cref="byte"/>s, and also an appropriate file type extension for this data.
    /// </summary>
    /// <param name="dataUrl">A read-only memory that contains a "data" URL according to RFC 2397.</param>
    ///  <param name="data">If the method returns <c>true</c>, the parameter contains the embedded
    /// data as an <see cref="EmbeddedData"/> union. The parameter is passed uninitialized.</param>
    /// <param name="fileTypeExtension">If the method returns <c>true</c>, the parameter contains an 
    /// appropriate file type extension for <paramref name="data"/>. The extension starts with 
    /// the period ".". The parameter is passed uninitialized.</param>
    /// 
    /// <returns><c>true</c> if <paramref name="dataUrl"/> is a valid "data" URL, otherwise <c>false</c>.</returns>
    /// 
    /// <remarks>Use this method if you want to use the data in the application.</remarks>
    /// 
    public static bool TryGetData(ReadOnlyMemory<char> dataUrl,
                                  out EmbeddedData data,
                                  [NotNullWhen(true)] out string? fileTypeExtension)
    {
        if (!DataUrlInfo.TryParseInternal(ref dataUrl, out DataUrlInfo info))
        {
            goto Fail;
        }

        if (info.TryGetData(out data))
        {
            fileTypeExtension = info.GetFileTypeExtension();
            return true;
        }

    Fail:
        data = default;
        fileTypeExtension = null;
        return false;
    }

    /// <summary>
    /// Tries to retrieve the embedded data as a <see cref="byte"/> array, and also an appropriate file 
    /// type extension for this data.
    /// </summary>
    /// <param name="dataUrl">A "data" URL according to RFC 2397.</param>
    /// <param name="bytes">If the method returns <c>true</c>, the parameter contains the embedded data. 
    /// The parameter is passed uninitialized.</param>
    /// <param name="fileTypeExtension">If the method returns <c>true</c>, the parameter contains an 
    /// appropriate file type extension for <paramref name="bytes"/>. The extension
    /// starts with the period ".". The parameter is passed uninitialized.</param>
    /// <returns><c>true</c> if <paramref name="dataUrl"/> is a valid "data" URL, otherwise <c>false</c>.</returns>
    /// 
    /// <remarks> The method fails only if a decoding error occurs. The use of the method is particularly 
    /// recommended if the embedded data of the "data" URL should be saved in a file.</remarks>
    /// 
    /// <example>
    /// <note type="note">
    /// For the sake of better readability, exception handling is ommitted in the example.
    /// </note>
    /// <para>
    /// Creating and parsing a "data" URL:
    /// </para>
    /// <code language="c#" source="./../Examples/DataUrlExample.cs"/>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetBytes(string? dataUrl,
                                  [NotNullWhen(true)] out byte[]? bytes,
                                  [NotNullWhen(true)] out string? fileTypeExtension)
        => TryGetBytes(dataUrl.AsMemory(), out bytes, out fileTypeExtension);

    /// <summary>
    /// Tries to retrieve the embedded data as a <see cref="byte"/> array, and also an appropriate file 
    /// type extension for this data.
    /// </summary>
    /// <param name="dataUrl">A read-only memory that contains a "data" URL according to RFC 2397.</param>
    ///  <param name="bytes">If the method returns <c>true</c>, the parameter contains the embedded data. 
    /// The parameter is passed uninitialized.</param>
    /// <param name="fileTypeExtension">If the method returns <c>true</c>, the parameter contains an 
    /// appropriate file type extension for <paramref name="bytes"/>. The extension
    /// starts with the period ".". The parameter is passed uninitialized.</param>
    /// 
    /// <returns><c>true</c> if <paramref name="dataUrl"/> is a valid "data" URL, otherwise <c>false</c>.</returns>
    /// 
    /// <remarks> The method fails only if a decoding error occurs. The use of the method is particularly 
    /// recommended if the embedded data of the "data" URL should be saved in a file.</remarks>
    /// 
    public static bool TryGetBytes(ReadOnlyMemory<char> dataUrl,
                                  [NotNullWhen(true)] out byte[]? bytes,
                                  [NotNullWhen(true)] out string? fileTypeExtension)
    {
        if (!DataUrlInfo.TryParseInternal(ref dataUrl, out DataUrlInfo info))
        {
            goto Fail;
        }

        if (info.TryAsBytes(out bytes))
        {
            fileTypeExtension = info.GetFileTypeExtension();
            return true;
        }
    Fail:
        bytes = default;
        fileTypeExtension = null;
        return false;
    }

    /// <summary>
    /// Tries to retrieve the embedded text and the file type extension from the <paramref name="dataUrl"/>.
    /// </summary>
    /// <param name="dataUrl">A "data" URL according to RFC 2397.</param>
    /// <param name="text">If the method returns <c>true</c>, the parameter contains the embedded text. 
    /// The parameter is passed uninitialized.</param>
    /// <param name="fileTypeExtension">If the method returns <c>true</c>, the parameter contains an 
    /// appropriate file type extension for <paramref name="text"/>. The extension starts with the period ".". 
    /// The parameter is passed uninitialized.</param>
    /// 
    /// <returns><c>true</c> if <paramref name="dataUrl"/> is a valid "data" URL that contains embedded text,
    /// otherwise <c>false</c>.</returns>
    /// 
    /// <remarks>
    /// Use this method if you are only interested in a specific text that may be embedded in the "data" URL.
    /// </remarks>
    /// 
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetText(string? dataUrl,
                                  [NotNullWhen(true)] out string? text,
                                  [NotNullWhen(true)] out string? fileTypeExtension)
        => TryGetText(dataUrl.AsMemory(), out text, out fileTypeExtension);

    /// <summary>
    /// Tries to retrieve the embedded text and the file type extension from the <paramref name="dataUrl"/>.
    /// </summary>
    /// <param name="dataUrl">A read-only memory that contains a "data" URL according to RFC 2397.</param>
    /// <param name="text">If the method returns <c>true</c>, the parameter contains the embedded text. 
    /// The parameter is passed uninitialized.</param>
    /// <param name="fileTypeExtension">If the method returns <c>true</c>, the parameter contains an 
    /// appropriate file type extension for <paramref name="text"/>. The extension starts with the period ".". 
    /// The parameter is passed uninitialized.</param>
    /// 
    /// <returns><c>true</c> if <paramref name="dataUrl"/> is a valid "data" URL that contains embedded text,
    /// otherwise <c>false</c>.</returns>
    /// 
    /// <remarks>
    /// Use this method if you are only interested in a specific text that may be embedded in the "data" URL.
    /// </remarks>
    /// 
    public static bool TryGetText(ReadOnlyMemory<char> dataUrl,
                                 [NotNullWhen(true)] out string? text,
                                 [NotNullWhen(true)] out string? fileTypeExtension)
    {
        if (!DataUrlInfo.TryParseInternal(ref dataUrl, out DataUrlInfo info))
        {
            goto Fail;
        }

        if (info.TryAsText(out text))
        {
            fileTypeExtension = info.GetFileTypeExtension();
            return true;
        }
    Fail:
        text = default;
        fileTypeExtension = null;
        return false;
    }

    /// <summary>
    /// Tries to parse a <see cref="string"/> that represents a "data" URL in order to make its content available
    /// as <see cref="DataUrlInfo"/>.
    /// </summary>
    /// 
    /// <param name="value">The <see cref="string"/> to parse.</param>
    /// <param name="info">If the method returns <c>true</c>, the parameter contains a <see cref="DataUrlInfo"/> 
    /// structure that provides the contents of the "data" URL. The parameter is passed uninitialized.</param>
    /// <returns><c>true</c> if <paramref name="value"/> could be parsed as <see cref="DataUrlInfo"/>, otherwise 
    /// <c>false</c>.</returns>
    /// 
    /// <example>
    /// <note type="note">
    /// For the sake of better readability, exception handling is ommitted in the example.
    /// </note>
    /// <para>
    /// Creating and parsing a "data" URL:
    /// </para>
    /// <code language="c#" source="./../Examples/DataUrlExample.cs"/>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParse(string? value, out DataUrlInfo info)
    {
        ReadOnlyMemory<char> mem = value.AsMemory();
        return DataUrlInfo.TryParseInternal(ref mem, out info);
    }

    /// <summary>
    /// Tries to parse a <see cref="ReadOnlyMemory{T}">ReadOnlyMemory&lt;Char&gt;</see> that represents a "data" 
    /// URL in order to make its content available as <see cref="DataUrlInfo"/>.
    /// </summary>
    /// <param name="value">The <see cref="ReadOnlyMemory{T}">ReadOnlyMemory&lt;Char&gt;</see> to parse.</param>
    /// <param name="info">If the method returns <c>true</c>, the parameter contains a <see cref="DataUrlInfo"/> 
    /// structure that provides the contents
    /// of the "data" URL. The parameter is passed uninitialized.</param>
    /// <returns><c>true</c> if <paramref name="value"/> could be parsed as <see cref="DataUrlInfo"/>, 
    /// <c>false</c> otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParse(ReadOnlyMemory<char> value, out DataUrlInfo info)
        => DataUrlInfo.TryParseInternal(ref value, out info);
}
