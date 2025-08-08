using System.Text;

namespace Chippie_Lite_WPF.Computer.Utility;

public static class SerialUtility
{
    public static int[] AsciiToInts(string input)
    {
        if (NumberUtility.TryParseNumber(input, out int number))
        {
            return [number];
        }
        
        int count = (int)Math.Ceiling((double)input.Length / 4);
                
        int[] res = new int[count];
                
        for (int i = 0; i < count; i++)
        {
            string raw = input[..Math.Min(4, input.Length)];
            input = input.Remove(0, Math.Min(4, input.Length));
        
            int num = WordToInt(raw);
            res[i] = num;
        }
        
        return res;
    }
    public static string IntsToAscii(int[] nums)
    {
        StringBuilder builder = new StringBuilder();

        foreach (var num in nums)
        {
            builder.Append(IntToWord(num));
        }

        return builder.ToString();
    }
    
    public static int WordToInt(string input)
    {
        int res = 0;
        
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '\0') break;
            res *= 256;
            res += input[i];
            if (i == 3) break;
        }
        
        return res;
    }

    public static string IntToWord(int num)
    {
        StringBuilder res = new StringBuilder();
                
        for (int i = 0; i < 4; i++)
        {
            char c = (char)(num % 256);
            if (c == 0) break;
            res.Insert(0, c);
            num /= 256;
        }
        
        return res.ToString();
    }
}