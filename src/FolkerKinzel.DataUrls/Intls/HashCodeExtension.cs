namespace FolkerKinzel.DataUrls.Intls;

#if NET461 || NETSTANDARD2_0 || NETSTANDARD2_1 || NET5_0

internal static class HashCodeExtension
{
    internal static void AddBytes(this HashCode hash, ReadOnlySpan<byte> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            hash.Add(value[i]);
        }
    }
}

#endif
