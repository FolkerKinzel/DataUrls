using FolkerKinzel.Helpers;

namespace FolkerKinzel.DataUrls.Intls;

/// <summary>
/// Provides functionality to build a "data" URL (RFC 2397) that 
/// embeds data in-line in a URL. A "data" URL can be created automatically from 
/// the data to embed. This can be a file, a byte array or a <see cref="string"/>. 
/// </summary>
internal static class DataUrlBuilder
{
    internal const int ESTIMATED_MIME_TYPE_LENGTH = 80;
    internal const string UTF_8 = "utf-8";
    private const int COMMA_LENGTH = 1;
    private const string CHARSET = "charset";

    /// <summary>
    /// Appends embedded text as "data" URL (RFC 2397) to the end of a <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to which a "data" URL is appended.</param>
    /// <param name="text">The text to embed into the "data" URL. <paramref name="text"/> MUST not be 
    /// URL-encoded.</param>
    /// <param name="mimeType">The <see cref="MimeType"/> of the <paramref name="text"/>.</param>
    /// <param name="dataEncoding">The encoding to use to embed the <paramref name="text"/>.</param>
    /// 
    /// <returns>A reference to <paramref name="builder"/>.</returns>
    /// <exception cref="FormatException"><paramref name="text"/> could not URL-encoded.</exception>
    internal static StringBuilder AppendEmbeddedTextInternal(this StringBuilder builder,
                                                             string? text,
                                                             MimeType mimeType,
                                                             DataEncoding dataEncoding)
    {
        Debug.Assert(builder != null);
        Debug.Assert(mimeType != null);

        text ??= string.Empty;

        string charSet = InitCharSet(text, mimeType);

        byte[] value = SerializeText(text, mimeType, charSet);

        return builder.AppendEmbeddedBytesIntl(value, mimeType.AsInfo(), dataEncoding);

        ////////////////////////////////////////////////////////////////////////////

        static bool IsCharSetParameter(MimeTypeParameter parameter) => parameter.IsCharSetParameter;

        static byte[] SerializeText(string text, MimeType mimeType, string charSet)
        {
            byte[] value;

            if(TextEncodingConverter.TryGetEncoding(charSet, out Encoding? enc))
            {
                value = enc.GetBytes(text);
            }
            else
            {
                value = Encoding.UTF8.GetBytes(text);

                AssertMimeTypeHasCharSetParameter(mimeType);
                mimeType.AppendParameter(CHARSET, UTF_8);
            }

            return value;

            ///////////////////////////////////////////////////////////

            [ExcludeFromCodeCoverage]
            [Conditional("DEBUG")]
            static void AssertMimeTypeHasCharSetParameter(MimeType mimeType)
                => Debug.Assert(mimeType.Parameters.Any(IsCharSetParameter));
        }

        static string InitCharSet(string text, MimeType mimeType)
        {
            if(mimeType.IsTextPlain && text.IsAscii())
            {
                mimeType.RemoveParameter(CHARSET);
                return UTF_8;
            }

            MimeTypeParameter? charSetParameter = mimeType.Parameters.FirstOrDefault(IsCharSetParameter);

            string charSet;
            if (charSetParameter?.Value is null)
            {
                charSet = UTF_8;

                if (mimeType.IsTextPlain)
                {
                    mimeType.AppendParameter(CHARSET, UTF_8);
                }
            }
            else
            {
                charSet = charSetParameter.Value;
            }

            return charSet;
        }
    }

    /// <summary>
    /// Appends binary data as "data" URL (RFC 2397) to the end of a <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to which a "data" URL is appended.</param>
    /// <param name="bytes">The binary data to embed into the "data" URL.</param>
    /// <param name="mimeType">The <see cref="MimeType"/> of the <paramref name="bytes"/>.</param>
    /// <param name="dataEncoding">The encoding to use to embed the <paramref name="bytes"/>.</param>
    /// <returns>A reference to <paramref name="builder"/>.</returns>
    internal static StringBuilder AppendEmbeddedBytesIntl(this StringBuilder builder,
                                                              ReadOnlySpan<byte> bytes,
                                                              in MimeTypeInfo mimeType,
                                                              DataEncoding dataEncoding) 
        =>  dataEncoding == DataEncoding.Base64 ? builder.AppendEmbeddedBytesBase64Encoded(bytes, in mimeType)
                                                : builder.AppendEmbeddedBytesUrlEncoded(bytes, in mimeType);

    private static StringBuilder AppendEmbeddedBytesUrlEncoded(this StringBuilder builder,
                                                               ReadOnlySpan<byte> bytes,
                                                               in MimeTypeInfo mimeType)
    {
        Debug.Assert(builder != null);

        _ = builder.EnsureCapacity(builder.Length
                                   + DataUrl.Scheme.Length
                                   + ESTIMATED_MIME_TYPE_LENGTH
                                   + COMMA_LENGTH
                                   + (int)(bytes.Length * UrlEncoding.EncodedLengthFactor));

        return builder.Append(DataUrl.Scheme).AppendMediaType(in mimeType).Append(',').AppendUrlEncoded(bytes);

        // $"data:{mediaTypeString},{UrlEncoding.EncodeBytes(bytes)}"
    }

    private static StringBuilder AppendEmbeddedBytesBase64Encoded(this StringBuilder builder,
                                                                  ReadOnlySpan<byte> bytes,
                                                                  in MimeTypeInfo mimeType)
    {
        Debug.Assert(builder != null);

        _ = builder.EnsureCapacity(builder.Length
                                   + DataUrl.Scheme.Length
                                   + ESTIMATED_MIME_TYPE_LENGTH
                                   + DataUrl.BASE_64.Length
                                   + COMMA_LENGTH
                                   + Base64.GetEncodedLength(bytes.Length));

        return builder
            .Append(DataUrl.Scheme).AppendMediaType(in mimeType).Append(DataUrl.BASE_64).Append(',').AppendBase64(bytes);

        // $"data:{mediaTypeString};base64,{Convert.ToBase64String(bytes)}"
    }

    /// <summary>
    /// Appends the content of a file as "data" URL (RFC 2397) to the end of a <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to which a "data" URL is appended.</param>
    /// <param name="filePath">Abolute path to the file whose content is to embed in the "data" URL.</param>
    /// <param name="mimeType">The <see cref="MimeType"/> of the file whose content is to embed.</param>
    /// <param name="dataEncoding">The encoding to use to embed the file.</param>
    /// 
    /// <returns>A reference to <paramref name="builder"/>.</returns>
    /// 
    /// <exception cref="ArgumentException"><paramref name="filePath"/> is not a valid file path.</exception>
    /// <exception cref="IOException">I/O error.</exception>
    internal static StringBuilder AppendFileContentInternal(this StringBuilder builder,
                                                            string filePath,
                                                            in MimeTypeInfo mimeType,
                                                            DataEncoding dataEncoding)
    {
        Debug.Assert(builder != null);
        Debug.Assert(filePath != null);

        return builder.AppendEmbeddedBytesIntl(BinaryFile.Load(filePath), in mimeType, dataEncoding);
    }
}
