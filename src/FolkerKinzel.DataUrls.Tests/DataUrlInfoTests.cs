using FolkerKinzel.DataUrls.Intls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace FolkerKinzel.DataUrls.Tests;

[TestClass]
public class DataUrlInfoTests
{
    [TestMethod]
    public void IsEmptyTest1() => Assert.IsTrue(DataUrlInfo.Empty.IsEmpty);

    [TestMethod]
    public void IsEmptyTest2()
    {
        _ = DataUrl.TryParse("data:,abc", out DataUrlInfo dataUrl);
        Assert.IsFalse(dataUrl.IsEmpty);
    }

 
    [TestMethod]
    public void GetFileTypeExtensionTest()
    {
        Assert.IsTrue(DataUrl.TryParse("data:,abc", out DataUrlInfo dataUrl));
        Assert.AreEqual(".txt", dataUrl.GetFileTypeExtension());
    }


    [TestMethod]
    public void TryGetEmbeddedTextTest1()
    {
        Assert.IsTrue(DataUrl.TryParse("data:;base64,A", out DataUrlInfo info));
        Assert.IsFalse(info.TryGetEmbeddedText(out _));
    }

    [TestMethod]
    public void TryGetEmbeddedTextTest2()
    {
        Assert.IsTrue(DataUrl.TryParse("data:text/plain;charset=utf-8;base64,ABCD", out DataUrlInfo info));
        Assert.IsFalse(info.TryGetEmbeddedText(out _));
    }

    [TestMethod]
    public void TryGetEmbedddedTextTest3() => Assert.IsTrue(new DataUrlInfo().TryGetEmbeddedText(out _));


    [TestMethod]
    public void TryGetEmbedddedBytesTest1() => Assert.IsFalse(new DataUrlInfo().TryGetEmbeddedBytes(out _));

    [TestMethod]
    public void TryGetEmbeddedBytesTest2()
    {
        Assert.IsTrue(DataUrl.TryParse("data:application/octet-stream;base64,A", out DataUrlInfo info));
        Assert.IsFalse(info.TryGetEmbeddedBytes(out _));
    }


    [TestMethod]
    public void PropertiesTest1()
    {
        const string input = "data:,This is unescaped ASCII text.";

        Assert.IsTrue(DataUrl.TryParse(input, out DataUrlInfo info));
        Assert.IsTrue(info.ContainsEmbeddedText);
        Assert.IsFalse(info.ContainsEmbeddedBytes);
    }


    [TestMethod]
    public void EqualsTest1()
    {
        const string input = "Märchenbücher";
        string urlStr1 = DataUrl.FromText(input);

        const string encodingName = "iso-8859-1";
        Encoding encoding = TextEncodingConverter.GetEncoding(encodingName);

        byte[]? bytes = encoding.GetBytes(input);

        //var mime = MimeType.Parse($"text/plain; charset={encodingName}");
        string urlStr2 = DataUrl.FromBytes(bytes, $"text/plain; charset={encodingName}");

        Assert.IsTrue(DataUrl.TryParse(urlStr1, out DataUrlInfo dataUrl1));
        Assert.IsTrue(DataUrl.TryParse(urlStr2, out DataUrlInfo dataUrl2));

        Assert.IsTrue(dataUrl1 == dataUrl2);
        Assert.IsFalse(dataUrl1 != dataUrl2);
        Assert.AreEqual(dataUrl1.GetHashCode(), dataUrl2.GetHashCode());

        Assert.IsTrue(dataUrl1.Equals(dataUrl2));

        object? o1 = dataUrl1;
        object? o2 = dataUrl2;

        //Assert.IsTrue(dataUrl1 == o2);
        //Assert.IsFalse(o1 != o2);
        Assert.AreEqual(o1.GetHashCode(), o2.GetHashCode());
    }


    [TestMethod]
    public void EqualsTest2()
    {
        string input = "xyz";
        string urlStr1 = $"data:application/octet-stream,{input}";
        string urlStr2 = $"data:application/octet-stream;base64,{Convert.ToBase64String(Encoding.ASCII.GetBytes(input))}";

        Assert.IsTrue(DataUrl.TryParse(urlStr1, out DataUrlInfo dataUrl1));
        Assert.IsTrue(DataUrl.TryParse(urlStr2, out DataUrlInfo dataUrl2));

        Assert.IsTrue(dataUrl1 == dataUrl2);
    }


    [TestMethod]
    public void EqualsTest3() => Assert.IsFalse(new DataUrlInfo().Equals(""));


    [TestMethod]
    public void EqualsTest4()
    {
        Assert.IsTrue(DataUrl.TryParse("data:,ABCD", out DataUrlInfo info1));
        Assert.IsTrue(DataUrl.TryParse("data:application/octet-stream;base64,ABCD", out DataUrlInfo info2));

        Assert.IsFalse(info1.Equals(info2));
        Assert.IsFalse(info2.Equals(info1));
    }

