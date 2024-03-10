using System;
using System.Runtime.CompilerServices;
using System.Text;
using BenchmarkDotNet.Attributes;
using FolkerKinzel.DataUrls;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class UrisBench
    {
        //private readonly StringBuilder _builder = new(16);
        //private const string TEST = "test";

        private readonly DataUrlInfo _dataUrlText1;
        private readonly DataUrlInfo _dataUrlText2;

        public UrisBench()
        {
            const string data = "Märchenbücher";
            const string isoEncoding = "iso-8859-1";

#if NETSTANDARD2_0_OR_GREATER || NET5_0_OR_GREATER
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
            string s = $"data:;charset={isoEncoding};base64,{Convert.ToBase64String(Encoding.GetEncoding(isoEncoding).GetBytes(data))}";

            _ = DataUrl.TryParse(s, out _dataUrlText1);
            _ = DataUrl.TryParse(DataUrl.FromText(data, ""), out _dataUrlText2);
        }


        //[Benchmark]
        //public StringBuilder AppendStackallock()
        //{
        //    return _builder.Append(stackalloc char[] { 't', 'e', 's', 't' }).Clear();
        //}

        //[Benchmark]
        //public StringBuilder AppendString() => _builder.Append("test").Clear();

        //[Benchmark]
        //public bool StartsWithString1()
        //    => TEST.AsSpan().StartsWith("test", StringComparison.OrdinalIgnoreCase);

        //[Benchmark]
        //public bool StartsWithString2()
        //    => TEST.StartsWith("test", StringComparison.OrdinalIgnoreCase);


        //[Benchmark]
        //public bool StartsWithStackallock()
        //    => TEST.AsSpan().StartsWith(stackalloc char[] { 't', 'e', 's', 't' }, StringComparison.OrdinalIgnoreCase);


        //[Benchmark]
        //public bool EqualsBench()
        //{
        //    return _dataUrlText1.Equals(_dataUrlText2);
        //}

        [Benchmark]
        public static bool ReadOnlyMemoryByValue()
        {
            var memory = default(ReadOnlyMemory<char>);
            return DoReadOnlyMemoryByValue(memory);
        }

        [Benchmark]
        public static bool ReadOnlyMemoryByIn()
        {
            var memory = default(ReadOnlyMemory<char>);
            return DoReadOnlyMemoryByIn(ref memory);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool DoReadOnlyMemoryByValue(ReadOnlyMemory<char> largeStruct) => largeStruct.IsEmpty;

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool DoReadOnlyMemoryByIn(ref ReadOnlyMemory<char> largeStruct) => largeStruct.IsEmpty;

        //[Benchmark]
        //public bool DataUrlByValue()
        //{
        //    var dataUrl = default(DataUrl);
        //    return DoDataUrlByValue(dataUrl);
        //}

        //[Benchmark]
        //public bool DataUrlByIn()
        //{
        //    var dataUrl = default(DataUrl);
        //    return DoDataUrlByIn(in dataUrl);
        //}

        //[MethodImpl(MethodImplOptions.NoInlining)]
        //private static bool DoDataUrlByValue(DataUrl largeStruct) => largeStruct.IsEmpty;

        //[MethodImpl(MethodImplOptions.NoInlining)]
        //private static bool DoDataUrlByIn(in DataUrl largeStruct) => largeStruct.IsEmpty;

        //[Benchmark]
        //public bool DataUrlEqualsByValue()
        //{
        //    var dataUrl = default(DataUrl);
        //    return dataUrl.Equals(dataUrl);
        //}

        //[Benchmark]
        //public bool DataUrlEqualsByIn()
        //{
        //    var dataUrl = default(DataUrl);
        //    return dataUrl.Equals(in dataUrl);
        //}




        //[Benchmark]
        //public bool MimeTypeByValue() => DoMimeTypeByValue(MimeType.Empty);

        //[Benchmark]
        //public bool MimeTypeByIn() => DoMimeTypeByIn(MimeType.Empty);

        //[MethodImpl(MethodImplOptions.NoInlining)]
        //private static bool DoMimeTypeByValue(MimeType largeStruct) => largeStruct.IsEmpty;

        //[MethodImpl(MethodImplOptions.NoInlining)]
        //private static bool DoMimeTypeByIn(in MimeType largeStruct) => largeStruct.IsEmpty;





    }
}
