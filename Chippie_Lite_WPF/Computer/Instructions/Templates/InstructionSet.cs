using System.Text;

namespace Chippie_Lite_WPF.Computer.Instructions.Templates;

public static class InstructionSet
{
    private static List<InstructionTemplate> Templates { get; set; } = new List<InstructionTemplate>(new []
    {
        new InstructionTemplate("Add",
            "add",
            [
                InstructionArgumentType.RegisterAndNumber, InstructionArgumentType.RegisterAndNumber,
                InstructionArgumentType.RegisterAndNumber
            ],
            [
                new InstructionAction("Add", [0, 1, 2])
            ])
    });


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
}