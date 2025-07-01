namespace Chippie_Lite.Computer.Instructions;

public struct Instruction
{
    public string Header { get; private set; }
    public IList<InstructionArgument> Arguments { get; private set; }


    public Instruction(string header, IList<InstructionArgument> arguments)
    {
        Header = header;
        Arguments = arguments;
    }
    public Instruction(string header) : this(header, []) {}


    public override string ToString()
    {
        return Header;
    }
}