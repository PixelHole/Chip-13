using System.Data;

namespace Chippie_Lite_WPF.Documentation;

public class DocumentTreeNode
{
    public string Name { get; }
    public DiskDocument? Document { get; }
    
    public DocumentTreeNode? Parent { get; init; }
    public List<DocumentTreeNode> Children { get; } = [];


    public DocumentTreeNode(string name, DocumentTreeNode? parent, DiskDocument? document = null)
    {
        Name = name;
        Document = document;
        Parent = parent;
    }

    public DocumentTreeNode AddDirectory(string name)
    {
        DocumentTreeNode directory = new DocumentTreeNode(name, this);
        AddChild(directory);
        return directory;
    }
    public DocumentTreeNode AddDocument(DiskDocument document)
    {
        DocumentTreeNode docNode = new DocumentTreeNode(document.Name, this, document);
        AddChild(docNode);
        return docNode;
    }

    public DocumentTreeNode? FindChild(string name) => Children.Find(child => child.Name == name);
    
    private void AddChild(DocumentTreeNode child)
    {
        if (Children.Find(node => node.Name == child.Name) != null) throw new DuplicateNameException();
        Children.Add(child);
    }
    private bool RemoveChild(DocumentTreeNode node)
    {
        return Children.Remove(node);
    }

    public override string ToString()
    {
        return Document == null ? $"{Name}({Children.Count})" : Name;
    }
}