using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.Uris.Intls.Tests
{
    [TestClass]
    public class TextEncodingConverterTests
    {
        [DataTestMethod]
        [DataRow(null, 65001)]
        [DataRow("iso-8859-1", 28591)]
        [DataRow("ISO-8859-1", 28591)]
        [DataRow("unBekannt", 65001)]
        public void GetEncodingTest(string? input, int codePage)
        {
            Assert.AreEqual(codePage, TextEncodingConverter.GetEncoding(input).CodePage);
        }
    }
}
