using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class MemoryView : UserControl
{
    private int HorizontalCellCount;
    private int VerticalCellCount;
    private int _cellsPerPage = 128;
    private int _currentPage;
    private ControlMode mode = ControlMode.View;

    private int searchIndex = 0;

    private Brush InvalidNumberBrush { get; set; }
    private Brush ValidNumberBrush { get; set; }
    
    private List<MemoryViewCell> Cells = new List<MemoryViewCell>();
    
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

    public int PageCount => Memory.Size / CellsPerPage;
    public int CellsPerPage
    {
        get => _cellsPerPage;
        set
        {
            bool changed = _cellsPerPage != value;
            _cellsPerPage = value;
            if (changed) UpdateCellGridLayout(CellGrid.RenderSize);
        } 
    }
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            bool changed = _currentPage != value;
            _currentPage = value;
            
            if (!changed) return;
            
            if (_currentPage >= PageCount) _currentPage = PageCount - 1;
            else if (_currentPage < 0) _currentPage = 0; 
            OnPageChanged();
        }
    }
    
    public MemoryView()
    {
        InitializeComponent();
        FetchColors();
        UpdatePageCountLabel();
        UpdatePageLabel();
        CreateCells();
        ConnectEvents();
    }
    private void ConnectEvents()
    {
        Memory.OnStorageUpdated += OnMemoryUpdated;
    }
    private void CreateCells()
    {
        for (int i = 0; i < CellsPerPage; i++)
        {
            MemoryViewCell cell = new MemoryViewCell();
            Cells.Add(cell);
            CellGrid.Children.Add(cell);
            cell.OnTextChanged += OnCellContentChanged;
        }
    }
    private void FetchColors()
    {
        InvalidNumberBrush = (Application.Current.Resources["Red"] as Brush)!;
        ValidNumberBrush = (Application.Current.Resources["White"] as Brush)!;
    }
    
    private void UpdatePageCountLabel()
    {
        PageCountBlock.Dispatcher.Invoke(() => PageCountBlock.Text = " / " + PageCount);
    }
    private void UpdatePageLabel()
    {
        PageIndexBox.Dispatcher.Invoke(() => PageIndexBox.Text = CurrentPage.ToString()
            .PadLeft(PageCount.ToString().Length, '0'));
    }

    private void OnPageChanged()
    {
        UpdateCellContent();
        UpdateAddressOffsetContent();
        UpdatePageLabel();
    }

    private void UpdateCellGridLayout(Size gridSize)
    {
        RecalculateRowColumnCount(gridSize);
        UpdateGridRowsColumns();
        SetCellsRowColumn();
        UpdateAddressOffsetLabels();
        UpdateAddressOffsetContent();
        UpdateCellContent();
    }
    private void RecalculateRowColumnCount(Size gridSize)
    {
        HorizontalCellCount = (int)(gridSize.Width / 128);
        VerticalCellCount = (CellsPerPage / HorizontalCellCount) + 1;
    }
    private void UpdateGridRowsColumns()
    {
        while (CellGrid.ColumnDefinitions.Count != HorizontalCellCount)
        {
            if (CellGrid.ColumnDefinitions.Count > HorizontalCellCount) CellGrid.ColumnDefinitions.RemoveAt(CellGrid.ColumnDefinitions.Count - 1);
            else if (CellGrid.ColumnDefinitions.Count < HorizontalCellCount) CellGrid.ColumnDefinitions.Add(new ColumnDefinition());
        }
        
        while (CellGrid.RowDefinitions.Count != VerticalCellCount)
        {
            if (CellGrid.RowDefinitions.Count > VerticalCellCount) CellGrid.RowDefinitions.RemoveAt(CellGrid.RowDefinitions.Count - 1);
            else if (CellGrid.RowDefinitions.Count < VerticalCellCount) CellGrid.RowDefinitions.Add(new RowDefinition());
        }
    }
    private void SetCellsRowColumn()
    {
        for (int i = 0; i < Cells.Count; i++)
        {
            var cell = Cells[i];
            int row = i % HorizontalCellCount;
            int column = i / HorizontalCellCount;
            Grid.SetColumn(cell, row);
            Grid.SetRow(cell, column);
        }
    }
    private void UpdateCellContent()
    {
        int start = CurrentPage * CellsPerPage,
            end = int.Min(start + 128, Memory.Size);

        for (int i = 0; i < CellsPerPage; i++)
        {
            if (CellGrid.Children[i] is not MemoryViewCell cell) continue;
            
            int address = start + i;

            if (address > end)
            {
                cell.Visibility = Visibility.Hidden;
                continue;
            }
            
            cell.SetAddressText("0x" + Convert.ToString(address, 16).PadLeft(8, '0'));

            int data = Memory.Read(address);
            
            cell.SetText(NumberUtility.ToHex(data));
        }
    }
    private void UpdateAddressOffsetLabels()
    {
        while (OffsetLabelGrid.ColumnDefinitions.Count != HorizontalCellCount)
        {
            if (OffsetLabelGrid.ColumnDefinitions.Count > HorizontalCellCount)
            {
                OffsetLabelGrid.ColumnDefinitions.RemoveAt(OffsetLabelGrid.ColumnDefinitions.Count - 1);
                OffsetLabelGrid.Children.RemoveAt(OffsetLabelGrid.Children.Count - 1);
            }
            else if (OffsetLabelGrid.ColumnDefinitions.Count < HorizontalCellCount)
            {
                var label = new MemoryViewLabel(){Text = "+" + OffsetLabelGrid.Children.Count};
                Grid.SetColumn(label, OffsetLabelGrid.Children.Count);
                OffsetLabelGrid.Children.Add(label);
                OffsetLabelGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }
        
        // idk
        while (AddressLabelGrid.RowDefinitions.Count != VerticalCellCount - 1)
        {
            if (AddressLabelGrid.RowDefinitions.Count > VerticalCellCount - 1)
            {
                AddressLabelGrid.RowDefinitions.RemoveAt(AddressLabelGrid.RowDefinitions.Count - 1);
                AddressLabelGrid.Children.RemoveAt(AddressLabelGrid.Children.Count - 1);
            }
            else if (AddressLabelGrid.RowDefinitions.Count < VerticalCellCount - 1)
            {
                var label = new MemoryViewLabel();
                Grid.SetRow(label, AddressLabelGrid.Children.Count);
                AddressLabelGrid.Children.Add(label);
                AddressLabelGrid.RowDefinitions.Add(new RowDefinition());
            }
        }
    }
    private void UpdateAddressOffsetContent()
    {
        int offset = 0;
        
        foreach (UIElement child in AddressLabelGrid.Children)
        {
            if (child is not MemoryViewLabel label) continue;

            label.Text = (CurrentPage * CellsPerPage + offset * HorizontalCellCount).ToString(); 
            
            offset++;
        }
    }

    private void OnModeChanged()
    {
        for (int i = 0; i < CellsPerPage; i++)
        {
            if (CellGrid.Children[i] is not MemoryViewCell cell) continue;
                
            cell.SetControlMode(Mode);
        }
    }
    private void OnCellContentChanged(MemoryViewCell sender, int data)
    {
        if (Mode == ControlMode.View) return;

        int address = CurrentPage * CellsPerPage + CellGrid.Children.IndexOf(sender);
        
        Memory.Write(address, data);
    }
    private void OnMemoryUpdated(int index)
    {
        if (Mode == ControlMode.Edit || index < CurrentPage * CellsPerPage || index >= (CurrentPage + 1) * CellsPerPage) return;

        int cellIndex = index % CellsPerPage;

        var cell = CellGrid.Children[cellIndex] as MemoryViewCell;

        int data = Memory.Read(index);
        
        cell!.SetText(Convert.ToString(data, 16).PadLeft(8, '0'));
    }

    private void GoToSearchedAddress()
    {
        if (searchIndex < 0 || searchIndex >= Memory.Size) return;

        int page = searchIndex / CellsPerPage;
        int index = searchIndex % CellsPerPage;

        CurrentPage = page;
        var cell = CellGrid.Children[index];
        
        var ip = (Grid)CellGridViewer.Content;
        var point = cell.TranslatePoint(new Point(), ip);
        CellGridViewer.ScrollToVerticalOffset(point.Y);
    }
    
    
    private void PageIndexBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        PageIndexBox.Foreground = int.TryParse(PageIndexBox.Text, out _) ? ValidNumberBrush : InvalidNumberBrush;
    }
    private void PageIndexBox_OnLostFocus(object sender, RoutedEventArgs e)
    {
        UpdatePageLabel();
    }
    private void PageIndexBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            Keyboard.ClearFocus();
            return;
        }
        
        if (!int.TryParse(PageIndexBox.Text, out int num)) return;

        if (e.Key == Key.Enter)
        {
            CurrentPage = num;
        }
    }
    
    private void PageUpBtn_OnClick(SquareButton sender)
    {
        CurrentPage++;
    }
    private void PageDownBtn_OnClick(SquareButton sender)
    {
        CurrentPage--;
    }
    
    private void SearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        string text = SearchBox.Text;

        if (NumberUtility.TryParseNumber(text, out searchIndex))
        {
            SearchBox.Foreground = ValidNumberBrush;
            return;
        }
        
        SearchBox.Foreground = InvalidNumberBrush;
    }
    private void SearchBtn_OnClick(SquareButton sender)
    {
        GoToSearchedAddress();
    }
    private void CellGrid_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        UpdateCellGridLayout(e.NewSize);
    }
}