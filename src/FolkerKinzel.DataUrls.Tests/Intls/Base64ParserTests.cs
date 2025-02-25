namespace FolkerKinzel.DataUrls.Intls.Tests;

[TestClass]
public class Base64ParserTests
{
    [DataTestMethod]
    [DataRow("ABCD")]
    [DataRow("ABC")]
    [DataRow("AB")]
    //[DataRow("A")]
    [DataRow("")]
    public void ParseTest2(string input) => Assert.IsTrue(Base64Helper.TryDecode(input.AsSpan(), out _));
}
