namespace FolkerKinzel.DataUrls.Tests;

[TestClass]
public class EmbeddedDataTests
{
    [TestMethod]
    public void DefaultTest()
    {
        EmbeddedData data = default;
        Assert.IsNull(data.Text);
        Assert.IsNotNull(data.Bytes);

    }
}
