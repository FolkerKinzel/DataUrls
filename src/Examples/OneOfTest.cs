using System.Text.RegularExpressions;
using OneOf;
using OneOf.Types;

namespace Examples;

internal static partial class OneOfTest
{
    [GeneratedRegex(@"\s+", RegexOptions.None, 500, "en-US")]
    private static partial Regex WhiteSpace();

    internal static void Test()
    {
        var one = GiveOneOf();
        string? s = one.Value as string;

        s = "H e l l o     W o r l d";

        s = RemoveWhiteSpace(s);

        object o = default(OneOf<string, byte[]>).Value;

        o = default(OneOf<string, int>).Value;

    }

    internal static OneOf<string, byte[]> GiveOneOf() => "Hi";


    private static string RemoveWhiteSpace(string s)
    {
        return WhiteSpace().Replace(s, "");
    }


}

[GenerateOneOf]
public partial class OneOfReturn : OneOfBase<string, byte[]> { }
