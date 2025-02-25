using FolkerKinzel.DataUrls.Extensions;

namespace FolkerKinzel.DataUrls;

public readonly partial struct DataUrlInfo
{
    internal static bool TryParseInternal(ref ReadOnlyMemory<char> value,
                                          out DataUrlInfo info)
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
                .EndsWith(DataUrl.BASE_64.AsSpan(), StringComparison.OrdinalIgnoreCase)
                      ? DataEncoding.Base64
                      : DataEncoding.Url;

        if (dataEncoding == DataEncoding.Base64)
        {
            mimeTypeLength -= DataUrl.BASE_64.Length;
        }

        // if text/plain is omitted and only the parameters are provided:
        ushort hasIncompleteMimeType = mimeTypeLength > 0 && span.StartsWith(';')
                                         ? INCOMPLETE_MIME_TYPE_VALUE
                                         : (ushort)0;

        DataType dataType = mimeTypeLength == 0 ||
                          hasIncompleteMimeType == INCOMPLETE_MIME_TYPE_VALUE
                          || span.StartsWith("text/", StringComparison.OrdinalIgnoreCase)
                          ? DataType.Text
                          : DataType.Binary;

        if (mimeTypeLength > MIME_TYPE_LENGTH_MAX_VALUE)
        {
            return false;
        }

        ushort idx = (ushort)(mimeTypeLength << MIME_TYPE_LENGTH_SHIFT);
        idx |= hasIncompleteMimeType;
        idx |= (ushort)dataEncoding;
        idx |= (ushort)dataType;

        info = new DataUrlInfo(idx, in value);

        return true;
    }
}
