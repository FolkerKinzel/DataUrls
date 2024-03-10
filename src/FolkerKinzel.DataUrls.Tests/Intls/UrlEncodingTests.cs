using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FolkerKinzel.Uris.Intls.Tests;

[TestClass]
public class UrlEncodingTests
{
    [TestMethod]
    public void EncodeBytesTest1()
    {
        byte[] bytes = new byte[] { 131, 142, 175 };
        string s = UrlEncoding.EncodeBytes(bytes);
        StringAssert.Contains(s, "%");

    }

    [TestMethod]
    public void TryDecodeStringTest1() => Assert.IsFalse(UrlEncoding.TryDecodeString("äöü", "utf-8", out _));

    [TestMethod]
    public void TryDecodeStringTest2() => Assert.IsFalse(UrlEncoding.TryDecodeString("%AF%CB%FF", "utf-8", out _));


    [TestMethod]
    public void TryDecodeBytesTest1() => Assert.IsFalse(UrlEncoding.TryDecodeBytes("äöü", out _));

}
