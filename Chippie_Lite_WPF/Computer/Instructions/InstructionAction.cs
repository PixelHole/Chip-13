using System.Text.Json.Serialization;

namespace Chippie_Lite_WPF.Computer.Instructions;

public struct InstructionAction
{
    public string Header { get; private set; }
    public int[] Indices { get; private set; }


    [JsonConstructor]
    public InstructionAction(string header, int[] indices)
    {
        Header = header;
        Indices = indices;
    }
}