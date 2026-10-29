namespace Chippie_Lite_WPF.Computer.Utility;

public static class StringUtility
{
    public static long TextToInt(string text)
    {
        if (IsString(text)) return StringToLong(text);
        if (IsChar(text)) return CharToInt(text);
        throw new ArgumentException();
    }

    private static long StringToLong(string text)
    {
        string trimmed = text.Substring(1, int.Min(4, text.Length - 2));

        return SerialUtility.WordToLong(trimmed);
    }
    private static long CharToInt(string text)
    {
        string trimmed = text[1].ToString();

        return SerialUtility.WordToLong(trimmed);
    }

    public static bool IsString(string text)
    {
        return text.StartsWith('\"') && text.EndsWith('\"') && text.Length <= 6;
    }
    public static bool IsChar(string text)
    {
        return text.StartsWith('\'') && text.EndsWith('\'') && text.Length == 3;
    }
    public static bool IsStringOrChar(string text) => IsString(text) || IsChar(text);
}