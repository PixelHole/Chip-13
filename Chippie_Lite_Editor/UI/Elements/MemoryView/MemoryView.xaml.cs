using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;
using wpf_Console;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class MemoryView
{
    private List<MemoryViewCell> Cells = [];
    private List<Memory> Modules = [];
    private Memory assignedMemory = ChippieComponentManager.GetComponentOfType<Memory>()!;
    
    private int cellsPerPage = 128;
    private int currentPage;
    private ControlMode mode = ControlMode.View;
    
    private bool IsUpdating;

    public Memory AssignedMemory
    {
        get => assignedMemory;
        set
        {
            assignedMemory.OnStorageUpdated -= OnMemoryUpdated;
            assignedMemory.OnNameChanged -= AssignedMemoryOnNameChanged;
            assignedMemory.OnSizeChanged -= AssignedMemoryOnOnSizeChanged;
            assignedMemory.OnLengthLimitChanged -= AssignedMemoryOnOnLengthLimitChanged;
            assignedMemory = value;
            assignedMemory.OnStorageUpdated += OnMemoryUpdated;
            assignedMemory.OnNameChanged += AssignedMemoryOnNameChanged;
            assignedMemory.OnSizeChanged += AssignedMemoryOnOnSizeChanged;
            assignedMemory.OnLengthLimitChanged += AssignedMemoryOnOnLengthLimitChanged;
            OnModuleChanged();
        }
    }

    public ControlMode Mode
    {
        get => mode;
        set
        {
            if (mode == value) return;
            mode = value;
            OnModeChanged();
        } 
    }
    public int PageCount => (int)Math.Ceiling((double)AssignedMemory.Size / CellsPerPage);
    public int CellsPerPage
    {
        get => cellsPerPage;
        set
        {
            if (cellsPerPage == value) return;
            cellsPerPage = value;
            OnCellsPerPageChanged();
        } 
    }
    public int CurrentPage
    {
        get => currentPage;
        set
        {
            if (currentPage == value) return;
            currentPage = value;
            
            if (currentPage >= PageCount) currentPage = 0;
            else if (currentPage < 0) currentPage = PageCount - 1; 
            OnPageChanged();
        }
    }

    private int CellsInCurrentPage => int.Min(CellsPerPage, AssignedMemory.Size - CurrentPage * CellsPerPage);
    private int CellWidth { get; set; } = -1;
    private int CellHeight { get; set; } = -1;

    
    public delegate void ModuleChangeAction(MemoryView sender, Memory memory);
    public event ModuleChangeAction? OnMemoryModuleChanged;

    public MemoryView()
    {
        InitializeComponent();
        ConnectEvents();
        LoadMemoryModules();
        CreateCells();
        WPFUtility.SetText(PageCountBlock, "/" + PageCount);
        PageIndexBox.Text = "1";
        ModuleSelect.Visibility = Mode == ControlMode.Edit ? Visibility.Collapsed : Visibility.Visible;
        ModuleNameLabel.Visibility = Mode == ControlMode.Edit ? Visibility.Visible : Visibility.Collapsed;
    }
    private void CreateCells()
    {
        ClearCellGrid();
        Cells.Clear();
        for (int i = 0; i < CellsPerPage; i++)
        {
            MemoryViewCell cell = new MemoryViewCell();
            cell.SetControlMode(Mode);
            CellGrid.Children.Add(cell);
            Cells.Add(cell);
            cell.OnTextChanged += CellOnTextChanged;
        }

        System.Console.WriteLine("a");
    }
    private void LoadMemoryModules()
    {
        Modules = ChippieComponentManager.GetComponentsOfType<Memory>();
        ModuleSelect.Items.Clear();
        foreach (var module in Modules)
        {
            ModuleSelect.Items.Add(module.Name);
        }

        IsUpdating = true;
        ModuleSelect.SelectedIndex = Modules.IndexOf(AssignedMemory);
        IsUpdating = false;
    }
    private void ConnectEvents()
    {
        ChippieComponentManager.OnComponentAdded += (_, _) => LoadMemoryModules();
        ChippieComponentManager.OnComponentRemoved += (_, _) => LoadMemoryModules();
    }

    private void OnModuleChanged()
    {
        if (CurrentPage >= PageCount) CurrentPage = PageCount - 1;
        WPFUtility.SetText(PageCountBlock, "/" + PageCount);
        UpdateCellGrid();
        UpdatePageContent();
        
        IsUpdating = true;
        ModuleSelect.SelectedIndex = Modules.IndexOf(AssignedMemory);
        IsUpdating = false;
    }
    private void AssignedMemoryOnNameChanged(Memory sender, string name)
    {
        if (Mode == ControlMode.View) WPFUtility.SetContent(ModuleNameLabel, AssignedMemory.Name);
        else LoadMemoryModules();
    }
    private void AssignedMemoryOnOnLengthLimitChanged(Memory sender, byte lengthlimit)
    {
        UpdatePageContent();
    }
    private void AssignedMemoryOnOnSizeChanged(Memory sender, int size)
    {
        WPFUtility.SetText(PageCountBlock, "/" + PageCount);
        UpdateCellGrid();
        UpdatePageContent();
    }
    private void OnModeChanged()
    {
        ModuleSelect.Visibility = Mode == ControlMode.Edit ? Visibility.Collapsed : Visibility.Visible;
        ModuleNameLabel.Visibility = Mode == ControlMode.Edit ? Visibility.Visible : Visibility.Collapsed;
        
        if (Mode == ControlMode.View) WPFUtility.SetContent(ModuleNameLabel, AssignedMemory.Name);
        else LoadMemoryModules();
        
        if (CellGrid.Children.Count > 0) UpdatePageContent();
    }
    private void OnMemoryUpdated(int index, long data)
    {
        if (index < CurrentPage * CellsPerPage || index >= (CurrentPage + 1) * CellsPerPage) return;
        UpdatePageContent();
    }
    private void OnPageChanged()
    {
        UpdateCellGrid();
        UpdatePageContent();
        PageIndexBox.Text = (CurrentPage + 1).ToString();
    }
    private void OnCellsPerPageChanged()
    {
        UpdateCellGrid();
    }
    private void CellOnTextChanged(MemoryViewCell sender, long data)
    {
        int index = CellGrid.Children.IndexOf(sender);
        if (index < 0 || index >= CellsInCurrentPage) return;
        int address = CurrentPage * CellsPerPage + index;
        AssignedMemory.WriteInitial(address, data);
    }

    private void UpdatePageContent()
    {
        if (CellGrid.ColumnDefinitions.Count == 0 || CellGrid.RowDefinitions.Count == 0) return;
        
        for (int i = 0; i < Cells.Count; i++)
        {
            var cell = Cells[i];
            Grid.SetColumn(cell, i % CellGrid.ColumnDefinitions.Count);
            int row = i / CellGrid.ColumnDefinitions.Count;
            Grid.SetRow(cell, row);

            var visibility = i < CellsInCurrentPage ? Visibility.Visible : Visibility.Collapsed;
            
            cell.Visibility = visibility;
                
            if (i >= CellsInCurrentPage) continue;
            
            var addressBlock = (AddressLabelGrid.Children[row] as MemoryViewLabel)!;
            addressBlock.Text = (CurrentPage * CellsPerPage + i * CellWidth).ToString("X8");
            
            cell.SetControlMode(Mode);
                
            int address = CurrentPage * CellsPerPage + i;
                
            cell.InputBox.Text = "0x" + NumberUtility.ToHex(AssignedMemory.ReadInitial(address), AssignedMemory.LengthLimit);
            cell.SetAddressText("0x" + address.ToString("x8"));
        }
    }

    private void ClearCellGrid()
    {
        CellGrid.Children.Clear();
        CellGrid.ColumnDefinitions.Clear();
        CellGrid.RowDefinitions.Clear();
        AddressLabelGrid.RowDefinitions.Clear();
        AddressLabelGrid.Children.Clear();
        OffsetLabelGrid.ColumnDefinitions.Clear();
        OffsetLabelGrid.Children.Clear();
    }
    private void UpdateCellGrid()
    {
        if (CellGrid.ActualWidth == 0) return;
        bool updateCells = false;
        CellWidth = (int)(CellGrid.ActualWidth / 128);
        CellHeight = (int)Math.Ceiling((double)CellsInCurrentPage / CellWidth);


        if (CellGrid.ColumnDefinitions.Count != CellWidth)
        {
            updateCells = true;
            while (CellGrid.ColumnDefinitions.Count != CellWidth)
            {
                if (CellGrid.ColumnDefinitions.Count < CellWidth) AddColumn();
                else RemoveColumn();
            }
        }
        if (CellGrid.RowDefinitions.Count != CellHeight)
        {
            updateCells = true;
            while (CellGrid.RowDefinitions.Count != CellHeight)
            {
                if (CellGrid.RowDefinitions.Count < CellHeight) AddRow();
                else RemoveRow();
            }

            for (int i = 0; i < AddressLabelGrid.Children.Count; i++)
            {
                var label = (AddressLabelGrid.Children[i] as MemoryViewLabel)!;
                label.Text = (CurrentPage * CellsPerPage + i * CellWidth).ToString("X8");
            }
        }

        if (updateCells) UpdatePageContent();
    }
    private void AddColumn()
    {
        CellGrid.ColumnDefinitions.Add(new ColumnDefinition());
        OffsetLabelGrid.ColumnDefinitions.Add(new ColumnDefinition());
        var offset = new MemoryViewLabel
        {
            Text = "+" + (OffsetLabelGrid.ColumnDefinitions.Count - 1).ToString()
        };
        OffsetLabelGrid.Children.Add(offset);
        Grid.SetColumn(offset, OffsetLabelGrid.ColumnDefinitions.Count - 1);
    }
    private void RemoveColumn()
    {
        CellGrid.ColumnDefinitions.RemoveAt(CellGrid.ColumnDefinitions.Count - 1);
        OffsetLabelGrid.Children.RemoveAt(OffsetLabelGrid.Children.Count - 1);
        OffsetLabelGrid.ColumnDefinitions.RemoveAt(OffsetLabelGrid.ColumnDefinitions.Count - 1);
    }
    private void AddRow()
    {
        CellGrid.RowDefinitions.Add(new RowDefinition());
        AddressLabelGrid.RowDefinitions.Add(new RowDefinition());
        var Address = new MemoryViewLabel();
        Address.MinHeight = 48;
        AddressLabelGrid.Children.Add(Address);
        Grid.SetRow(Address, AddressLabelGrid.RowDefinitions.Count - 1);
    }
    private void RemoveRow()
    {
        CellGrid.RowDefinitions.RemoveAt(CellGrid.RowDefinitions.Count - 1);
        AddressLabelGrid.Children.RemoveAt(AddressLabelGrid.Children.Count - 1);
        AddressLabelGrid.RowDefinitions.RemoveAt(AddressLabelGrid.RowDefinitions.Count - 1);
    }
    
    public bool PageIndexCondition(string s)
    {
        return NumberUtility.TryParseNumber(s, out var num) && num > 0 && num <= PageCount;
    }
    public bool SearchboxCondition(string s)
    {
        return NumberUtility.TryParseNumber(s, out var num);
    }
    
    private void PageIndexBox_OnTextConfirmed(string text)
    {
        
    }
    
    private void PageIndexBox_OnLostFocus(object sender, RoutedEventArgs e)
    {
        
    }
    private void PageIndexBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        
    }
    private void PageUpBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        CurrentPage++;
    }
    private void PageDownBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        CurrentPage--;
    }
    private void SearchBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        
    }
    
    private void CellGrid_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        UpdateCellGrid();
    }
    private void ModuleSelect_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (IsUpdating || ModuleSelect.SelectedIndex < 0 || ModuleSelect.SelectedIndex >= Modules.Count) return;
        AssignedMemory = Modules[ModuleSelect.SelectedIndex];
    }
}