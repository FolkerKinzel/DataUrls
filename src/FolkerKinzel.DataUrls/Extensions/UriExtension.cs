namespace FolkerKinzel.DataUrls.Extensions;

/// <summary>
/// Extension methods for the <see cref="Uri"/> class.
/// </summary>
public static class UriExtension
{
    /// <summary>
    /// Returns <c>true</c> if the <see cref="Uri"/> is a "data" URL. (RFC 2397)
    /// </summary>
    /// <param name="uri">The <see cref="Uri"/> to examine.</param>
    /// <returns><c>true</c> if <paramref name="uri"/> is a "data" URL.</returns>
    /// <remarks>Leading white space is skipped.</remarks>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="uri"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDataUrl([NotNullWhen(true)] this Uri uri) => 
        uri?.OriginalString.AsSpan().IsDataUrl() ?? throw new ArgumentNullException(nameof(uri));
}
