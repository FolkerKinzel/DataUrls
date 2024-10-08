﻿using System.Runtime.InteropServices;

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

    private readonly ReadOnlyMemory<char> _embeddedData;
    private readonly ushort _idx;

    private int MimeTypeLength => _idx >> MIME_TYPE_LENGTH_SHIFT;

    private bool IncompleteMimeType => (_idx & INCOMPLETE_MIME_TYPE_VALUE) == INCOMPLETE_MIME_TYPE_VALUE;

    private int EmbeddedDataStartIndex => IsEmpty ? 0
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
                                    ? (DataUrl.DefaultMediaType + _embeddedData.Span.Slice(0, MimeTypeLength).ToString()).AsMemory()
                                    : _embeddedData.Slice(0, MimeTypeLength);

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
    /// either a <see cref="string"/> or a collection of bytes. (See <see cref="ContainsEmbeddedText"/> and
    /// <see cref="ContainsEmbeddedBytes"/>.)
    /// </remarks>
    public ReadOnlySpan<char> Data => _embeddedData.Span.Slice(EmbeddedDataStartIndex);

    /// <summary>
    /// Indicates whether <see cref="Data"/> represents text.
    /// </summary>
    /// <value>
    /// <c>true</c> if <see cref="Data"/> contains text, otherwise <c>false</c>.
    /// </value>
    /// <example>
    /// <note type="note">
    /// For the sake of better readability, exception handling is ommitted in the example.
    /// </note>
    /// <para>
    /// Creating and parsing a "data" URL:
    /// </para>
    /// <code language="c#" source="./../Examples/DataUrlExample.cs"/>
    /// </example>
    public bool ContainsEmbeddedText => MimeTypeLength == 0 || IncompleteMimeType || _embeddedData.Span.StartsWith("text/".AsSpan(), StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Indicates whether <see cref="Data"/> represents binary data.
    /// </summary>
    /// <value>
    /// <c>true</c> if <see cref="Data"/> contains binary data, otherwise <c>false</c>.
    /// </value>
    /// <remarks>
    /// This property has not an either/or relation to <see cref="ContainsEmbeddedText"/>: <see cref="Data"/>
    /// could represent binary encoded text.
    /// </remarks>
    /// <example>
    /// <note type="note">
    /// For the sake of better readability, exception handling is ommitted in the example.
    /// </note>
    /// <para>
    /// Creating and parsing a "data" URL:
    /// </para>
    /// <code language="c#" source="./../Examples/DataUrlExample.cs"/>
    /// </example>
    public bool ContainsEmbeddedBytes => DataEncoding == DataEncoding.Base64 || !ContainsEmbeddedText;

    /// <summary>
    /// Indicates whether the instance contains no data.
    /// </summary>
    /// <value>
    /// <c>true</c> if the instance contains no data, otherwise <c>false</c>.
    /// </value>
    public bool IsEmpty => _embeddedData.IsEmpty;

    /// <summary>
    /// Returns an empty <see cref="DataUrlInfo"/> instance.
    /// </summary>
    public static DataUrlInfo Empty => default;
}

#pragma warning restore CA1303 // // literals not as localized parameters