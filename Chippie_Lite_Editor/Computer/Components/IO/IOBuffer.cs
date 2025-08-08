using System.Threading.Tasks.Dataflow;
using System.Windows.Input;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.Computer.Components;

public static class IOBuffer
{
    private static Register? bufferCountRegister;
    private static readonly Semaphore inputAccessPool = new(1, 1);
    private static readonly Semaphore outputAccessPool = new(1, 1);
    private static BufferBlock<int> InputBuffer { get; } = new();
    private static BufferBlock<Key> KeyInputBuffer { get; } = new();
    private static Queue<string> OutputBuffer { get; } = new();
    
    public static bool WaitingForInput { get; private set; }

    public static Register BufferCountRegister
    {
        get
        {
            if (bufferCountRegister == null) bufferCountRegister = RegisterBank.GetRegister("Input Buffer Count")!;
            return bufferCountRegister;
        }
    }


    public delegate void WaitForInputStartAction();
    public static event WaitForInputStartAction? OnWaitForInputStart;
    
    public delegate void WaitForInputEndAction();
    public static event WaitForInputEndAction? OnWaitForInputEnd;

    public delegate void WaitForKeyInputStartAction();
    public static event WaitForKeyInputStartAction? OnWaitForKeyInputStart;
    
    public delegate void WaitForKeyInputEndAction();
    public static event WaitForKeyInputEndAction? OnWaitForKeyInputEnd;
    
    public delegate void OutputBufferedAction();
    public static event OutputBufferedAction? OnOutputBuffered;
    
    public delegate void InputBufferedAction();
    public static event InputBufferedAction? OnInputBuffered;
    
    public delegate void KeyInputBufferedAction();
    public static event KeyInputBufferedAction? OnKeyInputBuffered;
    

    public static void BufferInput(string input)
    {
        int[] nums = SerialUtility.AsciiToInts(input);
        BufferCountRegister.Content += nums.Length;
        
        inputAccessPool.WaitOne();

        foreach (var num in nums)
        {
            InputBuffer.SendAsync(num);
        }
        
        inputAccessPool.Release();
        
        OnInputBuffered?.Invoke();
    }
    public static async Task<int> GetInput()
    {
        int res;
        
        if (InputBuffer.Count == 0)
        {
            WaitingForInput = true;
            OnWaitForInputStart?.Invoke();

            res = await InputBuffer.ReceiveAsync();

            WaitingForInput = false;
            OnWaitForInputEnd?.Invoke();
        }
        else
        {
            res = InputBuffer.Receive();
        }

        BufferCountRegister.Content--;
        return res;
    }
    public static void FlushInputBuffer()
    {
        InputBuffer.TryReceiveAll(out _);
        BufferCountRegister.Content = 0;
    }

    public static void BufferKeyInput(Key key)
    {
        inputAccessPool.WaitOne();
        
        KeyInputBuffer.SendAsync(key);
        
        inputAccessPool.Release();
        
        OnKeyInputBuffered?.Invoke();
    }
    public static async Task<Key> GetKeyInput()
    {
        Key res;
        
        if (KeyInputBuffer.Count == 0)
        {
            WaitingForInput = true;
            OnWaitForKeyInputStart?.Invoke();

            res = await KeyInputBuffer.ReceiveAsync();

            WaitingForInput = false;
            OnWaitForKeyInputEnd?.Invoke();
        }
        else
        {
            res = KeyInputBuffer.Receive();
        }

        return res;
    }
    public static void FlushKeyInputBuffer() => KeyInputBuffer.TryReceiveAll(out _);
    
    public static void BufferOutput(int[] output)
    {
        string parsed = SerialUtility.IntsToAscii(output);
        
        outputAccessPool.WaitOne();

        OutputBuffer.Enqueue(parsed);

        outputAccessPool.Release();
        
        OnOutputBuffered?.Invoke();
    }
    public static string GetOutput()
    {
        outputAccessPool.WaitOne();

        string output = OutputBuffer.Count == 0 ? "" : OutputBuffer.Dequeue();

        outputAccessPool.Release();

        return output;
    }
    public static void FlushOutputBuffer() => OutputBuffer.Clear();

    public static void FullFlush()
    {
        FlushInputBuffer();
        FlushKeyInputBuffer();
        FlushOutputBuffer();
    }
}