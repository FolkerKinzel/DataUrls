using System.Runtime.InteropServices;

namespace FolkerKinzel.DataUrls;

#pragma warning disable CA1303 // literals not as localized parameters

[StructLayout(LayoutKind.Auto)]
public readonly partial struct DataUrlInfo
{
    private const int MIME_TYPE_LENGTH_SHIFT = 2;
    private const ushort DATA_ENCODING_MAX_VALUE = 1;
    private const int COMMA_LENGTH = 1;
    private const ushort MIME_TYPE_LENGTH_MAX_VALUE = 0b0011_1111_1111_1111;
    private const ushort INCOMPLETE_MIME_TYPE_VALUE = 2;

    private readonly ReadOnlyMemory<char> _dataUrl;
    private readonly ushort _idx;

    private int MimeTypeLength => _idx >> MIME_TYPE_LENGTH_SHIFT;

    private bool IncompleteMimeType => (_idx & INCOMPLETE_MIME_TYPE_VALUE) == INCOMPLETE_MIME_TYPE_VALUE;

    private int DataStartIndex => IsEmpty ? 0
                                  : MimeTypeLength
                                    + (DataEncoding == DataEncoding.Base64 ? DataUrl.Base64.Length : 0)
                                    + COMMA_LENGTH;

    /// <summary>
    /// Internet Media Type of the embedded <see cref="Data"/>.
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
    public ReadOnlyMemory<char> MimeType =>
        MimeTypeLength == 0 ? DataUrl.DefaultMediaType.AsMemory()
                            : IncompleteMimeType
                                    ? (DataUrl.DefaultMediaType + _dataUrl.Span.Slice(0, MimeTypeLength).ToString()).AsMemory()
                                    : _dataUrl.Slice(0, MimeTypeLength);

    /// <summary>
    /// The encoding of <see cref="Data"/>.
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
    public DataEncoding DataEncoding => (DataEncoding)(_idx & DATA_ENCODING_MAX_VALUE);

    /// <summary>
    /// The part of the "data" URL, which contains the embedded data.
    /// </summary>
    /// <remarks>
    /// This part is either URL or Base64 encoded (see <see cref="DataEncoding"/>) and represents
    /// either a <see cref="string"/> or an array of <see cref="byte"/>s.
    /// </remarks>
    /// <seealso cref="DataType"/>
    public ReadOnlySpan<char> Data => _dataUrl.Span.Slice(DataStartIndex);

    /// <summary>
    /// Gets the data type of the embedded data.
    /// </summary>
    /// <remarks>
    /// The return value depends on the <see cref="MimeType"/>.
    /// </remarks>
    public DataType DataType 
        => MimeTypeLength == 0
        || IncompleteMimeType 
        || _dataUrl.Span.StartsWith("text/", StringComparison.OrdinalIgnoreCase) 
            ? DataType.Text 
            : DataType.Binary;

    /// <summary>
    /// Indicates whether the instance contains no data.
    /// </summary>
    /// <value>
    /// <c>true</c> if the instance contains no data, otherwise <c>false</c>.
    /// </value>
    public bool IsEmpty => _dataUrl.IsEmpty;

    /// <summary>
    /// Returns an empty <see cref="DataUrlInfo"/> instance.
    /// </summary>
    public static DataUrlInfo Empty => default;
}

#pragma warning restore CA1303 // // literals not as localized parameters