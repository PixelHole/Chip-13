namespace Chippie_Lite_WPF.Computer.Utility;

public static class BitMath
{
    public static int ShiftLeft(int number, int amount, bool fill = false)
    {
        int shift = (int)Math.Pow(2, amount);
        int result = number * shift;
        if (fill) result += shift - 1;
        return result;
    }
    public static int ShiftRight(int number, int amount, bool fill = false)
    {
        int shift = (int)Math.Pow(2, amount);
        int result = number / shift;
        if (fill) result += (shift - 1) * (int)Math.Pow(2, 32 - amount);
        return result;
    }
    public static int Not(int a)
    {
        return Int32.MaxValue - a;
    }
    public static int And(int a, int b)
    {
        return a & b;
    }
    public static int Nand(int a, int b)
    {
        return Not(And(a, b));
    }
    public static int Or(int a, int b)
    {
        return a | b;
    }
    public static int Nor(int a, int b)
    {
        return Not(Or(a, b));
    }
    public static int Xor(int a, int b)
    {
        return And(Nand(a, b), Or(a, b));
    }
    public static int Nxor(int a, int b)
    {
        return Or(Nor(a, b), And(a, b));
    }
}