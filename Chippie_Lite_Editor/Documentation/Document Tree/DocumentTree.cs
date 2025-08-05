using System.IO;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.File;

namespace Chippie_Lite_WPF.Documentation;

public class DocumentTree
{
    public DocumentTreeNode Root { get; } = new("Documentation", null);
    public List<DiskDocument> Documents { get; } = [];
    private List<DocumentTreeNode> Nodes { get; } = [];


    public static DocumentTree CreateFromDirectory(string path)
    {
        DocumentTree tree = new DocumentTree();

        string[] files = ExtractFilePaths(path);

        foreach (var file in files)
        {
            var doc = DiskDocument.FromFile(file);
            if (doc != null) tree.AddDocument(doc);
        }
        
        return tree;
    }

    private static string[] ExtractFilePaths(string path)
    {
        Queue<string> folders = new Queue<string>([path]);
        List<string> paths = [];

        while (folders.Count > 0)
        {
            string folderPath = folders.Dequeue();
            try
            {
                paths.AddRange(Directory.GetFiles(folderPath));
                foreach (var directory in Directory.GetDirectories(folderPath))
                {
                    folders.Enqueue(directory);
                }
            }
            catch (Exception e)
            {
                throw new FileOpenException(e);
            }
        }

        return paths.ToArray();
    }
    
    private void AddDocument(DiskDocument document)
    {
        if (Documents.Contains(document)) return;
        var dir = CreatePath(document.Address);
        Nodes.Add(dir.AddDocument(document));
        Documents.Add(document);
    }

    private DocumentTreeNode CreatePath(string[] address)
    {
        var node = Root;
        
        foreach (var s in address)
        {
            var next = node.FindChild(s);
            if (next == null)
            {
                next = node.AddDirectory(s);
                Nodes.Add(next);
            }
            node = next;
        }

        return node;
    }
}