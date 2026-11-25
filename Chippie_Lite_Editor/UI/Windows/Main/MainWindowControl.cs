using System.ComponentModel;
using Chippie_Lite_WPF.Computer;
using Chippie_Lite_WPF.Computer.File;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Handlers;
using Chippie_Lite_WPF.UI.Elements;
using Chippie_Lite_WPF.UI.Help;
using Microsoft.Win32;

namespace Chippie_Lite_WPF.UI.Windows;

public class MainWindowControl
{
    private static string FilePath { get; set; } = string.Empty;
    
    private MainWindow Owner { get; set; }
    private HelpWindow HelpWindow { get; set; } = new HelpWindow();

    public MainWindowControl(MainWindow owner)
    {
        Owner = owner;
        ConnectEvents();
    }

    private void ConnectEvents()
    {
        Chippie.OnRunStarted += ChippieOnOnRunStarted;
        Chippie.OnRunFinished += ChippieOnOnRunFinished;
        Chippie.OnSingleStepChanged += OnSingleChanged;
        SaveFileManager.OnFileLoaded += SaveFileManagerOnOnFileLoaded;
        SaveFileManager.OnFileSaved += SaveFileManagerOnOnFileSaved;
    }
    private void SaveFileManagerOnOnFileSaved(SaveInstance file, string path, SaveOptions options)
    {
        if (options != SaveOptions.FullOptions) return;
        FilePath = path;
    }
    private void SaveFileManagerOnOnFileLoaded(SaveInstance file, string path, SaveOptions options)
    {
        if (options != SaveOptions.FullOptions) return;
        FilePath = path;
    }

    public void RunScript(string script)
    {
        try
        {
            Chippie.CompileAndRunAssembly(script);
        }
        catch (Exception e)
        {
            Chippie.HaltOperation();
            if (e is InvalidInstructionException parseException) Owner.DevArea.HighlightCodeLine(parseException.ActualLine);
            InstructionExceptionHandler.HandleInstructionException(e);
        }
    }
    public void Halt()
    {
        Chippie.HaltOperation();
    }
    public void Restart()
    {
        Chippie.Restart();
    }
    public void NextStep()
    {
        Chippie.SingleStep = true;
        Chippie.ProceedStep();
    }
    public void RunToEnd()
    {
        Chippie.SingleStep = false;
        Chippie.ProceedStep();
    }


    public void RequestNewInstance()
    {
        if (!string.IsNullOrEmpty(FilePath) && Owner.DevArea.SourceChanged)
        {
            var prompt = new ConfirmationPrompt("Are you sure?",
                "you have unsaved changes that will be wiped when you create a new project. are you sure you would like to proceed",
                ["Cancel", "Ok"], s =>
                {
                    if (s == "Ok")
                    {
                        NewInstance();
                    }
                });
            
            prompt.Show();
            
            return;
        }
        
        NewInstance();
    }
    public void RequestLoad()
    {
        if (!string.IsNullOrEmpty(FilePath) && Owner.DevArea.SourceChanged)
        {
            var prompt = new ConfirmationPrompt("Are you sure?",
                "you have unsaved changes that will be wiped when you load a project. are you sure you would like to proceed",
                ["Cancel", "Ok"], 
                s => { if (s == "Ok") LoadInstance(FilePath); });
            
            prompt.Show();
            return;
        }
        
        LoadInstance(FilePath);
    }
    public void RequestSaveAs()
    {
        SaveInstance("");
    }
    public void RequestSave()
    {
        SaveInstance(FilePath);
    }

    private void SaveInstance(string path)
    {
        try
        {
            SaveFileManager.SaveToFile(path);
            if (string.IsNullOrWhiteSpace(FilePath)) FilePath = path;
        }
        catch (Exception e)
        {
            SaveFileExceptionHandler.HandleSaveExceptions(e);
        }
    }
    private void LoadInstance(string path)
    {
        try
        {
            SaveFileManager.LoadFromFile(path);
        }
        catch (Exception e)
        {
            SaveFileExceptionHandler.HandleLoadException(e);
        }
        
        FilePath = path;
        Owner.DevArea.SourceChanged = false;
    }
    private void NewInstance()
    {
        Chippie.FullFlush();
        Owner.DevArea.SetInputCode("");
        RequestSaveAs();
        Owner.DevArea.SourceChanged = false;
    }
    
    public void OpenHelpWindow()
    {
        HelpWindow.Show();
    }

    private void OnSingleChanged(bool singleStep)
    {
        Owner.RunToEndButtonVisible(singleStep);
    }

    private void ChippieOnOnRunFinished()
    {
        Owner.SetMode(AppMode.Edit);
    }
    private void ChippieOnOnRunStarted()
    {
        Owner.SetMode(AppMode.Run);
        Owner.ChangeDevAreaPage(2);
    }
}