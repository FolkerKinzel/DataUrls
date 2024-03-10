namespace FolkerKinzel.DataUrls.Extensions;

/// <summary>
/// Extension methods for the <see cref="string"/> class.
/// </summary>
public static class StringExtension
{
    /// <summary>
    /// Returns <c>true</c> if the <see cref="string"/> is a "data" URL. (RFC 2397)
    /// </summary>
    /// <param name="urlString">The <see cref="string"/> to examine.</param>
    /// <returns><c>true</c> if <paramref name="urlString"/> is a "data" URL. If <paramref name="urlString"/> is 
    /// empty or consists only of white space characters <c>false</c> is returned.</returns>
    /// <remarks>Leading white space is skipped.</remarks>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="urlString"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDataUrl([NotNullWhen(true)] this string urlString) =>
        urlString is null ? throw new ArgumentNullException(nameof(urlString)) 
                          : urlString.AsSpan().IsDataUrl();
}
