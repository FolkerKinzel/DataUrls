namespace FolkerKinzel.DataUrls.Intls;

internal static class Base64Helper
{
    internal static bool TryDecode(ReadOnlySpan<char> base64, [NotNullWhen(true)] out byte[]? decoded)
    {
        try
        {
            decoded = Base64.GetBytes(
                base64,
                Base64ParserOptions.AcceptMissingPadding | Base64ParserOptions.AcceptBase64Url);
            return true;
        }
        catch
        {
            decoded = null;
            return false;
        }
    }

}
