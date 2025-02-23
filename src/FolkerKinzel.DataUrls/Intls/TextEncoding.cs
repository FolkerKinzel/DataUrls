namespace FolkerKinzel.DataUrls.Intls;

internal static class TextEncoding
{
    internal static Encoding InitThrowingEncoding(int codePage)
        => TextEncodingConverter.GetEncoding(codePage,
                                             EncoderFallback.ExceptionFallback,
                                             DecoderFallback.ExceptionFallback,
                                             true);

    internal static Encoding InitThrowingEncoding(string encodingName)
        => TextEncodingConverter.GetEncoding(encodingName,
                                             EncoderFallback.ExceptionFallback,
                                             DecoderFallback.ExceptionFallback,
                                             true);
}
