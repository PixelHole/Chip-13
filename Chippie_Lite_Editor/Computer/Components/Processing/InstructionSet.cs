using System.IO;
using System.Text;
using System.Text.Json;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Templates;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.File;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.Computer.Components;

public static class InstructionSet
{
    private static List<InstructionTemplate> Templates { get; set; } =
    [
        new InstructionTemplate("Add",
            "add",
            [
                InstructionArgumentType.Register, InstructionArgumentType.RegisterAndValue,
                InstructionArgumentType.RegisterAndValue
            ],
            [
                new InstructionAction("Add", [0, 1, 2])
            ]),
        new InstructionTemplate("Subtract",
            "sub",
            [
                InstructionArgumentType.Register, InstructionArgumentType.RegisterAndValue,
                InstructionArgumentType.RegisterAndValue
            ],
            [
                new InstructionAction("subtract", [0, 1, 2])
            ]),
        new InstructionTemplate("shift left",
            "sll",
            [
                InstructionArgumentType.Register, InstructionArgumentType.RegisterAndValue,
                InstructionArgumentType.RegisterAndValue
            ],
            [
                new InstructionAction("shift left logic", [0, 1, 2])
            ]),
        new InstructionTemplate("shift right",
            "srl",
            [
                InstructionArgumentType.Register, InstructionArgumentType.RegisterAndValue,
                InstructionArgumentType.RegisterAndValue
            ],
            [
                new InstructionAction("shift right logic", [0, 1, 2])
            ]),
        new InstructionTemplate("serial input",
            "inp",
            [
                InstructionArgumentType.Register
            ],
            [
                new InstructionAction("serial input", [0])
            ]),
        new InstructionTemplate("serial output",
            "out",
            [
                InstructionArgumentType.RegisterAndValue,
            ],
            [
                new InstructionAction("serial output", [0])
            ])
    ];

    public static readonly string SavePath = "InstructionSet.json"; 
    
    public static InstructionTemplate? FindTemplate(string header)
    {
        return FindTemplate(temp => temp.Header == header);
    }
    public static InstructionTemplate? FindTemplate(Predicate<InstructionTemplate> predicate)
    {
        return Templates.Find(predicate);
    }

    public static string GetHeadersCsv()
    {
        StringBuilder csv = new StringBuilder();

        for (int i = 0; i < Templates.Count; i++)
        {
            string header = Templates[i].Header;

            csv.Append(header);

            if (i < Templates.Count - 1 && !string.IsNullOrEmpty(header)) csv.Append(',');
        }

        return csv.ToString();
    }
    
    public static void SaveSet(string path)
    {
        JsonFileUtility.SerializeAndWrite(Templates, path);
    }
    public static void LoadSet(string path)
    {
        Templates = JsonFileUtility.ReadAndDeserialize<List<InstructionTemplate>>(path);
    }
}