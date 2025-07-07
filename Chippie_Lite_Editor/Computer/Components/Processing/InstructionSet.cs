using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chippie_Lite_WPF.Computer.Instructions.Templates;

public static class InstructionSet
{
    private static List<InstructionTemplate> Templates { get; set; } =
    [
        new InstructionTemplate("Add",
            "add",
            [
                InstructionArgumentType.Register, InstructionArgumentType.RegisterAndNumber,
                InstructionArgumentType.RegisterAndNumber
            ],
            [
                new InstructionAction("Add", [0, 1, 2])
            ]),
        new InstructionTemplate("Subtract",
            "sub",
            [
                InstructionArgumentType.Register, InstructionArgumentType.RegisterAndNumber,
                InstructionArgumentType.RegisterAndNumber
            ],
            [
                new InstructionAction("subtract", [0, 1, 2])
            ]),
        new InstructionTemplate("shift left",
            "sll",
            [
                InstructionArgumentType.Register, InstructionArgumentType.RegisterAndNumber,
                InstructionArgumentType.RegisterAndNumber
            ],
            [
                new InstructionAction("shift left logic", [0, 1, 2])
            ]),
        new InstructionTemplate("shift right",
            "srl",
            [
                InstructionArgumentType.Register, InstructionArgumentType.RegisterAndNumber,
                InstructionArgumentType.RegisterAndNumber
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
                InstructionArgumentType.RegisterAndNumber,
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
        string json = JsonSerializer.Serialize(Templates, JsonSerializerOptions.Web);
        
        StreamWriter writer = new StreamWriter(path);

        writer.Write(json);
        
        writer.Close();
        writer.Dispose();
    }
    public static void LoadSet(string path)
    {
        StreamReader reader = new StreamReader(path);

        string json = reader.ReadToEnd();

        Templates = JsonSerializer.Deserialize<List<InstructionTemplate>>(json, JsonSerializerOptions.Web);
        
        reader.Dispose();
        reader.Close();
    }
}