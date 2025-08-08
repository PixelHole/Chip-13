using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Components.Actions;
using ConsoleControlLibrary;
using wpf_Console;

// ReSharper disable once CheckNamespace
namespace Wpf_ConsoleIO;

public class ConsoleControl
{
    private Vector2Int cursor = Vector2Int.Zero;
    private readonly ConsoleView View;

    private int BackgroundIndex = 0;
    private int ForegroundIndex = 7;
    

    public ConsoleMode Mode { get; set; } = ConsoleMode.Simple;
    internal List<ConsoleGlyph> Glyphs { get; } = [];

    private List<IntRect> WritableZones { get; } = [];
    private List<IntRect> CursorForbidZones { get; } = [];
    
    public Vector2Int Cursor
    {
        get => cursor;
        private set
        {
            var newPos = WrapPosition(value);
            if (cursor == newPos || !CanCursorGoTo(newPos)) return;
            cursor = newPos;
            OnCursorMoved?.Invoke(Cursor);
        } 
    }

    public Key SubmitKey { get; set; } = Key.Enter;
    public ConsoleInputMode InputMode { get; private set; } = ConsoleInputMode.None;
        

    public delegate void CursorMovedAction(Vector2Int position);
    public event CursorMovedAction? OnCursorMoved;
    

    public ConsoleControl(ConsoleView view)
    {
        View = view;
        ConnectEvents();
    }
    private void ConnectEvents()
    {
        IOBuffer.OnWaitForKeyInputStart += () => SetInputMode(ConsoleInputMode.Key);
        IOBuffer.OnWaitForKeyInputEnd += () => SetInputMode(ConsoleInputMode.None);
        IOBuffer.OnWaitForInputStart += () => SetInputMode(ConsoleInputMode.Text);
        IOBuffer.OnWaitForInputEnd += () => SetInputMode(ConsoleInputMode.None);

        IOBuffer.OnOutputBuffered += SerialIOOnOnOutputBuffered;
        
        Chippie.OnRunStarted += ChippieOnOnRunStarted;
        
        IOInterface.SetBackgroundRequest += InstructionIOActionsOnSetBackgroundRequest;
        IOInterface.SetForegroundRequest += InstructionIOActionsOnSetForegroundRequest; 
        IOInterface.SetCursorRequest += InstructionIOActionsOnSetCursorRequest; 
        IOInterface.SetCursorLeftRequest += InstructionIOActionsOnSetCursorLeftRequest;
        IOInterface.SetCursorTopRequest += InstructionIoActionsOnSetCursorTopRequest;
    }

    internal void SetInputMode(ConsoleInputMode mode) => InputMode = mode;
    
    internal void ProcessUserKey(Key key)
    {
        if (InputMode == ConsoleInputMode.Key)
        {
            IOBuffer.BufferKeyInput(key);
            return;
        }
        
        switch (key)
        {
            case Key.Back :
                DeleteAction();
                break;
            
            case Key.Enter:
                if (InputMode == ConsoleInputMode.Text) NextLine();
                break;
            
            case Key.Tab :
                if (InputMode == ConsoleInputMode.Text) Write("    ", ConsoleInputSource.User, true);
                break;
            
            case Key.Left :
                LeftAction();
                break;
            
            case Key.Right :
                RightAction();
                break;
            
            case Key.Up :
                UpAction();
                break;
            
            case Key.Down :
                DownAction();
                break;
        }

        if (key == SubmitKey)
        {
            if (InputMode == ConsoleInputMode.Text) BufferTextInput();
        }
    }

    private void DeleteAction()
    {
        var back = GetGlyphAt(WrapPosition(Cursor + Vector2Int.Left));
        if (back != null && back.Source != ConsoleInputSource.User) return;
        if (Delete(true) && InputMode == ConsoleInputMode.Text) CursorLeft();
    }
    private void LeftAction()
    {
        var back = GetGlyphAt(WrapPosition(Cursor + Vector2Int.Left));
        if (Mode == ConsoleMode.Simple && (back == null /*|| back.Source != ConsoleInputSource.User*/)) return;
        CursorLeft();
    }
    private void RightAction()
    {
        var right = GetGlyphAt(WrapPosition(Cursor + Vector2Int.Right));
        if (Mode == ConsoleMode.Simple && (GetGlyphAt(Cursor) == null /*|| right?.Source != ConsoleInputSource.User*/)) return;
        CursorRight();
    }
    private void UpAction()
    {
        var up = GetGlyphAt(WrapPosition(Cursor + Vector2Int.Down));
        if (Mode == ConsoleMode.Simple && (up == null /*|| up.Source != ConsoleInputSource.User*/)) return;
        CursorUp();
    }
    private void DownAction()
    {
        var down = GetGlyphAt(WrapPosition(Cursor + Vector2Int.Up));
        if (Mode == ConsoleMode.Simple && (down == null /*|| down.Source != ConsoleInputSource.User*/)) return;
        CursorDown();
    }
    
