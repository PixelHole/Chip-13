using System.Globalization;

namespace Chippie_Lite.Internal;

public static class NumberUtility
{
    public static bool TryParseNumber(string input, out int num)
    {
        try
        {
            num = ParseNumber(input);
            return true;
        }
        catch (Exception e)
        {
            num = -1;
            return false;
        }
    }
    public static int ParseNumber(string num)
    {
        if (num.EndsWith('b') || num.StartsWith("0b") || num.StartsWith("0y")) return ParseBinary(num);
        if (num.StartsWith("0x") || num.StartsWith("0h") || num.StartsWith("0c")) return ParseHex(num);
        return ParseDecimal(num);
    }
    public static int ParseBinary(string num)
    {
        num = RemoveFlags(num, "b", "0b", "0y", "_");
        return int.Parse(num, NumberStyles.BinaryNumber);
    }
    public static int ParseHex(string num)
    {
        num = RemoveFlags(num, "0x", "0h", "0x", "_");
        return int.Parse(num, NumberStyles.HexNumber);
    }
    public static int ParseDecimal(string num)
    {
        num = RemoveFlags(num, "d", "0d", "d");
        return int.Parse(num);
    }

    private static string RemoveFlags(string str, params string[] flags)
    {
        foreach (var flag in flags)
        {
            str = str.Replace(flag, "");
        }

        return str;
    }
}