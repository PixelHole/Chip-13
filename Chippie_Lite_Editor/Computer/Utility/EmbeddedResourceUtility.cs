using System.IO;

namespace Chippie_Lite_WPF.Computer.Utility;

public static class EmbeddedResourceUtility
{
    public static T DeserializeEmbeddedResource<T>(string name)
    {
        string raw = ReadEmbeddedResource(name);
        return JsonFileUtility.Deserialize<T>(raw);
    }
    public static string ReadEmbeddedResource(string name)
    {
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        
        Stream? stream = assembly.GetManifestResourceStream(name);
        if (stream == null) throw new FileNotFoundException();

        StreamReader reader = new StreamReader(stream);
        string data = reader.ReadToEnd();
        
        reader.Close();
        reader.Dispose();

        return data;
    }
}