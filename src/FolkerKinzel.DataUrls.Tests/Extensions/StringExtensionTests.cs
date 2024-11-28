using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FolkerKinzel.DataUrls.Extensions.Tests;

[TestClass]
public class StringExtensionTests
{
    private const string DATA_URL_PROTOCOL = "data:";


    [DataTestMethod]
    [DataRow(DATA_URL_PROTOCOL, true)]
    [DataRow("data:bla", true)]
    [DataRow("          data:bla", true)]
    [DataRow("DATA:bla", true)]
    [DataRow("dotu:bla", false)]
    [DataRow("", false)]
    public void IsDataUrlTest1(string input, bool expected)
        => Assert.AreEqual(expected, input.IsDataUrl());


    [TestMethod]
    public void IsDataUrlTest2()
    {
        string? input = null;
        Assert.IsFalse(input.IsDataUrl());
    }
}
