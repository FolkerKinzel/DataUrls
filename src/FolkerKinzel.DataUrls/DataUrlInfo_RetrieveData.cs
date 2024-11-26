using System.Text;
using FolkerKinzel.DataUrls.Intls;
using static System.Net.Mime.MediaTypeNames;

namespace FolkerKinzel.DataUrls;

public readonly partial struct DataUrlInfo
{
    private const int ASCII_CODEPAGE = 20127;

    /// <summary>
    /// Tries to retrieve the text, which is embedded in the "data" URL.
    /// </summary>
    /// <param name="text">If the method returns <c>true</c> the parameter contains 
    /// the embedded text. The parameter is passed uninitialized.</param>
    /// <returns><c>true</c> if the embedded <see cref="Data"/> in the "data" URL could 
    /// be parsed as <see cref="string"/>, otherwise <c>false</c>.</returns>
    /// <remarks>
    /// <para>
    /// The method succeeds only if the 
    /// <see cref="DataType"/> property returns <see cref="DataType.Text"/>.
    /// </para>
    /// <para>
    /// Use this method if you are only interested in a specific text that may be
    /// embedded in the "data" URL.
    /// </para>
    /// </remarks>
    /// 
    public bool TryAsText([NotNullWhen(true)] out string? text)
    {
        text = null;

        if (DataType != DataType.Text)
        {
            return false;
        }

        // base64-encoded text:
        if (Encoding == DataEncoding.Base64)
        {
            if (!Base64Helper.TryDecode(Data, out byte[]? data))
            {
                return false;
            }

            int bomLength = GetEncoding(data, out Encoding enc);

            try
            {
                text = enc.GetString(data, bomLength, data.Length - bomLength);
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
            return UrlEncoding.TryDecode(Data, encodingName, true, out text);
        }
    }

    /// <summary>
    /// Tries to retrieve the <see cref="Data"/>, which is embedded in the "data" URL, as a 
    /// <see cref="byte"/> array.
    /// </summary>
    /// <param name="bytes">If the method returns <c>true</c> the parameter contains 
    /// the embedded binary data. The parameter is passed uninitialized.</param>
    /// <returns><c>true</c> if the <see cref="Data"/> embedded in the "data" URL could be 
    /// parsed as a <see cref="byte"/> array, otherwise <c>false</c>.</returns>
    /// <remarks> The method fails only if a decoding error occurs. The use of the method is particularly 
    /// recommended if the data of the "data" URL should be saved in a file.</remarks>
    public bool TryAsBytes([NotNullWhen(true)] out byte[]? bytes)
        => this.Encoding == DataEncoding.Base64
                    ? Base64Helper.TryDecode(Data, out bytes)
                    : UrlEncoding.TryDecodeToBytes(Data, true, out bytes);

    /// <summary>
    /// Tries to retrieve the embedded <see cref="Data"/> decoded either as a <see cref="string"/> 
    /// or as a <see cref="byte"/> array, depending on the value of the <see cref="DataType"/> property.
    /// </summary>
    /// <param name="data">If the method returns <c>true</c> the parameter contains the embedded
    /// <see cref="Data"/> as an <see cref="EmbeddedData"/> union. The 
    /// parameter is passed uninitialized.</param>
    /// <returns><c>true</c> if <see cref="Data"/> could be converted to an <see cref="EmbeddedData"/> 
    /// instance, otherwise <c>false</c>.</returns>
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
    public bool TryGetData(out EmbeddedData data)
    {
        if (TryAsText(out string? embeddedText))
        {
            data = EmbeddedData.FromText(embeddedText);
            return true;
        }

        if (TryAsBytes(out byte[]? embeddedBytes))
        {
            data = EmbeddedData.FromBytes(embeddedBytes);
            return true;
        }

        data = default;
        return false;
    }

    /// <summary>
    /// Returns an appropriate file type extension for the <see cref="Data"/> embedded in the 
    /// "data" URL. The file type extension contains the period (".").
    /// </summary>
    /// <returns>An appropriate file type extension for the embedded <see cref="Data"/>. The 
    /// extension starts with the period ".".</returns>
    /// <remarks>
    /// The returned file name extension depends on the value of the <see cref="MimeType"/> property.
    /// </remarks>
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
