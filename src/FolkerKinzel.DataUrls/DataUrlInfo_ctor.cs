namespace FolkerKinzel.DataUrls;

/// <summary>
/// Provides the information stored in a "data" URL (RFC 2397).
/// </summary>
/// <remarks>
/// <note type="tip">
/// <para>
/// <see cref="DataUrlInfo"/> is a quite large structure. Pass it to other methods by reference 
/// (in, ref or out parameters in C#)!
/// </para>
/// <para>
/// If you intend to hold a <see cref="DataUrlInfo"/> for a long time in memory and if this 
/// <see cref="DataUrlInfo"/> is parsed from a <see cref="ReadOnlyMemory{T}">ReadOnlyMemory&lt;Char&gt;</see>
/// that comes from a very long <see cref="string"/>, keep in mind, that the <see cref="DataUrlInfo"/> holds 
/// a reference to that <see cref="string"/>. Consider in this case to make a copy of the 
/// <see cref="DataUrlInfo"/> structure with <see cref="DataUrlInfo.Clone"/>: The copy is built on a separate
/// <see cref="string"/> that is case-normalized and only as long as needed.
/// </para>
/// </note>
/// </remarks>
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
public readonly partial struct DataUrlInfo
{
    private DataUrlInfo(ushort idx, in ReadOnlyMemory<char> dataUrl)
    {
        _dataUrl = dataUrl;
        _idx = idx;
    }
}
