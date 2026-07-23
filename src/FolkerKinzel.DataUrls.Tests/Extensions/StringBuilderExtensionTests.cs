namespace FolkerKinzel.DataUrls.Extensions.Tests;

[TestClass]
public class StringBuilderExtensionTests
{
    [TestMethod]
    public void AppendDataUrlTest1()
    {
        StringBuilder? builder = null;
        _ = Assert.ThrowsExactly<ArgumentNullException>(
               () => builder!.AppendDataUrl("", MimeType.Parse(MimeString.OctetStream)));
    }

    [TestMethod]
    public void AppendDataUrlTest2()
    {
        StringBuilder? builder = null;
        _ = Assert.ThrowsExactly<ArgumentNullException>(
               () => builder!.AppendDataUrl(""));
    }

    [TestMethod]
    public void AppendDataUrlTest3()
    {
        StringBuilder? builder = null;
        byte[] bytes = [];
        _ = Assert.ThrowsExactly<ArgumentNullException>(
               () => builder!.AppendDataUrl(bytes));
    }

    [TestMethod]
    public void AppendDataUrlTest4()
    {
        StringBuilder? builder = null;
        byte[] bytes = [];
        var mimeTypeInfo = MimeTypeInfo.Parse(MimeString.OctetStream);
        _ = Assert.ThrowsExactly<ArgumentNullException>(
               () => builder!.AppendDataUrl(bytes, in mimeTypeInfo));
    }

    [TestMethod]
    public void AppendDataUrlTest5()
    {
        StringBuilder? builder = null;
        IEnumerable<byte> bytes = [];
        _ = Assert.ThrowsExactly<ArgumentNullException>(
               () => builder!.AppendDataUrl(bytes));
    }

    [TestMethod]
    public void AppendDataUrlTest6()
    {
        StringBuilder? builder = null;
        IEnumerable<byte> bytes = [];
        var mimeTypeInfo = MimeTypeInfo.Parse(MimeString.OctetStream);
        _ = Assert.ThrowsExactly<ArgumentNullException>(
               () => builder!.AppendDataUrl(bytes, in mimeTypeInfo));
    }

    [TestMethod]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", 
        "IDE0301:Simplify collection initialization", Justification = "<Pending>")]
    public void AppendDataUrlTest7()
    {
        StringBuilder? builder = null;
        _ = Assert.ThrowsExactly<ArgumentNullException>(
               () => builder!.AppendDataUrl(ReadOnlySpan<byte>.Empty));
    }

    [TestMethod]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", 
        "IDE0301:Simplify collection initialization", Justification = "<Pending>")]
    public void AppendDataUrlTest8()
    {
        StringBuilder? builder = null;
        var mimeTypeInfo = MimeTypeInfo.Parse(MimeString.OctetStream);
        _ = Assert.ThrowsExactly<ArgumentNullException>(
               () => builder!.AppendDataUrl(ReadOnlySpan<byte>.Empty, in mimeTypeInfo));
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

        Assert.IsTrue(DataUrl.TryParse("data:application/octet-stream,%01%02%03",
                                       out DataUrlInfo info));
        Assert.IsTrue(info.TryAsBytes(out byte[]? embeddedBytes));
        sb.AppendDataUrl(embeddedBytes, MimeType.Parse(MimeString.OctetStream).AsInfo());
        Assert.AreNotEqual(0, sb.Length);
    }

    [TestMethod]
    public void AppendDataUrlTest11()
    {
        var sb = new StringBuilder();

        Assert.IsTrue(DataUrl.TryParse("data:application/octet-stream;base64,ABCD",
                                       out DataUrlInfo info));
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
        Assert.HasCount(0, outBytes);
    }

    [TestMethod]
    public void AppendDataUrlTest13()
    {
        StringBuilder outText = new();

        outText = outText.AppendDataUrl((IEnumerable<byte>?)null);

        Assert.IsNotNull(outText);
        Assert.IsTrue(DataUrl.TryParse(outText.ToString(), out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));
        Assert.HasCount(0, outBytes);
    }

    [TestMethod]
    public void AppendDataUrlTest14()
    {
        StringBuilder outText = new();
        outText = outText.AppendDataUrl((IEnumerable<byte>?)null,
                                        MimeType.Parse("text/plain").AsInfo());

        Assert.IsNotNull(outText);
        Assert.IsTrue(DataUrl.TryParse(outText.ToString(), out DataUrlInfo dataUrl));
        Assert.IsTrue(dataUrl.TryAsBytes(out byte[]? outBytes));
        Assert.HasCount(0, outBytes);
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
        Assert.HasCount(0, outBytes);
    }

    [TestMethod]
    public void AppendFileAsDataUrlTest1()
    {
        StringBuilder? outText = null;
        _ = Assert.ThrowsExactly<ArgumentNullException>(
                () => outText!.AppendFileAsDataUrl("path"));
    }

    [TestMethod]
    public void AppendFileAsDataUrlTest2()
    {
        StringBuilder? outText = null;
        _ = Assert.ThrowsExactly<ArgumentNullException>(
               () => outText!.AppendFileAsDataUrl("path",
                                                  MimeType.Parse(MimeString.OctetStream).AsInfo()));
    }
}
