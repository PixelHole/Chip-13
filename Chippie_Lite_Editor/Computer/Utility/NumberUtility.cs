using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Chippie_Lite_WPF.Computer.Utility;

public static class NumberUtility
{
    public static bool TryParseNumber(string input, out long num)
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
    public static long ParseNumber(string num)
    {
        num = num.ToLower();
        if ((num.EndsWith('b') && !num.StartsWith("0x")) || num.StartsWith("0b") || num.StartsWith("0y")) return ParseBinary(num);
        if (num.StartsWith("0x") || num.StartsWith("0h") || num.StartsWith("0c")) return ParseHex(num);
        return ParseDecimal(num);
    }
    public static long ParseBinary(string num)
    {
        num = RemoveFlags(num, "b", "0b", "0y", "_", "-");
        return long.Parse(num, NumberStyles.BinaryNumber);
    }
    public static long ParseHex(string num)
    {
        num = RemoveFlags(num, "0x", "0h", "_", "-");
        return long.Parse(num, NumberStyles.HexNumber);
    }
    public static long ParseDecimal(string num)
    {
        num = RemoveFlags(num, "d", "0d", "d");
        return long.Parse(num);
    }

    public static string ToBinary(long num, int length = 64, bool separate = false, int separationLength = 8)
    {
        string converted = Convert.ToString(num, 2).PadLeft(length, '0');

        int firstLength = converted.Length % separationLength;
        if (firstLength == 0) firstLength = separationLength;

        StringBuilder result = new StringBuilder(converted[..firstLength]);

        for (int i = firstLength; i < converted.Length; i += separationLength)
        {
            result.Append('-');
            result.Append(converted.Substring(firstLength, separationLength));
        }
        
        return result.ToString();
    }
    public static string ToHex(long num, int length = 64, bool separate = false, int separationLength = 4)
    {
        var converted = Convert.ToString(num, 16).PadLeft(length / 8, '0');

        int firstLength = converted.Length % separationLength;
        if (firstLength == 0) firstLength = separationLength;

        StringBuilder result = new StringBuilder(converted[..firstLength]);

        for (int i = firstLength; i < converted.Length; i += separationLength)
        {
            result.Append('-');
            result.Append(converted.Substring(firstLength, separationLength));
        }

        return result.ToString();
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