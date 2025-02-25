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

    [TestMethod]
    public void SwitchTest1()
    {
        var data = EmbeddedData.FromBytes([]);
        data.Switch();
    }

    [TestMethod]
    public void SwitchTest2()
    {
        var data = EmbeddedData.FromText("");
        data.Switch();
    }

    [TestMethod]
    public void SwitchTest3()
    {
        var data = EmbeddedData.FromBytes([]);
        data.Switch(true);
    }

    [TestMethod]
    public void SwitchTest4()
    {
        var data = EmbeddedData.FromText("");
        data.Switch(true);
    }

    [TestMethod]
    public void SwitchTest5()
    {
        bool b = false;
        var data = EmbeddedData.FromBytes([]);
        data.Switch(bytesAction: bt => b = true);
        Assert.IsTrue(b);
    }

    [TestMethod]
    public void SwitchTest6()
    {
        bool b = false;

        var data = EmbeddedData.FromText("");
        data.Switch(textAction: str => b = true);
        Assert.IsTrue(b);
    }

    [TestMethod]
    public void SwitchTest7()
    {
        bool[] b = [false];

        var data = EmbeddedData.FromBytes([]);
        data.Switch(b, bytesAction: (bt, bl) => bl[0] = true);
        Assert.IsTrue(b[0]);
    }

    [TestMethod]
    public void SwitchTest8()
    {
        bool[] b = [false];
        var data = EmbeddedData.FromText("");
        data.Switch(b, textAction: static (str, bl) => bl[0] = true);
        Assert.IsTrue(b[0]);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest1()
    {
        var data = EmbeddedData.FromBytes([]);
        _ = data.Convert<bool>(null!, null!);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest2()
    {
        var data = EmbeddedData.FromText("");
        _ = data.Convert<bool>(null!, null!);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest3()
    {
        var data = EmbeddedData.FromBytes([]);
        _ = data.Convert<bool, bool>(true, null!, null!);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest4()
    {
        var data = EmbeddedData.FromText("");
        _ = data.Convert<bool, bool>(true, null!, null!);
    }

    [TestMethod]
    public void ConvertTest5()
    {
        bool b = false;
        var data = EmbeddedData.FromBytes([]);
        b = data.Convert<bool>(bt => true, str => false);
        Assert.IsTrue(b);
    }

    [TestMethod]
    public void ConvertTest6()
    {
        bool b = false;

        var data = EmbeddedData.FromText("");
        b = data.Convert<bool>(bt => false, str => true);
        Assert.IsTrue(b);
    }

    [TestMethod]
    public void ConvertTest7()
    {
        bool b = false;

        var data = EmbeddedData.FromBytes([]);
        b = data.Convert<bool, bool>(b, static (bt, bl) => !bl, static (str, bl) => bl);
        Assert.IsTrue(b);
    }

    [TestMethod]
    public void ConvertTest8()
    {
        bool b = false;

        var data = EmbeddedData.FromText("");
        b = data.Convert<bool, bool>(b, static (bt, bl) => bl, static (str, bl) => !bl);
        Assert.IsTrue(b);
    }

    [TestMethod]
    public void ToStringTest1()
    {
        string s = default(EmbeddedData).ToString();
        Assert.IsNotNull(s);
    }

    [TestMethod]
    public void ToStringTest2()
    {
        string s = EmbeddedData.FromText("").ToString();
        Assert.IsNotNull(s);
    }
}