    [TestMethod]
    public void EqualsTest5()
    {
        Assert.IsTrue(DataUrl.TryParse("data:,A", out DataUrlInfo info1));
        Assert.IsTrue(DataUrl.TryParse("data:;base64,A", out DataUrlInfo info2));

        Assert.IsFalse(info2.Equals(info1));
    }

    [TestMethod]
    public void EqualsTest6()
    {
        Assert.IsTrue(DataUrl.TryParse("data:application/octet-stream;base64,A", out DataUrlInfo info1));
        Assert.IsTrue(DataUrl.TryParse("data:application/octet-stream,%01%02%03", out DataUrlInfo info2));

        Assert.IsFalse(info1.Equals(info2));
        Assert.IsFalse(info2.Equals(info1));
    }


    [TestMethod]
    public void EqualsTest7() => Assert.AreNotEqual<object?>(DataUrlInfo.Empty, 42);

    [TestMethod]
    public void EqualsTest8() => Assert.AreEqual(DataUrlInfo.Empty, (object)DataUrlInfo.Empty);


    [TestMethod]
    public void EqualsTest9()
    {
        Assert.IsTrue(DataUrl.TryParse("data:,A", out DataUrlInfo info));
        Assert.AreNotEqual(DataUrlInfo.Empty, (object)info);
    }

    [TestMethod]
    public void EqualsTest9b()
    {
        Assert.IsTrue(DataUrl.TryParse("data:,A", out DataUrlInfo info));
        Assert.AreNotEqual((object)info, DataUrlInfo.Empty);
    }

    [TestMethod]
    public void EqualsTest10()
    {
        Assert.IsTrue(DataUrl.TryParse("data:,A", out DataUrlInfo info1));
        Assert.IsTrue(DataUrl.TryParse("data:quark,A", out DataUrlInfo info2));
        Assert.AreNotEqual(info1, info2);
    }

    [TestMethod]
    public void EqualsTest10b()
    {
        Assert.IsTrue(DataUrl.TryParse("data:quark,A", out DataUrlInfo info1));
        Assert.IsTrue(DataUrl.TryParse("data:,A", out DataUrlInfo info2));
        Assert.AreNotEqual(info1, info2);
    }

    [TestMethod]
    public void EqualsTest11()
    {
        Assert.IsTrue(DataUrl.TryParse("data:,A", out DataUrlInfo info1));
        Assert.IsTrue(DataUrl.TryParse("data:text/plain;charset=utf-8,A", out DataUrlInfo info2));
        Assert.AreEqual(info1, info2);
    }

    

    [TestMethod]
    public void EqualsTest12()
    {
        Assert.IsTrue(DataUrl.TryParse("data:text/html,A", out DataUrlInfo info1));
        Assert.IsTrue(DataUrl.TryParse("data:text/plain;charset=utf-8,A", out DataUrlInfo info2));
        Assert.AreNotEqual(info1, info2);
    }

    [TestMethod]
    public void EqualsTest13()
    {
        Assert.IsTrue(DataUrl.TryParse("data:quark,A", out DataUrlInfo info1));
        Assert.IsTrue(DataUrl.TryParse("data:blödelblubb,A", out DataUrlInfo info2));
        Assert.AreEqual(info1, info2);
    }

    [TestMethod]
    public void EqualsTest14()
    {
        Assert.IsTrue(DataUrl.TryParse("data:,A", out DataUrlInfo info1));
        Assert.IsTrue(DataUrl.TryParse("data:,B", out DataUrlInfo info2));
        Assert.AreNotEqual(info1, info2);
    }

    [TestMethod]
    public void EqualsTest15()
    {
        Assert.IsTrue(DataUrl.TryParse("data:application/octet-stream;base64,1234", out DataUrlInfo info1));
        Assert.IsTrue(DataUrl.TryParse("data:application/octet-stream;base64,1234", out DataUrlInfo info2));
        Assert.AreEqual(info1, info2);
    }

    [TestMethod]
    public void CloneTest1()
    {
        Assert.IsTrue(DataUrl.TryParse("data:,xyz", out DataUrlInfo dtInfo));
        ICloneable info = dtInfo;

        object dataUrl2 = info.Clone();

        Assert.IsTrue(((DataUrlInfo)info) == (dataUrl2 as DataUrlInfo?));

    }

    [TestMethod]
    public void CloneTest2()
    {
        var info = (DataUrlInfo)new DataUrlInfo().Clone();
        Assert.IsTrue(info.IsEmpty);
    }

    [TestMethod]
    public void GetHashCodeTest1()
    {
        Assert.IsTrue(DataUrl.TryParse("data:application/octet-stream;base64,ABCD", out DataUrlInfo info2));
        Assert.AreNotEqual(new DataUrlInfo().GetHashCode(), info2.GetHashCode());
    }


    

}
