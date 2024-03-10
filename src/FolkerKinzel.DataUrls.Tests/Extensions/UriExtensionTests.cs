namespace FolkerKinzel.DataUrls.Extensions.Tests;

[TestClass]
public class UriExtensionTests
{
    [DataTestMethod]
    [DataRow("data:,", true)]
    [DataRow("DATA:,bla", true)]
    [DataRow("dotu:,bla", false)]
    [DataRow("http://www.contoso.com/", false)]
    public void IsDataUrlTest2(string input, bool expected)
    {
        Uri? uri = new Uri(input);
        Assert.AreEqual(expected, uri.IsDataUrl());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void IsDataUrlTest3() 
    {
        Uri? uri = null;
        _ = uri!.IsDataUrl();
    }
}
