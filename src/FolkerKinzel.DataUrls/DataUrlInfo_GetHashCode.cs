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
            hash.Add(text);
        }
        else if (TryAsBytes(out byte[]? bytes))
        {
            hash.AddBytes(bytes);
        }
        return hash.ToHashCode();
    }
}
