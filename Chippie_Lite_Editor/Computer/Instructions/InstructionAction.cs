using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Chippie_Lite_WPF.Computer.Instructions;

public struct InstructionAction : IEquatable<InstructionAction>
{
    public string Header { get; private set; }
    public int[] Indices { get; private set; }


    [JsonConstructor]
    public InstructionAction(string header, int[] indices)
    {
        Header = header;
        Indices = indices;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is InstructionAction other) return Equals(other);
        return base.Equals(obj);
    }
    public bool Equals(InstructionAction other)
    {
        return Header == other.Header && Indices.Equals(other.Indices);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Header, Indices);
    }
}