using System.Text;

namespace Chippie_Lite_WPF.Computer.Utility;

public static class SerialUtility
{
    public static long[] AsciiToLongs(string input)
    {
        if (NumberUtility.TryParseNumber(input, out long number))
        {
            return [number];
        }
        
        int count = (int)Math.Ceiling((double)input.Length / 8);
                
        long[] res = new long[count];
                
        for (int i = 0; i < count; i++)
        {
            string raw = input[..Math.Min(8, input.Length)];
            input = input.Remove(0, Math.Min(8, input.Length));
        
            long num = WordToLong(raw);
            res[i] = num;
        }
        
        return res;
    }
    public static string LongsToAscii(long[] nums)
    {
        StringBuilder builder = new StringBuilder();

        foreach (var num in nums)
        {
            builder.Append(LongToWord(num));
        }

        return builder.ToString();
    }
    
    public static long WordToLong(string input)
    {
        long res = 0;
        
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '\0') break;
            res <<= 8;
            res += input[i];
            if (i == 7) break;
        }
        
        return res;
    }

    public static string LongToWord(long num)
    {
        StringBuilder res = new StringBuilder();
                
        for (int i = 0; i < 8; i++)
        {
            char c = (char)(num % 256);
            if (c == 0) break;
            res.Insert(0, c);
            num >>= 8;
        }
        
        return res.ToString();
    }
}