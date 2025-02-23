namespace FolkerKinzel.DataUrls.Extensions;

/// <summary>
/// Extension methods for the <see cref="ReadOnlySpan{T}">ReadOnlySpan&lt;Char&gt;</see> 
/// structure.</summary>
public static class ReadOnlySpanExtension
{
    /// <summary>
    /// Determines whether the <see cref="ReadOnlySpan{T}">ReadOnlySpan&lt;Char&gt;</see>
    /// contains a "data" URL (RFC 2397).
    /// </summary>
    /// <param name="span">The <see cref="ReadOnlySpan{T}">ReadOnlySpan&lt;Char&gt;</see> to
    /// examine.</param>
    /// <returns><c>true</c> if <paramref name="span"/> contains a "data" URL; otherwise, 
    /// <c>false</c>.</returns>
    /// <remarks>Leading white space is skipped.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDataUrl(this ReadOnlySpan<char> span)
        => span.TrimStart().StartsWith(DataUrl.Scheme.AsSpan(), StringComparison.OrdinalIgnoreCase);
}
