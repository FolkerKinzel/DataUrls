namespace FolkerKinzel.DataUrls.Extensions.Tests;

[TestClass]
public class StringBuilderExtensionTests
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AppendDataUrlTest1()
    {
        StringBuilder? builder = null;
        _ = builder!.AppendDataUrl("", MimeType.Parse(MimeString.OctetStream));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AppendDataUrlTest2()
    {
        StringBuilder? builder = null;
        _ = builder!.AppendDataUrl("");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AppendDataUrlTest3()
    {
        StringBuilder? builder = null;
        byte[] bytes = [];
        _ = builder!.AppendDataUrl(bytes);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AppendDataUrlTest4()
    {
        StringBuilder? builder = null;
        byte[] bytes = [];
        var mimeTypeInfo = MimeTypeInfo.Parse(MimeString.OctetStream);
        _ = builder!.AppendDataUrl(bytes, in mimeTypeInfo);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AppendDataUrlTest5()
    {
        StringBuilder? builder = null;
        IEnumerable<byte> bytes = [];
        _ = builder!.AppendDataUrl(bytes);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AppendDataUrlTest6()
    {
        StringBuilder? builder = null;
        IEnumerable<byte> bytes = [];
        var mimeTypeInfo = MimeTypeInfo.Parse(MimeString.OctetStream);
        _ = builder!.AppendDataUrl(bytes, in mimeTypeInfo);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AppendDataUrlTest7()
    {
        StringBuilder? builder = null;
        ReadOnlySpan<byte> bytes = [];
        _ = builder!.AppendDataUrl(bytes);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AppendDataUrlTest8()
    {
        StringBuilder? builder = null;
        ReadOnlySpan<byte> bytes = [];
        var mimeTypeInfo = MimeTypeInfo.Parse(MimeString.OctetStream);
        _ = builder!.AppendDataUrl(bytes, in mimeTypeInfo);
    }

    [TestMethod]
    public void AppendDataUrlTest9()
    {
        var builder = new StringBuilder();
        _ = builder.AppendDataUrl(null, MimeType.Parse(MimeString.OctetStream));
        Assert.AreNotEqual(0, builder.Length);
    }


    [TestMethod]
    public void AppendDataUrlTest10()
    {
        var sb = new StringBuilder();

        Assert.IsTrue(DataUrl.TryParse("data:application/octet-stream,%01%02%03", out DataUrlInfo info));
        Assert.IsTrue(info.TryAsBytes(out byte[]? embeddedBytes));
        sb.AppendDataUrl(embeddedBytes, MimeType.Parse(MimeString.OctetStream).AsInfo());
        Assert.AreNotEqual(0, sb.Length);
    }

    [TestMethod]
    public void AppendDataUrlTest11()
    {
        var sb = new StringBuilder();

        Assert.IsTrue(DataUrl.TryParse("data:application/octet-stream;base64,ABCD", out DataUrlInfo info));
        Assert.IsTrue(info.TryAsBytes(out byte[]? embeddedBytes));
        _ = sb.AppendDataUrl(embeddedBytes, MimeString.OctetStream);
        Assert.AreNotEqual(0, sb.Length);
    }

    [TestMethod]
    public void AppendDataUrlTest12()
    {
        StringBuilder outText = new();

        outText = outText.AppendDataUrl((byte[]?)null, MimeType.Parse("text/plain").AsInfo());

        Assert.IsNotNull(outText);
        Assert.IsTrue(DataUrl.TryParse(outText.ToString(), out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));
        CollectionAssert.AreEqual(Array.Empty<byte>(), outBytes);
    }

    [TestMethod]
    public void AppendDataUrlTest13()
    {
        StringBuilder outText = new();

        outText = outText.AppendDataUrl((IEnumerable<byte>?)null);

        Assert.IsNotNull(outText);
        Assert.IsTrue(DataUrl.TryParse(outText.ToString(), out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));
        CollectionAssert.AreEqual(Array.Empty<byte>(), outBytes);
    }

    [TestMethod]
    public void AppendDataUrlTest14()
    {
        StringBuilder outText = new();
        outText = outText.AppendDataUrl((IEnumerable<byte>?)null, MimeType.Parse("text/plain").AsInfo());

        Assert.IsNotNull(outText);
        Assert.IsTrue(DataUrl.TryParse(outText.ToString(), out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));
        CollectionAssert.AreEqual(Array.Empty<byte>(), outBytes);
    }

    [TestMethod]
    public void AppendDataUrlTest15()
    {
        StringBuilder outText = new();

        ReadOnlySpan<byte> span = [];
        outText = outText.AppendDataUrl(span, MimeType.Parse("text/plain").AsInfo());

        Assert.IsNotNull(outText);
        Assert.IsTrue(DataUrl.TryParse(outText.ToString(), out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));
        CollectionAssert.AreEqual(Array.Empty<byte>(), outBytes);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AppendFileAsDataUrlTest1()
    {
        StringBuilder? outText = null;
        _ = outText!.AppendFileAsDataUrl("path");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AppendFileAsDataUrlTest2()
    {
        StringBuilder? outText = null;
        _ = outText!.AppendFileAsDataUrl("path", MimeType.Parse(MimeString.OctetStream).AsInfo());
    }
}
