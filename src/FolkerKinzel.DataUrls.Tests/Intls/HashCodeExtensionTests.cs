using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.DataUrls.Intls.Tests;

[TestClass]
public class HashCodeExtensionTests
{
    [TestMethod]
    public void AddBytesTest1()
    {
        var hashCode = new HashCode();

        int hash1 = hashCode.ToHashCode();

        hashCode.AddBytes(new byte[] { 1, 2, 3 });

        int hash2 = hashCode.ToHashCode();

        Assert.AreNotEqual(hash1, hash2);
    }
}
