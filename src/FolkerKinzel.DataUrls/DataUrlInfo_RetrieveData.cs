using FolkerKinzel.DataUrls.Intls;

namespace FolkerKinzel.DataUrls;

public readonly partial struct DataUrlInfo
{
    private const int ASCII_CODEPAGE = 20127;

    /// <summary>
    /// Tries to retrieve the text, which is embedded in the "data" URL.
    /// </summary>
    /// <param name="embeddedText">If the method returns <c>true</c> the parameter contains 
    /// the embedded text.
    /// The parameter is passed uninitialized.</param>
    /// <returns><c>true</c> if the embedded <see cref="Data"/> in the "data" URL could 
    /// be parsed as text, otherwise <c>false</c>.</returns>
    /// <remarks>It depends of the <see cref="MimeType"/> whether the <see cref="Data"/> 
    /// in the "data" URL is treated as embedded text. The <see cref="MimeType"/>
    /// should rather be empty or have the Top-Level Media Type "text".</remarks>
    public bool TryGetEmbeddedText([NotNullWhen(true)] out string? embeddedText)
    {
        embeddedText = null;

        if (!ContainsEmbeddedText)
        {
            return false;
        }

        // als Base64 codierter Text:
        if (DataEncoding == DataEncoding.Base64)
        {
            if (!Base64Helper.TryDecode(Data, out byte[]? data))
            {
                return false;
            }

            int bomLength = GetEncoding(data, out Encoding enc);

            try
            {
                embeddedText = enc.GetString(data, bomLength, data.Length - bomLength);
                return true;
            }
            catch
            {
                return false;
            }
        }
        else
        {
            // URL encoded String:
            string? encodingName = TryGetEncodingFromMimeType(out encodingName) ? encodingName
                                                                                : DataUrlBuilder.UTF_8;
            return UrlEncoding.TryDecode(Data, encodingName, true, out embeddedText);
        }
    }

    /// <summary>
    /// Tries to retrieve the binary <see cref="Data"/>, which is embedded in the "data" URL.
    /// </summary>
    /// <param name="embeddedBytes">If the method returns <c>true</c> the parameter contains 
    /// the embedded binary data.
    /// The parameter is passed uninitialized.</param>
    /// <returns><c>true</c> if the <see cref="Data"/> embedded in the "data" URL could be 
    /// parsed as binary data, otherwise <c>false</c>.</returns>
    /// <remarks>It depends of the <see cref="MimeType"/> whether the embedded <see cref="Data"/> 
    /// in the "data" URL is treated as binary data.</remarks>
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
    public bool TryGetEmbeddedBytes([NotNullWhen(true)] out byte[]? embeddedBytes)
    {
        embeddedBytes = null;

        return ContainsEmbeddedBytes &&
               (this.DataEncoding == DataEncoding.Base64
                    ? Base64Helper.TryDecode(Data, out embeddedBytes)
                    : UrlEncoding.TryDecodeToBytes(Data, true, out embeddedBytes));
    }

    /// <summary>
    /// Tries to retrieve the embedded <see cref="Data"/> decoded either as a <see cref="string"/> 
    /// or as a byte array, depending on <see cref="MimeType"/>.
    /// </summary>
    /// <param name="data">The embedded <see cref="Data"/>. This can be either a <see cref="string"/> 
    /// or a byte array. The parameter is passed uninitialized.</param>
    /// <returns><c>true</c> if <see cref="Data"/> could be converted either into a <see cref="string"/>
    /// or a byte array.</returns>
    public bool TryGetEmbeddedData(out OneOf<string, byte[]> data)
    {
        if (TryGetEmbeddedText(out string? embeddedText))
        {
            data = embeddedText;
            return true;
        }

        if (TryGetEmbeddedBytes(out byte[]? embeddedBytes))
        {
            data = embeddedBytes;
            return true;
        }

        data = default;
        return false;
    }

    /// <summary>
    /// Returns an appropriate file type extension for the <see cref="Data"/> embedded in the 
    /// "data" URL. The file type extension contains the period (".").
    /// </summary>
    /// <returns>An appropriate file type extension for the embedded <see cref="Data"/>. The extension starts with the period ".".</returns>
    /// 
    ///<example>
    /// <note type="note">
    /// For the sake of better readability, exception handling is ommitted in the example.
    /// </note>
    /// <para>
    /// Creating and parsing a "data" URL:
    /// </para>
    /// <code language="c#" source="./../Examples/DataUrlExample.cs"/>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetFileTypeExtension() => MimeString.ToFileTypeExtension(MimeType.Span);

    private int GetEncoding(byte[] data, out Encoding enc)
    {
        int codePage = TextEncodingConverter.GetCodePage(data, out int bomLength);

        enc = TryGetEncodingFromMimeType(out string? charsetName)
                   ? TextEncoding.InitThrowingEncoding(charsetName)
                   : TextEncoding.InitThrowingEncoding(codePage);

        return bomLength;
    }

    private bool TryGetEncodingFromMimeType([NotNullWhen(true)] out string? encodingName)
    {
        if (!MimeTypeInfo.TryParse(MimeType, out MimeTypeInfo info))
        {
            encodingName = null;
            return false;
        }

        MimeTypeParameterInfo charsetPara = info.Parameters().FirstOrDefault(Predicate);
        if (charsetPara.IsEmpty)
        {
            encodingName = null;
            return false;
        }
        encodingName = charsetPara.Value.ToString();
        return true;

        ///////////////////////////////////////////////////////////

        static bool Predicate(MimeTypeParameterInfo p) => p.IsCharSetParameter;
    }
}
