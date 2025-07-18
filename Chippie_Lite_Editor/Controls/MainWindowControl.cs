using System.ComponentModel;
using System.IO;
using Chippie_Lite_WPF.Computer;
using Chippie_Lite_WPF.Computer.File;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;
using Chippie_Lite_WPF.UI.Elements;
using Chippie_Lite_WPF.UI.Windows;
using Microsoft.Win32;

namespace Chippie_Lite_WPF.Controls;

public class MainWindowControl
{
    private static string FilePath { get; set; } = string.Empty;
    
    
    private MainWindow Owner { get; set; }

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
    }

    public void RunScript(string script)
    {
        try
        {
            Chippie.RunRawAssembly(script);
        }
        catch (Exception e)
        {
            Chippie.HaltOperation();
            
            switch (e)
            {
                case InvalidInstructionException parseException:
                    var point = Owner.DevArea.HighlightCodeLine(parseException.ActualLine);
                    ErrorBoxManager.ShowExceptionError(parseException, point);
                    break;

                case InstructionInterpretationException interpretationException:
                    ErrorBoxManager.ShowExceptionError(interpretationException);
                    break;

                default:
                    ErrorBoxManager.ShowError("internal error", $"An internal error occured : {e.Message}");
                    break;
            }
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
        Chippie.ProceedStep();
    }
    public void RunToEnd()
    {
        Chippie.SingleStep = false;
    }


    public void RequestNewInstance()
    {
        if (!string.IsNullOrEmpty(FilePath) && Owner.DevArea.SourceChanged)
        {
            var prompt = new ConfirmationPrompt("Are you sure?",
                "you have unsaved changes that will be wiped when you create a new project. are you sure you would like to proceed");
            
            prompt.OnButtonSelected += NewInstancePromptOnSelect;
            
            prompt.Show();
            
            return;
        }
        
        NewInstance();
    }
    private void NewInstancePromptOnSelect(string choice)
    {
        if (choice != "Ok") return;
        
        NewInstance();
    }
    public void RequestLoad()
    {
        if (!string.IsNullOrEmpty(FilePath) && Owner.DevArea.SourceChanged)
        {
            var prompt = new ConfirmationPrompt("Are you sure?",
                "you have unsaved changes that will be wiped when you load a project. are you sure you would like to proceed");
            
            prompt.OnButtonSelected += LoadPromptOnButtonSelected;
            
            prompt.Show();
            
            return;
        }
        
        LaunchLoadWindow();
    }
    private void LoadPromptOnButtonSelected(string choice)
    {
        if (choice != "Ok") return;
        
        LaunchLoadWindow();
    }
    public void RequestSaveAs()
    {
        LaunchSaveWindow();
    }
    public void RequestSave()
    {
        if (!string.IsNullOrEmpty(FilePath))
        {
            SaveInstance(FilePath);
            return;
        }
        
        LaunchSaveWindow();
    }
    
    private void LaunchLoadWindow()
    {
        OpenFileDialog fileDialog = new OpenFileDialog()
        {
            Multiselect = false,
            Filter = "json files (*.json)|*.json|All files (*.*)|*.*"
        };
        
        fileDialog.FileOk += LoadDialogOk;
        
        fileDialog.ShowDialog();
    }
    private void LoadDialogOk(object? sender, CancelEventArgs e)
    {
        var fileDialog = sender as OpenFileDialog;
        fileDialog.FileOk -= LoadDialogOk;
        
        string path = fileDialog.FileName;
        
        LoadInstance(path);
    }
    private void LaunchSaveWindow()
    {
        SaveFileDialog fileDialog = new SaveFileDialog()
        {
            CreatePrompt = false,
            OverwritePrompt = true,
            Filter = "json files (*.json)|*.json|All files (*.*)|*.*"
        };
        
        fileDialog.FileOk += SaveDialogOk;
        
        fileDialog.ShowDialog();
    }
    private void SaveDialogOk(object? sender, CancelEventArgs e)
    {
        var fileDialog = sender as SaveFileDialog;
        fileDialog.FileOk -= SaveDialogOk;

        string path = fileDialog.FileName;
        
        SaveInstance(path);
    }

    private void SaveInstance(string path)
    {
        try
        {
            SaveFileManager.SaveData(Owner.DevArea.GetInputCode().Trim(), path);
            FilePath = path;
        }
        catch (Exception e)
        {
            
        }
    }
    private void LoadInstance(string path)
    {
        try
        {
            string code = SaveFileManager.LoadData(path);
            Owner.DevArea.SetInputCode(code);
            FilePath = path;
            Owner.DevArea.SourceChanged = false;
        }
        catch (Exception e)
        {
            
        }
    }
    private void NewInstance()
    {
        Chippie.FullFlush();
        Owner.DevArea.SetInputCode("");
        RequestSaveAs();
        Owner.DevArea.SourceChanged = false;
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