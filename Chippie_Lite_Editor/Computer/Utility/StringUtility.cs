namespace Chippie_Lite_WPF.Computer.Utility;

public static class StringUtility
{
    public static int TextToInt(string text)
    {
        if (IsString(text)) return StringToInt(text);
        if (IsChar(text)) return CharToInt(text);
        throw new ArgumentException();
    }

    private static int StringToInt(string text)
    {
        string trimmed = text.Substring(1, int.Min(4, text.Length - 2));

        return SerialUtility.WordToInt(trimmed);
    }
    private static int CharToInt(string text)
    {
        string trimmed = text[1].ToString();

        return SerialUtility.WordToInt(trimmed);
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