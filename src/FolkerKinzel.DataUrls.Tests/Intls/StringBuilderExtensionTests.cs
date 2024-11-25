using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FolkerKinzel.DataUrls.Intls.Tests;

[TestClass]
public class StringBuilderExtensionTests
{
    //private const string DATA_URL_PROTOCOL = "data:";
    //private const string BASE64 = ";base64,";

    

    //[TestMethod]
    //public void AppendDataUrlProtocolTest()
    //{
    //    var sb = new StringBuilder();
    //    Assert.AreEqual(sb, sb.Append(DATA_URL_PROTOCOL));
    //    Assert.AreEqual(DATA_URL_PROTOCOL, sb.ToString());
    //}

    //[TestMethod]
    //public void AppendBase64Test()
    //{
    //    var sb = new StringBuilder();
    //    Assert.AreEqual(sb, sb.Append(BASE64));
    //    Assert.AreEqual(BASE64, sb.ToString());
    //}

    [TestMethod]
    public void AppendMediaTypeTest1()
    {
        Assert.IsTrue(MimeTypeInfo.TryParse("text/plain", out MimeTypeInfo media));

        var sb = new StringBuilder();

        Assert.AreEqual(sb, sb.AppendMediaType(in media));

        Assert.AreEqual("", sb.ToString());
    }

    [TestMethod]
    public void AppendMediaTypeTest2()
    {
        Assert.IsTrue(MimeTypeInfo.TryParse("text/plain;charset=iso-8859-1", out MimeTypeInfo media));

        var sb = new StringBuilder();

        Assert.AreEqual(sb, sb.AppendMediaType(in media));

        Assert.AreEqual(";charset=iso-8859-1", sb.ToString());
    }

    [TestMethod]
    public void AppendMediaTypeTest3()
    {
        string input = "text/html;charset=iso-8859-1";
        Assert.IsTrue(MimeTypeInfo.TryParse(input, out MimeTypeInfo media));

        var sb = new StringBuilder();

        Assert.AreEqual(sb, sb.AppendMediaType(in media));

        Assert.AreEqual(input, sb.ToString());
    }
}
