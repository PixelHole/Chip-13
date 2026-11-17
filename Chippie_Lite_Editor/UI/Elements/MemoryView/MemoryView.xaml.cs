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

    public Memory AssignedMemory
    {
        get => assignedMemory;
        set
        {
            assignedMemory.OnStorageUpdated -= OnMemoryUpdated;
            assignedMemory = value;
            assignedMemory.OnStorageUpdated += OnMemoryUpdated;
            CurrentPage = 0;
            OnPageChanged();
        }
    }

    public ControlMode Mode
    {
        get => mode;
        set
        {
            bool changed = mode == value;
            mode = value;
            if (!changed) return;
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

    public MemoryView()
    {
        InitializeComponent();
        ConnectEvents();
        LoadMemoryModules();
        CreateCells();
    }
    private void CreateCells()
    {
        for (int i = 0; i < CellsPerPage; i++)
        {
            MemoryViewCell cell = new MemoryViewCell();
            CellGrid.Children.Add(cell);
            Cells.Add(cell);
            cell.OnTextChanged += CellOnOnTextChanged;
        }
    }
    private void LoadMemoryModules()
    {
        Modules = ChippieComponentManager.GetComponentsOfType<Memory>();
        ModuleSelect.Items.Clear();
        foreach (var module in Modules)
        {
            ModuleSelect.Items.Add(module.Name);
        }
    }
    private void ConnectEvents()
    {
        ChippieComponentManager.OnComponentAdded += (component, _) => {
            if (component is Memory mem) Modules.Add(mem);
        };
        ChippieComponentManager.OnComponentRemoved += (component, index) => {
            if (component is Memory mem) Modules.Remove(mem);
        };
    }

    private void OnModeChanged()
    {
        
    }
    private void OnMemoryUpdated(int index, long data)
    {
        
    }
    private void OnPageChanged()
    {
        
    }
    private void OnCellContentChanged(MemoryViewCell sender, long data)
    {
        
    }
    private void OnCellsPerPageChanged()
    {
        
    }
    private void CellOnOnTextChanged(MemoryViewCell sender, long data)
    {
        
    }

    private void UpdatePageContent()
    {
        for (int i = 0; i < int.Min(Cells.Count, CellsInCurrentPage); i++)
        {
            
        }
        for (int i = CellsInCurrentPage; i < Cells.Count; i++)
        {
            
        }
    }
    
    private void UpdateCellGrid()
    {
        CellWidth = (int)(CellGrid.Width / 128);
        CellHeight = (int)(CellGrid.Height / 48);

        while (CellGrid.ColumnDefinitions.Count != CellWidth)
        {
            if (CellGrid.ColumnDefinitions.Count < CellWidth) AddColumn();
            else RemoveColumn();
        }
        while (CellGrid.RowDefinitions.Count != CellHeight)
        {
            if (CellGrid.RowDefinitions.Count < CellHeight) AddRow();
            else RemoveRow();
        }
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
        OffsetLabelGrid.ColumnDefinitions.RemoveAt(OffsetLabelGrid.ColumnDefinitions.Count - 1);
    }
    private void AddRow()
    {
        // fix later
        CellGrid.RowDefinitions.Add(new RowDefinition());
        AddressLabelGrid.RowDefinitions.Add(new RowDefinition());
        // var Address = new MemoryViewLabel()
        // {
        //     Text = 
        // }
    }
    private void RemoveRow()
    {
        CellGrid.RowDefinitions.RemoveAt(CellGrid.RowDefinitions.Count - 1);
        AddressLabelGrid.RowDefinitions.RemoveAt(AddressLabelGrid.RowDefinitions.Count - 1);
    }
    
    public bool PageIndexCondition(string s)
    {
        return NumberUtility.TryParseNumber(s, out _);
    }
    public bool SearchboxCondition(string s)
    {
        return NumberUtility.TryParseNumber(s, out var num);
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
}