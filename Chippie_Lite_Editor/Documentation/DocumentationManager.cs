namespace Chippie_Lite_WPF.Documentation;

public static class DocumentationManager
{
    private static DocumentTree? tree;

    public static DocumentTree Tree
    {
        get { return tree ??= DocumentTree.CreateFromDirectory(DocumentationFolderPath); }
    }

    private static readonly string DocumentationFolderPath = "Documentation";
    
    
    
}