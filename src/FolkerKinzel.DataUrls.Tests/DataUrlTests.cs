using FolkerKinzel.DataUrls.Intls;
using System.Diagnostics.CodeAnalysis;

namespace FolkerKinzel.DataUrls.Tests;

[TestClass]
public class DataUrlTests
{
    [NotNull]
    public TestContext? TestContext { get; set; }

    [TestMethod]
    public void TryParseTest1()
    {
        string text = "http://www.fölkerchen.de";


        string test = DataUrl.Scheme + "text/plain;charset=utf-8" + ";" + "UrlEncoding" + "," + Uri.EscapeDataString(text);

        Assert.IsTrue(DataUrl.TryParse(test, out DataUrlInfo dataUri));

        Assert.IsTrue(dataUri.TryAsText(out string? outText));
        Assert.AreEqual(text, outText);

        outText = DataUrl.FromText(text, "");

        Assert.IsNotNull(outText);

        Assert.IsTrue(MimeType.TryParse("application/x-octet", out MimeType? mime));

        outText = DataUrl.FromBytes([1, 2, 3], mime.AsInfo());

        Assert.IsNotNull(outText);
    }

    [TestMethod]
    public void TryParseTest2()
    {
        string text = "http://www.fölkerchen.de";
        //string test = DATA_PROTOCOL + "text/plain;charset=utf-8" + ";" + DEFAULT_ENCODING + "," + Uri.EscapeDataString(text);

        string outText = DataUrl.FromText(text, "");

        Assert.IsNotNull(outText);

        Assert.IsTrue(MimeType.TryParse("application/x-octet", out MimeType? mime));

        outText = DataUrl.FromBytes([1, 2, 3], mime.AsInfo());

        Assert.IsNotNull(outText);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("http://wwww.folker-kinzel.de/index.htm")]
    public void TryParseTest3(string? value) => Assert.IsFalse(DataUrl.TryParse(value, out _));


    [TestMethod()]
    public void TryParseTest4()
    {
        string test = "data:;charset=UTF-8,Text";

        Assert.IsTrue(DataUrl.TryParse(test, out DataUrlInfo dataUrl2));

        Assert.AreEqual("Text", dataUrl2.Data.ToString());
        Assert.AreEqual("text/plain;charset=UTF-8", dataUrl2.MimeType.ToString());

        Assert.AreEqual(DataEncoding.Url, dataUrl2.Encoding);
        Assert.AreEqual("UTF-8", 
                        MimeTypeInfo.Parse(dataUrl2.MimeType).Parameters().First().Value.ToString());


        Assert.IsTrue(dataUrl2.TryAsText(out string? outString));
        Assert.AreEqual("Text", outString);
    }

