using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.DataUrls.Extensions.Tests;

[TestClass]
public class ReadOnlySpanExtensionTests
{
    private const string DATA_URL_PROTOCOL = "data:";


    [DataTestMethod]
    [DataRow(DATA_URL_PROTOCOL, true)]
    [DataRow("data:bla", true)]
    [DataRow("DATA:bla", true)]
    [DataRow("dotu:bla", false)]
    [DataRow("", false)]
    [DataRow(null, false)]
    public void IsDataUrlTest1(string? input, bool expected)
        => Assert.AreEqual(expected, input.AsSpan().IsDataUrl());

    
}
