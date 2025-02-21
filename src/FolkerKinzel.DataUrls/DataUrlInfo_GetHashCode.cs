using FolkerKinzel.DataUrls.Intls;

namespace FolkerKinzel.DataUrls;

public readonly partial struct DataUrlInfo
{
    /// <summary>
    /// Creates a hash code for this instance.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(GetFileTypeExtension());

        if (TryAsText(out string? text))
        {
            hash.Add(text, StringComparer.Ordinal);
        }
        else if (TryAsBytes(out byte[]? bytes))
        {
#if NET462 || NETSTANDARD2_0 || NETSTANDARD2_1
            ReadOnlySpan<byte> span = bytes;

            for (int i = 0; i < span.Length; i++)
            {
                hash.Add(span[i]);
            }
#else
            hash.AddBytes(bytes);
#endif
        }

        return hash.ToHashCode();
    }
}