    [TestMethod]
    public void TryParseTest5()
    {
        const string url = "data:application/x-octet,A%42C";
        byte[] data = "ABC"u8.ToArray();

        Assert.IsTrue(DataUrl.TryParse(url, out DataUrlInfo dataUrl));
        Assert.AreEqual(DataEncoding.Url, dataUrl.Encoding);
        Assert.AreEqual(DataType.Binary, dataUrl.DataType);

        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? output));

        Assert.AreSequenceEqual(data, output);
    }

    [TestMethod]
    [DataRow("data:abc")]
    //[DataRow("data:,a bc")]
    public void TryParseTest7(string input) => Assert.IsFalse(DataUrl.TryParse(input, out _));

    [TestMethod]
    public void TryParseTest8()
    {
        const string data = "Märchenbücher";
        const string isoEncoding = "iso-8859-1";

        string s = $"data:;charset={isoEncoding};base64,{Convert.ToBase64String(TextEncodingConverter.GetEncoding(isoEncoding).GetBytes(data))}";

        Assert.IsTrue(DataUrl.TryParse(s, out DataUrlInfo _));
    }

    [TestMethod]
    public void TryParseTest9()
    {
        var sb = new StringBuilder(256 * 3);

        for (int i = 0; i < 256; i++)
        {
            _ = sb.Append('%').Append(i.ToString("x2"));
        }

        Assert.IsTrue(DataUrl.TryParse($"data:application/octet-stream,{sb}", out DataUrlInfo dataUrl));
        Assert.AreEqual(DataType.Binary, dataUrl.DataType);
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? bytes));
        Assert.HasCount(256, bytes);

        for (int i = 0; i < bytes!.Length; i++)
        {
            Assert.AreEqual(i, bytes[i]);
        }
    }

    [TestMethod]
    public void TryParseTest10()
    {
        const string text = "This is long Ascii text.";
        string urlString = DataUrl.FromText(text);

        Assert.IsTrue(DataUrl.TryParse(urlString, out DataUrlInfo dataUrl));
        Assert.AreEqual(DataEncoding.Url, dataUrl.Encoding);
        Assert.AreEqual(DataType.Text, dataUrl.DataType);
        Assert.IsTrue(dataUrl.TryAsText(out string? outText));
        Assert.AreEqual(text, outText);
    }

    [TestMethod]
    public void TryParseTest11()
    {
        const string input = "data:blabla,abc";
        Assert.IsTrue(DataUrl.TryParse(input, out DataUrlInfo info));
        Assert.AreEqual("abc", info.Data.ToString());
        Assert.AreEqual(".bin", info.GetFileTypeExtension());
    }

    [TestMethod]
    public void TryParseTest12()
    {
        byte[] data = [1, 2, 3];
        string url = DataUrl.FromBytes(data, MimeType.Parse("application/x-stuff; key=\";bla,blabla\"").AsInfo());
        Assert.IsTrue(DataUrl.TryParse(url, out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? parsed));
        Assert.AreSequenceEqual(data, parsed);
    }

    [TestMethod]
    public void TryParseTest13()
    {
        ReadOnlyMemory<char> mem = "data:application/octet-stream;base64,ABCD".AsMemory();
        Assert.IsTrue(DataUrl.TryParse(mem, out DataUrlInfo info));
        Assert.IsFalse(info.IsEmpty);
    }

    [TestMethod]
    public void TryParseTest14()
    {
        ReadOnlyMemory<char> mem = "blabla".AsMemory();
        Assert.IsFalse(DataUrl.TryParse(mem, out _));
    }

    [TestMethod]
    public void TryParseTest15()
    {
        const string mem = "blabla";
        Assert.IsFalse(DataUrl.TryParse(mem, out _));
    }

    [TestMethod]
    public void TryParseTest16()
    {
        string s = $"data:application/{new string('a', short.MaxValue)},";
        Assert.IsFalse(DataUrl.TryParse(s, out _));
    }


    [TestMethod]
    public void FromBytesTest2()
    {
        Assert.IsTrue(MimeType.TryParse("application/x-octet", out MimeType? mime));

        byte[] bytes = [1, 2, 3];
        string outText = DataUrl.FromBytes(bytes, mime.AsInfo());

        Assert.IsNotNull(outText);
        Assert.IsTrue(DataUrl.TryParse(outText, out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));
        Assert.AreSequenceEqual(bytes, outBytes);
    }

    [TestMethod]
    public void FromBytesTest2b()
    {
        Assert.IsTrue(MimeType.TryParse("application/x-octet", out MimeType? mime));

        byte[] bytes = [1, 2, 3];
        string outText = DataUrl.FromBytes(bytes.AsEnumerable(), mime.AsInfo());

        Assert.IsNotNull(outText);
        Assert.IsTrue(DataUrl.TryParse(outText, out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));
        Assert.AreSequenceEqual(bytes, outBytes);
    }

    [TestMethod]
    public void FromBytesTest2c()
    {
        Assert.IsTrue(MimeType.TryParse("application/x-octet", out MimeType? mime));

        byte[] bytes = [1, 2, 3];
        string outText = DataUrl.FromBytes(bytes.ToList(), mime.AsInfo());

        Assert.IsNotNull(outText);
        Assert.IsTrue(DataUrl.TryParse(outText, out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));
        Assert.AreSequenceEqual(bytes, outBytes);
    }

    [TestMethod]
    public void FromBytesTest2d()
    {
        Assert.IsTrue(MimeType.TryParse("application/x-octet", out MimeType? mime));

        byte[] bytes = [1, 2, 3];
        string outText = DataUrl.FromBytes(bytes.AsSpan(), mime.AsInfo());

        Assert.IsNotNull(outText);
        Assert.IsTrue(DataUrl.TryParse(outText, out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));
        Assert.AreSequenceEqual(bytes, outBytes);
    }

    [TestMethod]
    public void FromBytesTest4()
    {
        string url = DataUrl.FromBytes(null, encoding: DataEncoding.Url);
        Assert.AreNotEqual(0, url.Length);
    }

    [TestMethod]
    public void FromBytesTest4b()
    {
        string url = DataUrl.FromBytes((IEnumerable<byte>?)null, encoding: DataEncoding.Url);
        Assert.AreNotEqual(0, url.Length);
    }

    [TestMethod]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0301:Simplify collection initialization", Justification = "<Pending>")]
    public void FromBytesTest4d()
    {
        string url = DataUrl.FromBytes(ReadOnlySpan<byte>.Empty, encoding: DataEncoding.Url);
        Assert.AreNotEqual(0, url.Length);
    }

    [TestMethod]
    public void FromBytesTest6()
    {
        string url = DataUrl.FromBytes(null, "nixmime/äöü");
        Assert.AreNotEqual(0, url.Length);
        Assert.IsTrue(DataUrl.TryParse(url, out DataUrlInfo info));
        Assert.AreEqual("application/octet-stream", info.MimeType.ToString());
    }

    [TestMethod]
    public void FromBytesTest7()
    {
        string url = DataUrl.FromBytes(null, "image/png");
        Assert.AreNotEqual(0, url.Length);
        Assert.IsTrue(DataUrl.TryParse(url, out DataUrlInfo info));
        Assert.AreEqual("image/png", info.MimeType.ToString());
    }

    [TestMethod]
    public void FromBytesTest8()
    {
        string url = DataUrl.FromBytes(null, "");
        Assert.AreNotEqual(0, url.Length);
        Assert.IsTrue(DataUrl.TryParse(url, out DataUrlInfo info));
        Assert.AreEqual("application/octet-stream", info.MimeType.ToString());
    }

    [TestMethod]
    public void FromBytesTest9()
        => _ = Assert.ThrowsExactly<ArgumentException>(
                () => DataUrl.FromBytes([1, 2, 3], new MimeTypeInfo()));


    [TestMethod]
    public void FromFileTest1()
    {
        string path = TestFiles.FolkerPng;
        string url = DataUrl.FromFile(path);
        Assert.IsNotNull(url);

        Assert.IsTrue(DataUrl.TryParse(url, out DataUrlInfo dataUrl));

        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));

        Assert.AreSequenceEqual(outBytes, File.ReadAllBytes(path));
    }

    [TestMethod]
    public void FromFileTest2()
    {
        string path = TestFiles.EmptyTextFile;
        string url = DataUrl.FromFile(path);
        Assert.IsNotNull(url);
        Assert.IsTrue(DataUrl.TryParse(url, out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));
        Assert.AreSequenceEqual(outBytes, File.ReadAllBytes(path));
    }

    [TestMethod]
    public void FromFileTest3()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
                () => DataUrl.FromFile(null!));

    [TestMethod]
    public void FromFileTest4() 
        => _ = Assert.ThrowsExactly<ArgumentException>(
                () => DataUrl.FromFile("   "));

    [TestMethod]
    public void FromFileTest5()
    {
        string path = TestFiles.Utf8;
        string fileContent = File.ReadAllText(path);

        string url = DataUrl.FromFile(path);

        Assert.IsTrue(DataUrl.TryParse(url, out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsText(out string? dataUrlText));

        Assert.AreEqual(fileContent, dataUrlText);
    }

    [TestMethod]
    public void FromFileTest6()
    {
        string path = TestFiles.Utf16LE;
        string fileContent = File.ReadAllText(path);

        string url = DataUrl.FromFile(path);

        Assert.IsTrue(DataUrl.TryParse(url, out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsText(out string? dataUrlText));

        Assert.AreEqual(fileContent, dataUrlText);
    }

    [TestMethod]
    public void FromFileTest7()
    {
        const string fileName = "test.jpg";
        byte[] testData = [1, 2, 3];
        string path = Path.Combine(TestContext.TestRunResultsDirectory!, fileName);
        File.WriteAllBytes(path, testData);

        string url1 = DataUrl.FromFile(path);
        Assert.Contains("image/jpeg", url1);
    }

    [TestMethod]
    public void FromFileTest8()
    {
        const string fileName = "test.jpg";
        byte[] testData = [1, 2, 3];
        string path = Path.Combine(TestContext.TestRunResultsDirectory!, fileName);
        File.WriteAllBytes(path, testData);

        var mime = MimeTypeInfo.Parse("image/png");

        string url1 = DataUrl.FromFile(path, in mime);
        Assert.Contains("image/png", url1);
    }

    [TestMethod]
    public void FromFileTest10()
    {
        string url = DataUrl.FromFile(TestFiles.FolkerPng, "nixmime/äöü");
        Assert.AreNotEqual(0, url.Length);
        Assert.IsTrue(DataUrl.TryParse(url, out DataUrlInfo info));
        Assert.AreEqual("image/png", info.MimeType.ToString());
    }

    [TestMethod]
    public void FromFileTest11()
    {
        string url = DataUrl.FromFile(TestFiles.FolkerPng, "image/jpeg");
        Assert.AreNotEqual(0, url.Length);
        Assert.IsTrue(DataUrl.TryParse(url, out DataUrlInfo info));
        Assert.AreEqual("image/jpeg", info.MimeType.ToString());
    }

    [TestMethod]
    public void FromFileTest12() 
        => _ = Assert.ThrowsExactly<ArgumentException>(
                () => DataUrl.FromFile(TestFiles.FolkerPng, new MimeTypeInfo()));


    [TestMethod]
    public void FromTextOnNull()
    {
        string urlString = DataUrl.FromText(null);
        Assert.IsNotNull(urlString);
        Assert.IsTrue(DataUrl.TryParse(urlString, out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsText(out string? output));
        Assert.AreEqual(string.Empty, output);
    }

    [TestMethod]
    public void FromTextOnStringEmpty()
    {
        string urlString = DataUrl.FromText("");
        Assert.IsTrue(DataUrl.TryParse(urlString, out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsText(out string? output));
        Assert.AreEqual(string.Empty, output);
    }

    [TestMethod()]
    public void FromTextTest1()
    {
        const string TEXT = "In Märchenbüchern herumstöbern.";

        string dataUrl1 = DataUrl.FromText(TEXT);

        Assert.IsTrue(DataUrl.TryParse(dataUrl1, out DataUrlInfo info));
        Assert.AreEqual("text/plain;charset=utf-8", info.MimeType.ToString());
        Assert.HasCount(1, MimeTypeInfo.Parse(info.MimeType).Parameters());
        Assert.IsTrue(info.TryAsText(out string? outText));
        Assert.AreEqual(TEXT, outText);
    }

    [TestMethod()]
    public void FromTextTest2()
    {
        const string TEXT = "1% + 2% = 3%";

        string dataUrl1 = DataUrl.FromText(TEXT);

        Assert.IsTrue(DataUrl.TryParse(dataUrl1, out DataUrlInfo info));
        Assert.AreEqual("text/plain", info.MimeType.ToString());
        Assert.IsTrue(info.TryAsText(out string? outText));
        Assert.AreEqual(TEXT, outText);
    }


    [TestMethod]
    public void FromTextTest3()
    {
        string text = "http://www.fölkerchen.de";

        string outText = DataUrl.FromText(text);

        Assert.IsNotNull(outText);
        Assert.IsTrue(DataUrl.TryParse(outText, out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsText(out string? output));
        Assert.AreEqual(text, output);
    }

    [TestMethod()]
    public void FromTextTest4()
    {
        string url = DataUrl.FromText("äöü", "text/plain; charset=nixda");
        Assert.Contains(";charset=utf-8", url);
    }

    [TestMethod()]
    public void FromTextTest5()
    {
        string url = DataUrl.FromText("äöü", "text/plain; charset=\"\"");
        Assert.Contains(";charset=utf-8", url);
    }

    [TestMethod()]
    public void FromTextTest6()
    {
        const string TEXT = "In Märchenbüchern herumstöbern.";

        MimeType mime = MimeType.Create("text", "plain").AppendParameter("charset", "iso-8859-1");

        string dataUrl1 = DataUrl.FromText(TEXT, mime);

        Assert.IsTrue(DataUrl.TryParse(dataUrl1, out DataUrlInfo info));
        Assert.AreEqual("text/plain;charset=iso-8859-1", info.MimeType.ToString());
        Assert.HasCount(1, MimeTypeInfo.Parse(info.MimeType).Parameters());
        Assert.IsTrue(info.TryAsText(out string? outText));
        Assert.AreEqual(TEXT, outText);
    }

    [TestMethod()]
    public void FromTextTest7()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
                () => DataUrl.FromText("text", (MimeType?)null!));

    [TestMethod]
    public void FromTextTest8()
    {
        string url = DataUrl.FromText(null, "nixmime/äöü");
        Assert.AreNotEqual(0, url.Length);
        Assert.IsTrue(DataUrl.TryParse(url, out DataUrlInfo info));
        Assert.AreEqual("text/plain", info.MimeType.ToString());
    }

    [TestMethod]
    public void FromTextTest9()
    {
        string url = DataUrl.FromText(null, "text/html");
        Assert.AreNotEqual(0, url.Length);
        Assert.IsTrue(DataUrl.TryParse(url, out DataUrlInfo info));
        Assert.AreEqual("text/html", info.MimeType.ToString());
    }


    [TestMethod]
    public void LargeFileTest1()
    {
        byte[] buf = new byte[1024 * 1024];
        new Random().NextBytes(buf);

        string url = DataUrl.FromBytes(buf, "application/octet-stream");
        Assert.IsTrue(DataUrl.TryParse(url, out DataUrlInfo info));
        Assert.IsTrue(info.TryAsBytes(out _));
    }

    [TestMethod]
    public void LargeFileTest2()
    {
        const string chunk = "%01%02%03";
        var sb = new StringBuilder(chunk.Length * 20100);

        for (int i = 0; i < 20000; i++)
        {
            sb.Append(chunk);
        }

        string url = "data:application/octet-stream," + sb.ToString();
        Assert.IsTrue(DataUrl.TryParse(url, out DataUrlInfo info));
        Assert.IsTrue(info.TryAsBytes(out _));
    }

    [TestMethod]
    public void LargeFileTest3()
    {
        const string chunk = "%01%02%03";
        var sb = new StringBuilder(chunk.Length * 20100);

        for (int i = 0; i < 20000; i++)
        {
            sb.Append(chunk);
        }

        string url = "data:," + sb.ToString();
        Assert.IsTrue(DataUrl.TryParse(url, out DataUrlInfo info));
        Assert.IsTrue(info.TryAsText(out _));
    }

    [TestMethod]
    public void AppendTextToTest1()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
                () => DataUrl.AppendTextTo(null!, "", MimeType.Parse(MimeString.OctetStream)));

    [TestMethod]
    public void AppendTextToTest2()
    {
        var stringBuilder = new StringBuilder();
        _ = DataUrl.AppendTextTo(stringBuilder, null, MimeType.Parse(MimeString.OctetStream));
        Assert.AreNotEqual(0, stringBuilder.Length);
    }

    [TestMethod]
    public void AppendBytesToTest1()
    {
        var sb = new StringBuilder();

        Assert.IsTrue(DataUrl.TryParse("data:application/octet-stream,%01%02%03", out DataUrlInfo info));
        Assert.IsTrue(info.TryAsBytes(out byte[]? embeddedBytes));
        DataUrl.AppendBytesTo(sb, embeddedBytes, MimeType.Parse(MimeString.OctetStream).AsInfo());
        Assert.AreNotEqual(0, sb.Length);
    }

    [TestMethod]
    public void AppendBytesToTest2()
    {
        var sb = new StringBuilder();

        Assert.IsTrue(DataUrl.TryParse("data:application/octet-stream;base64,ABCD", out DataUrlInfo info));
        Assert.IsTrue(info.TryAsBytes(out byte[]? embeddedBytes));
        _ = DataUrl.AppendBytesTo(sb, embeddedBytes, MimeString.OctetStream);
        Assert.AreNotEqual(0, sb.Length);
    }


    [TestMethod]
    [SuppressMessage("Style", "IDE0301:Simplify collection initialization",
                     Justification = "Needed for testing.")]
    public void AppendBytesToTest3() 
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
                () => DataUrl.AppendBytesTo(null!, Array.Empty<byte>()));

    [TestMethod]
    [SuppressMessage("Style", "IDE0301:Simplify collection initialization", 
                     Justification = "Needed for testing.")]
    public void AppendBytesToTest3b()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
                () => DataUrl.AppendBytesTo(null!, 
                                            ReadOnlySpan<byte>.Empty,
                                            MimeTypeInfo.Parse("image/png")));

    [TestMethod]
    public void AppendBytesToTest4()
    {
        StringBuilder outText = 
            DataUrl.AppendBytesTo(new StringBuilder(),
                                  (byte[]?)null,
                                  MimeType.Parse("text/plain").AsInfo());

        Assert.IsNotNull(outText);
        Assert.IsTrue(DataUrl.TryParse(outText.ToString(), out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));
        Assert.AreSequenceEqual([], outBytes);
    }

    [TestMethod]
    public void AppendBytesToTest5()
    {
        StringBuilder outText = DataUrl.AppendBytesTo(new StringBuilder(),
                                                      (IEnumerable<byte>?)null);

        Assert.IsNotNull(outText);
        Assert.IsTrue(DataUrl.TryParse(outText.ToString(), out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));
        Assert.AreSequenceEqual([], outBytes);
    }

    [TestMethod]
    public void AppendBytesToTest6()
    {
        StringBuilder outText = DataUrl.AppendBytesTo(new StringBuilder(),
                                                      (IEnumerable<byte>?)null,
                                                      MimeType.Parse("text/plain").AsInfo());

        Assert.IsNotNull(outText);
        Assert.IsTrue(DataUrl.TryParse(outText.ToString(), out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));
        Assert.AreSequenceEqual([], outBytes);
    }

    [TestMethod]
    [SuppressMessage("Style", "IDE0301:Simplify collection initialization", Justification = "<Pending>")]
    public void AppendBytesToTest7()
    {
        StringBuilder outText = DataUrl.AppendBytesTo(new StringBuilder(),
                                                      ReadOnlySpan<byte>.Empty,
                                                      MimeType.Parse("text/plain").AsInfo());

        Assert.IsNotNull(outText);
        Assert.IsTrue(DataUrl.TryParse(outText.ToString(), out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));
        Assert.AreSequenceEqual([], outBytes);
    }

    [TestMethod]
    public void AppendFileToTest1() 
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
                () => DataUrl.AppendFileTo(null!, "path"));


    [TestMethod]
    public void TryGetDataTest1()
        => Assert.IsFalse(DataUrl.TryGetData((string?)null, out _, out _));

    [TestMethod]
    public void TryGetDataTest2()
    {
        Assert.IsTrue(DataUrl.TryGetData("data:image/jpeg,ABC",
                                         out EmbeddedData data,
                                         out string? ext));
        Assert.IsNotNull(data.Bytes);
        Assert.AreEqual(".jpg", ext);
    }

    [TestMethod]
    public void TryGetDataTest3()
        => Assert.IsFalse(DataUrl.TryGetData("data:image/jpeg;base64,ÄÖÜ", out _, out _));


    [TestMethod]
    public void TryGetDataTest4()
    {
        Assert.IsTrue(DataUrl.TryGetData($"data:text/äöü,{Uri.EscapeDataString("ÄÖÜ")}",
                                         out EmbeddedData data,
                                         out string? ext));
        Assert.IsNotNull(data.Text);
        Assert.AreEqual("ÄÖÜ", data.Text);
        Assert.AreEqual(".bin", ext);
    }

    [TestMethod]
    public void TryGetBytesTest1()
        => Assert.IsFalse(DataUrl.TryGetBytes("data:application/octet-stream;base64,A", out _, out _));

    [TestMethod]
    public void TryGetBytesTest2()
        => Assert.IsTrue(DataUrl.TryGetBytes("data:application/octet-stream;base64,ABC", out _, out _));

    [TestMethod]
    public void TryGetBytesTest3()
        => Assert.IsTrue(DataUrl.TryGetBytes("data:,ABCDE", out _, out _));

    [TestMethod]
    public void TryGetBytesTest4()
        => Assert.IsFalse(DataUrl.TryGetBytes("blabla", out _, out _));

    [TestMethod]
    public void TryGetTextTest1()
        => Assert.IsFalse(DataUrl.TryGetText("data:;base64,A", out _, out _));

    [TestMethod]
    public void TryGetTextTest2()
    {
        Assert.IsTrue(DataUrl.TryGetText("data:text/plain;charset=utf-8;base64,ABC",
                                         out _,
                                         out string? ext));
        Assert.AreEqual(".txt", ext);
    }

    [TestMethod]
    public void TryGetTextTest3() => Assert.IsFalse(DataUrl.TryGetText("blabla", out _, out _));

    [TestMethod]
    public void TryGetTextTest4()
    {
        string base64 = Convert.ToBase64String([190, 208]);
        Assert.IsFalse(DataUrl.TryGetText($"data:text/plain;charset=utf-8;base64,{base64}",
                                          out _,
                                          out _));
    }
}
