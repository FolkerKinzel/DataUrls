using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.DataUrls.Intls;
internal static class TextEncoding
{
    internal static Encoding InitThrowingEncoding(int codePage) =>
        TextEncodingConverter.GetEncoding(codePage, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback, true);

    internal static Encoding InitThrowingEncoding(string encodingName) =>
        TextEncodingConverter.GetEncoding(encodingName, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback, true);
}
