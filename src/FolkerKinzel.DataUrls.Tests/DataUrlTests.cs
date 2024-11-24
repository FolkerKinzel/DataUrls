using FolkerKinzel.DataUrls.Intls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneOf;
using System.Diagnostics.CodeAnalysis;
using System.Web;

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

        outText = DataUrl.FromBytes(new byte[] { 1, 2, 3 }, mime.AsInfo());

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

        outText = DataUrl.FromBytes(new byte[] { 1, 2, 3 }, mime.AsInfo());

        Assert.IsNotNull(outText);
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("http://wwww.folker-kinzel.de/index.htm")]
    public void TryParseTest3(string? value) => Assert.IsFalse(DataUrl.TryParse(value, out _));


    [TestMethod()]
    public void TryParseTest4()
    {
        string test = "data:;charset=UTF-8,Text";

        Assert.IsTrue(DataUrl.TryParse(test, out DataUrlInfo dataUrl2));

        Assert.AreEqual(dataUrl2.Data.ToString(), "Text");
        Assert.AreEqual(dataUrl2.MimeType.ToString(), "text/plain;charset=UTF-8");

        Assert.AreEqual(dataUrl2.DataEncoding, DataEncoding.Url);
        Assert.AreEqual(MimeTypeInfo.Parse(dataUrl2.MimeType).Parameters().First().Value.ToString(), "UTF-8");


        Assert.IsTrue(dataUrl2.TryAsText(out string? outString));
        Assert.AreEqual("Text", outString);
    }

    [TestMethod]
    public void TryParseTest5()
    {
        const string url = "data:application/x-octet,A%42C";
        byte[] data = "ABC"u8.ToArray();

        Assert.IsTrue(DataUrl.TryParse(url, out DataUrlInfo dataUrl));
        Assert.AreEqual(DataEncoding.Url, dataUrl.DataEncoding);
        Assert.AreEqual(DataType.Binary, dataUrl.DataType);

        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? output));

        CollectionAssert.AreEqual(data, output);
    }

    [DataTestMethod]
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
        Assert.AreEqual(256, bytes!.Length);

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
        Assert.AreEqual(DataEncoding.Url, dataUrl.DataEncoding);
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
        CollectionAssert.AreEqual(data, parsed);
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
        CollectionAssert.AreEqual(bytes, outBytes);
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
        CollectionAssert.AreEqual(bytes, outBytes);
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
        CollectionAssert.AreEqual(bytes, outBytes);
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
        CollectionAssert.AreEqual(bytes, outBytes);
    }

   

    [TestMethod]
    public void FromBytesTest4()
    {
        string url = DataUrl.FromBytes(null, dataEncoding: DataEncoding.Url);
        Assert.AreNotEqual(0, url.Length);
    }

    [TestMethod]
    public void FromBytesTest4b()
    {
        string url = DataUrl.FromBytes((IEnumerable<byte>?)null, dataEncoding: DataEncoding.Url);
        Assert.AreNotEqual(0, url.Length);
    }

    [TestMethod]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0301:Simplify collection initialization", Justification = "<Pending>")]
    public void FromBytesTest4d()
    {
        string url = DataUrl.FromBytes(ReadOnlySpan<byte>.Empty, dataEncoding: DataEncoding.Url);
        Assert.AreNotEqual(0, url.Length);
    }

    //[TestMethod]
    //[ExpectedException(typeof(ArgumentNullException))]
    //public void FromBytesTest5() => _ = DataUrl.FromBytes(Array.Empty<byte>(), (MimeType?)null!);


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
    [ExpectedException(typeof(ArgumentException))]
    public void FromBytesTest9() => _ = DataUrl.FromBytes(new byte[] { 1, 2, 3 }, new MimeTypeInfo());


    [TestMethod]
    public void FromFileTest1()
    {
        string path = TestFiles.FolkerPng;
        string url = DataUrl.FromFile(path);
        Assert.IsNotNull(url);

        Assert.IsTrue(DataUrl.TryParse(url, out DataUrlInfo dataUrl));

        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));

        CollectionAssert.AreEqual(outBytes, File.ReadAllBytes(path));
    }

    [TestMethod]
    public void FromFileTest2()
    {
        string path = TestFiles.EmptyTextFile;
        string url = DataUrl.FromFile(path);
        Assert.IsNotNull(url);
        Assert.IsTrue(DataUrl.TryParse(url, out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));
        CollectionAssert.AreEqual(outBytes, File.ReadAllBytes(path));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void FromFileTest3() => _ = DataUrl.FromFile(null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void FromFileTest4() => _ = DataUrl.FromFile("   ");

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
        StringAssert.Contains(url1, "image/jpeg");
    }

    [TestMethod]
    public void FromFileTest8()
    {
        const string fileName = "test.jpg";
        byte[] testData = [1, 2, 3];
        string path = Path.Combine(TestContext.TestRunResultsDirectory!, fileName);
        File.WriteAllBytes(path, testData);

        MimeTypeInfo mime = MimeTypeInfo.Parse("image/png");

        string url1 = DataUrl.FromFile(path, in mime);
        StringAssert.Contains(url1, "image/png");
    }

    //[TestMethod]
    //[ExpectedException(typeof(ArgumentNullException))]
    //public void FromFileTest9() => _ = DataUrl.FromFile("test.jpg", (MimeType?)null!);

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
    [ExpectedException(typeof(ArgumentException))]
    public void FromFileTest12() => _ = DataUrl.FromFile(TestFiles.FolkerPng, new MimeTypeInfo());


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

        Assert.AreEqual(info.MimeType.ToString(), "text/plain;charset=utf-8");

        Assert.AreEqual(1, MimeTypeInfo.Parse(info.MimeType).Parameters().Count());

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
        //string test = DATA_PROTOCOL + "text/plain;charset=utf-8" + ";" + DEFAULT_ENCODING + "," + Uri.EscapeDataString(text);

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
        StringAssert.Contains(url, ";charset=utf-8");
    }

    [TestMethod()]
    public void FromTextTest5()
    {
        string url = DataUrl.FromText("äöü", "text/plain; charset=\"\"");
        StringAssert.Contains(url, ";charset=utf-8");
    }

    [TestMethod()]
    public void FromTextTest6()
    {
        const string TEXT = "In Märchenbüchern herumstöbern.";

        MimeType mime = MimeType.Create("text", "plain").AppendParameter("charset", "iso-8859-1");

        string dataUrl1 = DataUrl.FromText(TEXT, mime);

        Assert.IsTrue(DataUrl.TryParse(dataUrl1, out DataUrlInfo info));

        Assert.AreEqual(info.MimeType.ToString(), "text/plain;charset=iso-8859-1");

        Assert.AreEqual(1, MimeTypeInfo.Parse(info.MimeType).Parameters().Count());

        Assert.IsTrue(info.TryAsText(out string? outText));
        Assert.AreEqual(TEXT, outText);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void FromTextTest7() => _ = DataUrl.FromText("text", (MimeType?)null!);

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
        StringBuilder sb = new StringBuilder(chunk.Length * 20100);

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
        StringBuilder sb = new StringBuilder(chunk.Length * 20100);

        for (int i = 0; i < 20000; i++)
        {
            sb.Append(chunk);
        }

        string url = "data:," + sb.ToString();
        Assert.IsTrue(DataUrl.TryParse(url, out DataUrlInfo info));
        Assert.IsTrue(info.TryAsText(out _));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AppendEmbeddedTextToTest1() => _ = DataUrl.AppendTextTo(null!, "", MimeType.Parse(MimeString.OctetStream));


    [TestMethod]
    public void AppendEmbeddedTextToTest2()
    {
        var stringBuilder = new StringBuilder();
        _ = DataUrl.AppendTextTo(stringBuilder, null, MimeType.Parse(MimeString.OctetStream));
        Assert.AreNotEqual(0, stringBuilder.Length);
    }


    [TestMethod]
    public void AppendEmbeddedBytesToTest1()
    {
        var sb = new StringBuilder();

        Assert.IsTrue(DataUrl.TryParse("data:application/octet-stream,%01%02%03", out DataUrlInfo info));
        Assert.IsTrue(info.TryAsBytes(out byte[]? embeddedBytes));
        DataUrl.AppendBytesTo(sb, embeddedBytes, MimeType.Parse(MimeString.OctetStream).AsInfo());
        Assert.AreNotEqual(0, sb.Length);
    }

    [TestMethod]
    public void AppendEmbeddedBytesToTest2()
    {
        var sb = new StringBuilder();

        Assert.IsTrue(DataUrl.TryParse("data:application/octet-stream;base64,ABCD", out DataUrlInfo info));
        Assert.IsTrue(info.TryAsBytes(out byte[]? embeddedBytes));
        _ = DataUrl.AppendBytesTo(sb, embeddedBytes, MimeString.OctetStream);
        Assert.AreNotEqual(0, sb.Length);
    }


    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AppendEmbeddedBytesToTest3() => _ = DataUrl.AppendBytesTo(null!, Array.Empty<byte>());

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0301:Simplify collection initialization", Justification = "<Pending>")]
    public void AppendEmbeddedBytesToTest3b() => _ = DataUrl.AppendBytesTo(null!, ReadOnlySpan<byte>.Empty, MimeTypeInfo.Parse("image/png"));

    [TestMethod]
    public void AppendEmbeddedBytesToTest4()
    {
        StringBuilder outText = DataUrl.AppendBytesTo(new StringBuilder(), (byte[]?)null, MimeType.Parse("text/plain").AsInfo());

        Assert.IsNotNull(outText);
        Assert.IsTrue(DataUrl.TryParse(outText.ToString(), out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));
        CollectionAssert.AreEqual(Array.Empty<byte>(), outBytes);
    }

    [TestMethod]
    public void AppendEmbeddedBytesToTest5()
    {
        StringBuilder outText = DataUrl.AppendBytesTo(new StringBuilder(), (IEnumerable<byte>?)null);

        Assert.IsNotNull(outText);
        Assert.IsTrue(DataUrl.TryParse(outText.ToString(), out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));
        CollectionAssert.AreEqual(Array.Empty<byte>(), outBytes);
    }

    [TestMethod]
    public void AppendEmbeddedBytesToTest6()
    {
        StringBuilder outText = DataUrl.AppendBytesTo(new StringBuilder(), (IEnumerable<byte>?)null, MimeType.Parse("text/plain").AsInfo());

        Assert.IsNotNull(outText);
        Assert.IsTrue(DataUrl.TryParse(outText.ToString(), out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));
        CollectionAssert.AreEqual(Array.Empty<byte>(), outBytes);
    }

    [TestMethod]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0301:Simplify collection initialization", Justification = "<Pending>")]
    public void AppendEmbeddedBytesToTest7()
    {
        StringBuilder outText = DataUrl.AppendBytesTo(new StringBuilder(), ReadOnlySpan<byte>.Empty, MimeType.Parse("text/plain").AsInfo());

        Assert.IsNotNull(outText);
        Assert.IsTrue(DataUrl.TryParse(outText.ToString(), out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));
        CollectionAssert.AreEqual(Array.Empty<byte>(), outBytes);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AppendEmbeddedFileToTest1() => _ = DataUrl.AppendFileTo(null!, "path");


    [TestMethod]
    public void TryGetEmbeddedDataTest1() => Assert.IsFalse(DataUrl.TryGetData((string?)null, out _, out _));

    [TestMethod]
    public void TryGetEmbeddedDataTest2()
    {
        Assert.IsTrue(DataUrl.TryGetData("data:image/jpeg,ABC", out OneOf<string, byte[]> data, out string? ext));
        Assert.IsInstanceOfType(data.Value, typeof(byte[]));
        Assert.AreEqual(".jpg", ext);
    }


    [TestMethod]
    public void TryGetEmbeddedDataTest3() => Assert.IsFalse(DataUrl.TryGetData("data:image/jpeg;base64,ÄÖÜ", out _, out _));


    [TestMethod]
    public void TryGetEmbeddedDataTest4()
    {
        Assert.IsTrue(DataUrl.TryGetData($"data:text/äöü,{Uri.EscapeDataString("ÄÖÜ")}", out OneOf<string, byte[]> data, out string? ext));
        Assert.IsInstanceOfType(data.Value, typeof(string));
        Assert.AreEqual("ÄÖÜ", data.Value.ToString());
        Assert.AreEqual(".bin", ext);
    }

}
