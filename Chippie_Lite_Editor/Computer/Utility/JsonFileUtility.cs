using System.IO;
using System.Text.Json;
using Chippie_Lite_WPF.Computer.File;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.File;

namespace Chippie_Lite_WPF.Computer.Utility;

public static class JsonFileUtility
{
    public static T ReadAndDeserialize<T>(string path)
    {
        StreamReader reader;
        string json;
        T result;
        try
        {
            reader = new StreamReader(path);
        }
        catch (Exception e)
        {
            throw new FileOpenException(e);
        }

        try
        {
            json = reader.ReadToEnd();
        }
        catch (Exception e)
        {
            throw new FileReadException(e);
        }

        reader.Dispose();
        reader.Close();
        
        try
        {
            result = JsonSerializer.Deserialize<T>(json, JsonSerializerOptions.Web) ?? throw new ArgumentNullException();
        }
        catch (Exception e)
        {
            throw new FileParseException(e);
        }

        return result;
    }
    public static void SerializeAndWrite(object obj, string path)
    {
        string json = JsonSerializer.Serialize(obj, JsonSerializerOptions.Web);
        StreamWriter writer;
          
        try
        {
            writer = new StreamWriter(path);
        }
        catch (Exception e)
        {
            throw new FileOpenException(e);
        }
          
        try
        {
            writer.Write(json);
        }
        catch (Exception e)
        {
            throw new FileWriteException(e);
        }
          
        writer.Flush();
        writer.Close();
    }
}