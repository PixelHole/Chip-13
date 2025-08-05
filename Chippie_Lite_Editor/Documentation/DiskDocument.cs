using System.IO;
using System.Text;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.File;

namespace Chippie_Lite_WPF.Documentation;

public class DiskDocument
{
    public string Name { get; private set; }
    public string[] Address { get; private set; }
    private string FilePath { get; set; }
    public bool IsOpen { get; private set; }

    private StringBuilder ContentBuffer { get; set; }

    public delegate void OpenedAction();
    public event OpenedAction? OnOpened;
    
    public delegate void ClosedAction();
    public event ClosedAction? OnClosed;
    
    
    private DiskDocument(string name, string[] address, string filePath)
    {
        Name = name;
        Address = address;
        FilePath = filePath;
        IsOpen = false;
        ContentBuffer = new StringBuilder();
    }
    
    public string Read()
    {
        if (IsOpen) return ContentBuffer.ToString();
        
        StreamReader reader;

        try
        {
            reader = new StreamReader(FilePath);
        }
        catch (Exception e)
        {
            throw new FileReadException(e);
        }

        try
        {
            string? line = reader.ReadLine();

            while (line != null)
            {
                if (!IsLineAddressProperty(line) && !IsLineTitleProperty(line) && !IsLineDocumentSignature(line))
                    ContentBuffer.AppendLine(line);
                line = reader.ReadLine();
            }
        }
        catch (Exception e)
        {
            throw new FileReadException(e);
        }

        reader.Dispose();
        reader.Close();

        IsOpen = true;
        
        return ContentBuffer.ToString();
    }
    public void Flush()
    {
        ContentBuffer.Clear();
        IsOpen = false;
    }
    
    public static DiskDocument? FromFile(string path)
    {
        StreamReader reader;

        try
        {
            reader = new StreamReader(path);
        }
        catch (Exception e)
        {
            throw new FileReadException(e);
        }
        
        try
        {
            string? signature = reader.ReadLine();
            if (signature == null || !IsLineDocumentSignature(signature)) return null;
        }
        catch (Exception e)
        {
            throw new FileReadException(e);
        }
        
        string title = "";
        string[] address = [];
        
        while (string.IsNullOrEmpty(title) || address.Length == 0)
        {
            string? line;
            
            try
            {
                line = reader.ReadLine();
            }
            catch (Exception e)
            {
                throw new FileReadException(e);
            }
            
            if (line == null) break;
            
            if (IsLineTitleProperty(line))
            {
                title = ExtractTitleFromLine(line);
                continue;
            }
            if (IsLineAddressProperty(line)) address = ExtractAddressFromLine(line);
        }

        if (string.IsNullOrEmpty(title) || address.Length == 0) return null;
        
        return new DiskDocument(title, address, path);
    }

    private static bool IsLineDocumentSignature(string line)
    {
        return line == "[Chippie document]";
    }
    private static bool IsLineTitleProperty(string line)
    {
        string trimmed = TrimString(line).ToLower();
        return trimmed.StartsWith("title=\"") && trimmed.EndsWith('\"'); 
    }
    private static string ExtractTitleFromLine(string line)
    {
        return ExtractPropertyBody(line.Trim());
    }
    private static bool IsLineAddressProperty(string line)
    {
        string trimmed = TrimString(line).ToLower();
        return trimmed.StartsWith("address=\"") && trimmed.EndsWith('\"'); 
    }
    private static string[] ExtractAddressFromLine(string line)
    {
        string body = ExtractPropertyBody(line.Trim());

        return body.Split('.');
    }
    private static string ExtractPropertyBody(string property)
    {
        property = property.Substring(property.IndexOf('\"') + 1);
        property = property.Substring(0, property.Length - 1);
        return property;
    }

    private static string TrimString(string str)
        => str.Trim().Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("\r", "");
}