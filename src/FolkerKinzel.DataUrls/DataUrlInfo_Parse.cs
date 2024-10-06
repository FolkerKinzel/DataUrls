using FolkerKinzel.DataUrls.Extensions;

namespace FolkerKinzel.DataUrls;

public readonly partial struct DataUrlInfo
{
    [SuppressMessage("Globalization", "CA1303:Literale nicht als lokalisierte Parameter übergeben", Justification = "<Ausstehend>")]
    internal static bool TryParseInternal(ref ReadOnlyMemory<char> value, out DataUrlInfo info)
    {
        value = value.Trim();
        info = default;

        if (!value.Span.IsDataUrl())
        {
            return false;
        }

        value = value.Slice(DataUrl.Scheme.Length);
        ReadOnlySpan<char> span = value.Span;
        int mimeTypeLength = span.IndexOf(',');

        if (mimeTypeLength == -1) // missing ','
        {
            return false;
        }

        DataEncoding dataEncoding =
            span.Slice(0, mimeTypeLength)
                .EndsWith(DataUrl.Base64.AsSpan(), StringComparison.OrdinalIgnoreCase)
                      ? DataEncoding.Base64
                      : DataEncoding.Url;

        if (dataEncoding == DataEncoding.Base64)
        {
            mimeTypeLength -= DataUrl.Base64.Length;
        }

        // if text/plain is omitted and only the parameters are provided:
        ushort hasIncompleteMimeType = mimeTypeLength > 0 && span.StartsWith(';') ? INCOMPLETE_MIME_TYPE_VALUE : (ushort)0;

        if (mimeTypeLength > MIME_TYPE_LENGTH_MAX_VALUE)
        {
            return false;
        }

        ushort idx = (ushort)(mimeTypeLength << MIME_TYPE_LENGTH_SHIFT);
        idx |= hasIncompleteMimeType;
        idx |= (ushort)dataEncoding;

        info = new DataUrlInfo(idx, in value);

        return true;
    }
}
