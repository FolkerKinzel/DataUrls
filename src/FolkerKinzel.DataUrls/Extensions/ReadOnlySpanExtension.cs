namespace FolkerKinzel.DataUrls.Extensions;

/// <summary>
/// Extension methods for the <see cref="ReadOnlySpan{T}">ReadOnlySpan&lt;Char&gt;</see> structure.
/// </summary>
public static class ReadOnlySpanExtension
{
    /// <summary>
    /// Returns <c>true</c> if the <see cref="ReadOnlySpan{T}">ReadOnlySpan&lt;Char&gt;</see> is a "data" URL. (RFC 2397)
    /// </summary>
    /// <param name="span">The <see cref="ReadOnlySpan{T}">ReadOnlySpan&lt;Char&gt;</see> to examine.</param>
    /// <returns><c>true</c> if <paramref name="span"/> contains a "data" URL.</returns>
    /// <remarks>Leading white space is skipped.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Globalization", "CA1303:Literale nicht als lokalisierte Parameter übergeben", Justification = "<Ausstehend>")]
    public static bool IsDataUrl(this ReadOnlySpan<char> span)
        => span.TrimStart().StartsWith(DataUrl.Scheme.AsSpan(), StringComparison.OrdinalIgnoreCase);

}