    internal void ProcessUserText(string text)
    {
        if (InputMode is ConsoleInputMode.Key or ConsoleInputMode.None) return;

        var current = GetGlyphAt(Cursor);
        if (current != null && current.Source != ConsoleInputSource.User) return;
        
        foreach (var c in text.Where(c => !char.IsControl(c)))
        {
            Write(c, ConsoleInputSource.User, true);
        }
    }

    private void BufferTextInput()
    {
        var sentences = ReadAll(ConsoleInputSource.User);

        foreach (var sentence in sentences)
        {
            IOBuffer.BufferInput(sentence);
        }
    }


    public string[] ReadAll(ConsoleInputSource targetSource, bool onlyUnprocessed = true)
    {
        switch (Glyphs.Count)
        {
            case 0:
                return [];
            case 1:
            {
                var glyph = Glyphs[0];
                if (glyph.Processed == onlyUnprocessed) return [];
                glyph.Processed = true;
                return [glyph.Text.ToString()];
            }
        }

        List<string> sentences = new List<string>();

        
        StringBuilder buffer = new StringBuilder();
        
        for (int i = 0; i < Glyphs.Count; i++)
        {
            var current = Glyphs[i];
            if ((current.Processed != onlyUnprocessed) && current.Source == targetSource) buffer.Append(current.Text.ToString());
            else continue;

            if (i >= Glyphs.Count - 1) continue;
            
            var next = Glyphs[i + 1];

            if (LinearDistance(current, next) > 1 || next.Source != targetSource || (next.Processed && onlyUnprocessed))
            {
                sentences.Add(buffer.ToString());
                buffer.Clear();
            }
        }

        if (buffer.Length > 0) sentences.Add(buffer.ToString());
        buffer.Clear();
        
        return sentences.ToArray();
    }
    
    public char Read(ConsoleInputSource targetSource, bool onlyUnprocessed = true) => ReadAt(Cursor, targetSource, onlyUnprocessed);
    public char ReadAt(Vector2Int position, ConsoleInputSource targetSource, bool onlyUnprocessed = true)
    {
        var glyph = GetGlyphAt(position);

        if (glyph == null) return '\0';
        
        if (glyph.Source != targetSource || (glyph.Processed && onlyUnprocessed)) return '\0';

        glyph.Processed = true;
        return glyph.Text;
    }
    

    public void WriteLine(string text, ConsoleInputSource source)
    {
        Write(text, source, true);
        NextLine();
    }
    
    public void Write(string text, ConsoleInputSource source, bool push) => WriteAt(text, source, Cursor, push);
    public void Write(char c, ConsoleInputSource source, bool push) => WriteAt(c, source, Cursor, push);
    
    public void WriteAt(string text, ConsoleInputSource source, Vector2Int position, bool push)
    {
        for (int i = 0; i < text.Length; i++)
        {
            WriteAt(text[i], source, position + Vector2Int.Right * i, push);
        }
    }
    public void WriteAt(char c, ConsoleInputSource source, Vector2Int position, bool push)
    {
        position = WrapPosition(position);
        
        if (!CanWriteAt(position)) return;
        
        var glyph = GetGlyphAt(position);
        
        if (glyph != null && !push)
        {
            UpdateGlyphContent(glyph, c);
        }
        else
        {
            if (glyph != null && push)
            {
                PushGlyphsForward(glyph);
            }
            AddGlyphAt(c, source, position);
        }

        var cur = position - Cursor + Vector2Int.Right;
        
        MoveCursor(cur);
    }

    public bool Delete(bool pull) => DeleteAt(WrapPosition(Cursor + Vector2Int.Left), pull);
    public bool DeleteAt(Vector2Int position, bool pull)
    {
        var deleted = DeleteGlyphAt(position);

        if (deleted == null) return false;
        if (!pull) return true;

        var right = GetGlyphAt(RightOf(deleted));
        
        if (right != null) PullGlyphsBack(right);

        return true;
    }

