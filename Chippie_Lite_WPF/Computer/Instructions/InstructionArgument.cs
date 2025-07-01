using Chippie_Lite.Computer.Components.Registers;

namespace Chippie_Lite.Computer.Instructions;

public struct InstructionArgument
{
    private int Value { get; set; }
    private Register? Register { get; set; }


    public InstructionArgument(int value = 0, Register? register = null)
    {
        Value = value;
        Register = register;
    }

    public int GetValue()
    {
        if (Register == null) return Value;
        return Value + Register.Content;
    }
    public Register GetRegister()
    {
        int index = Value;
        if (Register != null) index += RegisterBank.IndexOf(Register);
        return RegisterBank.GetRegister(index);
    }


    public override string ToString()
    {
        string reg = Register == null ? "None" : Register.Name;
        return $"{reg} : {Value}";
    }
}