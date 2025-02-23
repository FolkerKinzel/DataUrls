using FolkerKinzel.DataUrls.Intls;
using FolkerKinzel.DataUrls.Properties;

namespace FolkerKinzel.DataUrls.Extensions;

/// <summary>
/// Extension methods for the <see cref="StringBuilder"/> class.
/// </summary>
public static class StringBuilderExtension
{
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendDataUrl(this StringBuilder builder,
                                              string? text,
                                              string? mimeType = DataUrl.DefaultMediaType,
                                              DataEncoding encoding = DataEncoding.Url)
        => DataUrl.AppendTextTo(builder, text, mimeType, encoding);

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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendDataUrl(this StringBuilder builder,
                                              string? text,
                                              MimeType mimeType,
                                              DataEncoding encoding = DataEncoding.Url)
        => DataUrl.AppendTextTo(builder, text, mimeType, encoding);
        

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
    public static StringBuilder AppendDataUrl(this StringBuilder builder,
                                              byte[]? bytes,
                                              string? mimeType = MimeString.OctetStream,
                                              DataEncoding encoding = DataEncoding.Base64)
        => DataUrl.AppendBytesTo(builder, bytes.AsSpan(), mimeType, encoding);

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
    public static StringBuilder AppendDataUrl(this StringBuilder builder,
                                              IEnumerable<byte>? bytes,
                                              string? mimeType = MimeString.OctetStream,
                                              DataEncoding encoding = DataEncoding.Base64)
        => DataUrl.AppendBytesTo(builder, bytes.AsSpan(), mimeType, encoding);

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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendDataUrl(this StringBuilder builder,
                                              ReadOnlySpan<byte> bytes,
                                              string? mimeType = MimeString.OctetStream,
                                              DataEncoding encoding = DataEncoding.Base64)
        => DataUrl.AppendBytesTo(builder, bytes, mimeType, encoding);

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
    public static StringBuilder AppendDataUrl(this StringBuilder builder,
                                              byte[]? bytes,
                                              in MimeTypeInfo mimeType,
                                              DataEncoding encoding = DataEncoding.Base64)
        => DataUrl.AppendBytesTo(builder, bytes.AsSpan(), in mimeType, encoding);

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
    public static StringBuilder AppendDataUrl(this StringBuilder builder,
                                              IEnumerable<byte>? bytes,
                                              in MimeTypeInfo mimeType,
                                              DataEncoding encoding = DataEncoding.Base64)
        => DataUrl.AppendBytesTo(builder, bytes.AsSpan(), in mimeType, encoding);

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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendDataUrl(this StringBuilder builder,
                                              ReadOnlySpan<byte> bytes,
                                              in MimeTypeInfo mimeType,
                                              DataEncoding encoding = DataEncoding.Base64)
        => DataUrl.AppendBytesTo(builder, bytes, in mimeType, encoding);

    /// <summary>
    /// Appends the content of a file as "data" URL (RFC 2397) to the end of a <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to which a "data" URL is appended.</param>
    /// <param name="filePath">Abolute path to the file whose content is to embed in the "data" URL.</param>
    /// <param name="mimeType">The Internet Media Type ("MIME type") of the file whose content is to embed, or 
    /// <c>null</c> to let the method automatically retrieve the <see cref="MimeType"/> from the file type 
    /// extension.</param>
    /// <param name="encoding">The encoding to use to embed the file content.</param>
    /// 
    /// <returns>A reference to <paramref name="builder"/>.</returns>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> or <paramref name="filePath"/> is 
    /// <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="filePath"/> is not a valid file path.</exception>
    /// <exception cref="IOException">I/O error.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendFileAsDataUrl(this StringBuilder builder,
                                                    string filePath,
                                                    string? mimeType = null,
                                                    DataEncoding encoding = DataEncoding.Base64)
        => DataUrl.AppendFileTo(builder, filePath, mimeType, encoding);

    /// <summary>
    /// Appends the content of a file as "data" URL (RFC 2397) to the end of a <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to which a "data" URL is appended.</param>
    /// <param name="filePath">Abolute path to the file whose content is to embed in the "data" URL.</param>
    /// <param name="mimeType">The Internet Media Type ("MIME type") of the file whose content is to embed.
    /// </param>
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendFileAsDataUrl(this StringBuilder builder,
                                                    string filePath,
                                                    in MimeTypeInfo mimeType,
                                                    DataEncoding encoding = DataEncoding.Base64)
        => DataUrl.AppendFileTo(builder, filePath, in mimeType, encoding);
}