    private void AddGlyphAt(char c, ConsoleInputSource source,Vector2Int position)
    {
        ConsoleGlyph created = View.CreateGlyph(c, ForegroundIndex, BackgroundIndex, position, source);
        TrackGlyph(created);
    }
    public ConsoleGlyph? DeleteGlyphAt(Vector2Int position)
    {
        position = WrapPosition(position);
        
        var glyph = GetGlyphAt(position);
        
        if (glyph == null) return null;

        View.RemoveGlyph(glyph);
        Glyphs.Remove(glyph);

        return glyph;
    }
    private void UpdateGlyphContent(ConsoleGlyph glyph, char c)
    {
        if (glyph.CheckAccess()) UpdateGlyphContentAction(glyph, c);
        else glyph.Dispatcher.Invoke(() => UpdateGlyphContentAction(glyph, c));
    }
    private void UpdateGlyphContentAction(ConsoleGlyph glyph, char c)
    {
        glyph.Text = c;
        glyph.SetForeground(ForegroundIndex);
        glyph.SetBackground(BackgroundIndex);
    }
    public void Clear()
    {
        foreach (var glyph in Glyphs)
        {
            View.RemoveGlyph(glyph);
        }
        Glyphs.Clear();
    }

    private void PushGlyphsForward(ConsoleGlyph start)
    {
        List<ConsoleGlyph> line = new List<ConsoleGlyph>([start]);

        ConsoleGlyph? right = GetGlyphAt(RightOf(start));

        while (right != null)
        {
            if (right.Processed != start.Processed || right.Source != start.Source || right == start) return;
            line.Add(right);
            right = GetGlyphAt(RightOf(right));
        }

        if (right != null) return;

        for (int i = line.Count - 1; i >= 0; i--)
        {
            var glyph = line[i];
            PushGlyph(glyph, Vector2Int.Right);
        }
    }
    private void PullGlyphsBack(ConsoleGlyph start)
    {
        if (GetGlyphAt(LeftOf(start)) != null) return;

        ConsoleGlyph glyph = start;
        
        while (true)
        {
            var right = GetGlyphAt(RightOf(glyph));
            PushGlyph(glyph, Vector2Int.Left);
            if (right == null || right.Processed != start.Processed || right.Source != start.Source) return;
            glyph = right;
        }
    }
    private void PushGlyph(ConsoleGlyph glyph, Vector2Int direction)
    {
        var blocker = GetGlyphAt(glyph.Position + direction);
        
        if (blocker != null) return;

        glyph.Position = WrapPosition(glyph.Position + direction);

        View.UpdateGlyph(glyph);
    }

    private void TrackGlyph(ConsoleGlyph glyph)
    {
        switch (Glyphs.Count)
        {
            case 0:
                Glyphs.Add( glyph);
                return;
            case 1:
            {
                var other = Glyphs[0];
                if (glyph.CheckAccess()) Glyphs.Insert(other.Position >= glyph.Position ? 0 : 1, glyph);
                else glyph.Dispatcher.Invoke(() => Glyphs.Insert(other.Position >= glyph.Position ? 0 : 1, glyph));
                return;
            }
        }

        int start = 0, end = Glyphs.Count;
        int mid;
        
        while (true)
        {
            mid = (start + end) / 2;
            
            if (start >= end || int.Abs(start - end) <= 1) break;
            
            var other = Glyphs[mid];

            if (LinearPosition(other) >= LinearPosition(glyph))
            {
                end = mid;
                continue;
            }

            if (LinearPosition(other) < LinearPosition(glyph))
            {
                start = mid;
            }
        }
        
        Glyphs.Insert(mid + 1, glyph);
    }

    public bool AddWriteableZone(IntRect zone)
    {
        if (WritableZones.Contains(zone)) return false;
        WritableZones.Add(zone);
        return true;
    }
    public bool RemoveWriteableZone(IntRect zone)
    {
        return WritableZones.Remove(zone);
    }
    public IEnumerable<IntRect> GetWriteableZonesAt(Vector2Int point)
    {
        List<IntRect> zones = [];
        zones.AddRange(WritableZones.Where(zone => zone.IsPointInside(point)));

        return zones;
    }

    public bool AddCursorForbidZone(IntRect zone)
    {
        if (CursorForbidZones.Contains(zone)) return false;
        CursorForbidZones.Add(zone);
        return true;
    }
    public bool RemoveCursorForbidZone(IntRect zone)
    {
        return CursorForbidZones.Remove(zone);
    }
    public IEnumerable<IntRect> GetCursorForbidZonesAt(Vector2Int point)
    {
        List<IntRect> zones = [];
        zones.AddRange(CursorForbidZones.Where(zone => zone.IsPointInside(point)));
        return zones;
    }
    
