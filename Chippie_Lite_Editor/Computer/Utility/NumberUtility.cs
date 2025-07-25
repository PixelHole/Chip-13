using System.Globalization;

namespace Chippie_Lite_WPF.Computer.Utility;

public static class NumberUtility
{
    public static bool TryParseNumber(string input, out int num)
    {
        try
        {
            num = ParseNumber(input);
            return true;
        }
        catch (Exception)
        {
            num = -1;
            return false;
        }
    }
    public static int ParseNumber(string num)
    {
        num = num.ToLower();
        if (num.EndsWith('b') || num.StartsWith("0b") || num.StartsWith("0y")) return ParseBinary(num);
        if (num.StartsWith("0x") || num.StartsWith("0h") || num.StartsWith("0c")) return ParseHex(num);
        return ParseDecimal(num);
    }
    public static int ParseBinary(string num)
    {
        num = RemoveFlags(num, "b", "0b", "0y", "_", "-");
        return int.Parse(num, NumberStyles.BinaryNumber);
    }
    public static int ParseHex(string num)
    {
        num = RemoveFlags(num, "0x", "0h", "_", "-");
        return int.Parse(num, NumberStyles.HexNumber);
    }
    public static int ParseDecimal(string num)
    {
        num = RemoveFlags(num, "d", "0d", "d");
        return int.Parse(num);
    }

    public static string ToBinary(int num, bool separate = false)
    {
        string result = Convert.ToString(num, 2).PadLeft(32, '0') + "b";
        if (separate)
        {
           result = $"{result.Substring(0, 8)}-{result.Substring(8, 8)}-{result.Substring(16, 8)}-{result.Substring(24, 8)}b";
        }

        return result;
    }
    public static string ToHex(int num, bool separate = false)
    {
        string result = "0x" + Convert.ToString(num, 16).PadLeft(8, '0');

        if (separate)
        {
            result = $"{result.Substring(0, 6)}-{result.Substring(6, 4)}";
        }

        return result;
    }

    private static string RemoveFlags(string str, params string[] flags)
    {
        str = str.ToLower();
        
        foreach (var flag in flags)
        {
            str = str.Replace(flag, "");
        }

        return str;
    }
}