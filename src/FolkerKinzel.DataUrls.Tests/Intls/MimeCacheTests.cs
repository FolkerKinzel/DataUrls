using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.Uris.Intls.Tests
{
    [TestClass]
    public class MimeCacheTests
    {
        //[TestMethod()]
        //public void TestItTest() => MimeCache.TestIt();

        [DataTestMethod]
        [DataRow(".json", "application/json")]
        [DataRow(".psd", "image/vnd.adobe.photoshop")]
        [DataRow(".json", "application/json")]
        [DataRow("##############++", "application/octet-stream")]
        public void GetMimeTypeTest1(string extension, string mimeType) => Assert.AreEqual(mimeType, MimeCache.GetMimeType(extension), true);



        [DataTestMethod]
        //[DataRow(".ez", "application/andrew-inset")]
        [DataRow(".ice", "x-conference/x-cooltalk")]
        [DataRow(".ttc", "font/collection")]
        [DataRow(".woff2", "font/woff2")]
        [DataRow(".bin", "font/blabla")]
        [DataRow(".json", "application/json")]
        [DataRow(".psd", "image/vnd.adobe.photoshop")]
        [DataRow(".json", "application/json")]
        [DataRow(".bin", "blabla/nichda")]
        public void GetFileTypeExtensionTest1(string extension, string mimeType) => Assert.AreEqual(extension, MimeCache.GetFileTypeExtension(mimeType), true);



    }
}
