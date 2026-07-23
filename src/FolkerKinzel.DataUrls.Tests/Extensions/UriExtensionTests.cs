namespace FolkerKinzel.DataUrls.Extensions.Tests;

[TestClass]
public class UriExtensionTests
{
    [TestMethod]
    [DataRow("data:,", true)]
    [DataRow("DATA:,bla", true)]
    [DataRow("dotu:,bla", false)]
    [DataRow("http://www.contoso.com/", false)]
    public void IsDataUrlTest2(string input, bool expected)
    {
        var uri = new Uri(input);
        Assert.AreEqual(expected, uri.IsDataUrl());
    }

    [TestMethod]
    public void IsDataUrlTest3()
    {
        Uri? uri = null;
        _ = Assert.ThrowsExactly<ArgumentNullException>(
               () => uri!.IsDataUrl());
    }
}
