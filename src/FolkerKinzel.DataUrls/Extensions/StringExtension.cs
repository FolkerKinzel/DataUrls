namespace FolkerKinzel.DataUrls.Extensions;

/// <summary>
/// Extension methods for the <see cref="string"/> class.
/// </summary>
public static class StringExtension
{
    /// <summary>
    /// Returns <c>true</c> if the <see cref="string"/> is a "data" URL. (RFC 2397)
    /// </summary>
    /// <param name="urlString">The <see cref="string"/> to examine, or <c>null</c>.</param>
    /// <returns><c>true</c> if <paramref name="urlString"/> is a "data" URL. If 
    /// <paramref name="urlString"/> is <c>null</c>, <see cref="string.Empty"/>, or consists
    /// only of white space characters, <c>false</c> is returned.</returns>
    /// <remarks>Leading white space is skipped.</remarks>
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
    public static bool IsDataUrl([NotNullWhen(true)] this string? urlString)
        => urlString.AsSpan().IsDataUrl();
}