    public void MoveCursor(Vector2Int movement)
    {
        Cursor += movement;
    }
    public void NextLine() => MoveCursor(new Vector2Int(-Cursor.x, 1));
    public void CursorLeft() => MoveCursor(Vector2Int.Left);
    public void CursorRight() => MoveCursor(Vector2Int.Right);
    public void CursorUp() => MoveCursor(Vector2Int.Down);
    public void CursorDown() => MoveCursor(Vector2Int.Up);
    
    public ConsoleGlyph? GetGlyphAt(Vector2Int position)
    {
        return Glyphs.Find(g => g.Position == position);
    }
    private int IndexOf(ConsoleGlyph glyph) => Glyphs.IndexOf(glyph);
    private Vector2Int WrapPosition(Vector2Int position)
    {
        if (position.x >= View.GlyphCount.x) position = new Vector2Int(position.x % View.GlyphCount.x, position.y + 1);
        else if (position.x < 0) position = new Vector2Int(View.GlyphCount.x - (-position.x % View.GlyphCount.x), position.y - 1);
        if (position.y >= View.GlyphCount.y) position.y %= View.GlyphCount.y;
        else if (position.y < 0) position.y = View.GlyphCount.y - (-position.y % View.GlyphCount.y);

        return position;
    }
    private Vector2Int LeftOf(ConsoleGlyph glyph) => WrapPosition(glyph.Position + Vector2Int.Left);
    private Vector2Int RightOf(ConsoleGlyph glyph) => WrapPosition(glyph.Position + Vector2Int.Right);
    // ReSharper disable once UnusedMember.Local
    private List<ConsoleGlyph> GetRightConnected(ConsoleGlyph start)
    {
        int index = Glyphs.IndexOf(start);
        if (index == -1 || index == Glyphs.Count - 1) return [];

        List<ConsoleGlyph> result = new List<ConsoleGlyph>();

        var glyph = start;
        
        for (int i = index + 1; i < Glyphs.Count; i++)
        {
            var right = Glyphs[i];
            if (LinearDistance(glyph, right) != 1) return result;
            result.Add(right);
            glyph = right;
        }

        return result;
    }
    
    private int LinearDistance(ConsoleGlyph a, ConsoleGlyph b)
    {
        return int.Abs(LinearPosition(b) - LinearPosition(a));
    }
    private int LinearPosition(ConsoleGlyph glyph)
    {
        int pos = 0;

        if (glyph.CheckAccess()) pos = glyph.Position.y * View.GlyphCount.x + glyph.Position.x;
        else glyph.Dispatcher.Invoke(() => pos = glyph.Position.y * View.GlyphCount.x + glyph.Position.x);

        return pos;
    }

    private bool CanWriteAt(Vector2Int pos)
    {
        return WritableZones.Count == 0 || WritableZones.Any(zone => zone.IsPointInside(pos));
    }
    private bool CanCursorGoTo(Vector2Int pos)
    {
        return CursorForbidZones.Count == 0 || !CursorForbidZones.Any(zone => zone.IsPointInside(pos));
    }
    
    private void SerialIOOnOnOutputBuffered()
    {
        var output = IOBuffer.GetOutput();
        Write(output, ConsoleInputSource.System, false);
    }
    private void ChippieOnOnRunStarted()
    {
        Clear();
        SetInputMode(ConsoleInputMode.None);
        Cursor = Vector2Int.Zero;
    }
    private void InstructionIOActionsOnSetForegroundRequest(int index)
    {
        ForegroundIndex = index;
    }
    private void InstructionIOActionsOnSetBackgroundRequest(int index)
    {
        BackgroundIndex = index;
    }
    private void InstructionIOActionsOnSetCursorRequest(Vector2Int position)
    {
        var movement = WrapPosition(position) - Cursor;
        MoveCursor(movement);
    }
    private void InstructionIOActionsOnSetCursorLeftRequest(int left)
    {
        var movement = new Vector2Int(left - Cursor.x, 0);
        MoveCursor(movement);
    }
    private void InstructionIoActionsOnSetCursorTopRequest(int top)
    {
        var movement = new Vector2Int(0, top - Cursor.y);
        MoveCursor(movement);
    }
}