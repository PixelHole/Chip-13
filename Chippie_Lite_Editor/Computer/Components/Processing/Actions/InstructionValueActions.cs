using System.Numerics;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Arguments;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.Computer.Components.Actions;

public static class InstructionValueActions
{
    internal static void Add(InstructionAction action)
    {
        var a = ActionUtility.InterpretRead(action.Arguments[1], action.Instruction);
        var b = ActionUtility.InterpretRead(action.Arguments[2], action.Instruction);
        
        ActionUtility.InterpretWrite(action.Arguments[0], a + b, action.Instruction);
    }
    internal static void Multiply(InstructionAction action)
    {
        var a = ActionUtility.InterpretRead(action.Arguments[2], action.Instruction);
        var b = ActionUtility.InterpretRead(action.Arguments[3], action.Instruction);

        BigInteger result = a * b;

        Register high = ActionUtility.GetRegister(action.Arguments[0], action.Instruction);
        Register low = ActionUtility.GetRegister(action.Arguments[1], action.Instruction);
        
        low.Content = (long)result;
        high.Content = (long)(result >> low.LengthLimit);
    }
    internal static void Power(InstructionAction action)
    {
        var a = ActionUtility.InterpretRead(action.Arguments[2], action.Instruction);
        var b = ActionUtility.InterpretRead(action.Arguments[3], action.Instruction);

        BigInteger result = BigInteger.Pow(a, (int)b);

        Register high = ActionUtility.GetRegister(action.Arguments[0], action.Instruction);
        Register low = ActionUtility.GetRegister(action.Arguments[1], action.Instruction);

        low.Content = (long)result;
        high.Content = (long)(result >> low.LengthLimit);
    }
    internal static void Divide(InstructionAction action)
    {
        var a = ActionUtility.InterpretRead(action.Arguments[1], action.Instruction);
        var b = ActionUtility.InterpretRead(action.Arguments[2], action.Instruction);
        
        ActionUtility.InterpretWrite(action.Arguments[0], a / b, action.Instruction);
    }
    internal static void Remainder(InstructionAction action)
    {
        var a = ActionUtility.InterpretRead(action.Arguments[1], action.Instruction);
        var b = ActionUtility.InterpretRead(action.Arguments[2], action.Instruction);
        
        ActionUtility.InterpretWrite(action.Arguments[0], a % b, action.Instruction);
    }
    internal static void ShiftLeftLogic(InstructionAction action)
    {
        var value = ActionUtility.InterpretRead(action.Arguments[1], action.Instruction);
        var amount = ActionUtility.InterpretRead(action.Arguments[2], action.Instruction);
        
        ActionUtility.InterpretWrite(action.Arguments[0], value << (int)amount, action.Instruction);
    }
    internal static void ShiftRightLogic(InstructionAction action)
    {
        var value = ActionUtility.InterpretRead(action.Arguments[1], action.Instruction);
        var amount = ActionUtility.InterpretRead(action.Arguments[2], action.Instruction);
        
        ActionUtility.InterpretWrite(action.Arguments[0], value >> (int)amount, action.Instruction);
    }
    internal static void ShiftLeftArithmatic(InstructionAction action)
    {
        var value = ActionUtility.InterpretRead(action.Arguments[1], action.Instruction);
        var amount = ActionUtility.InterpretRead(action.Arguments[2], action.Instruction);
        
        ActionUtility.InterpretWrite(action.Arguments[0], BitMath.ShiftLeft(value, (int)amount, true), action.Instruction);
    }
    internal static void ShiftRightArithmatic(InstructionAction action)
    {
        var value = ActionUtility.InterpretRead(action.Arguments[1], action.Instruction);
        var amount = ActionUtility.InterpretRead(action.Arguments[2], action.Instruction);
        
        ActionUtility.InterpretWrite(action.Arguments[0], value >>> (int)amount, action.Instruction);
    }
}