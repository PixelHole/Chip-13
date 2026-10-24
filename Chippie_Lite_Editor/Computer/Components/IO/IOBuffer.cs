using System.Threading.Tasks.Dataflow;
using System.Windows.Input;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.Computer.Components;

public class IOBuffer : ChippieComponent
{
    private Register? bufferCountRegister;
    private readonly Semaphore inputAccessPool = new(1, 1);
    private readonly Semaphore outputAccessPool = new(1, 1);
    private BufferBlock<long> InputBuffer { get; } = new();
    private BufferBlock<Key> KeyInputBuffer { get; } = new();
    private Queue<string> OutputBuffer { get; } = new();
    
    public bool WaitingForInput { get; private set; }

    public Register BufferCountRegister
    {
        get
        {
            if (bufferCountRegister == null) bufferCountRegister = Chippie.MainRegisterBank.GetRegister("Input Buffer Count")!;
            return bufferCountRegister;
        }
    }


    public delegate void WaitForInputStartAction();
    public event WaitForInputStartAction? OnWaitForInputStart;
    
    public delegate void WaitForInputEndAction();
    public event WaitForInputEndAction? OnWaitForInputEnd;

    public delegate void WaitForKeyInputStartAction();
    public event WaitForKeyInputStartAction? OnWaitForKeyInputStart;
    
    public delegate void WaitForKeyInputEndAction();
    public event WaitForKeyInputEndAction? OnWaitForKeyInputEnd;
    
    public delegate void OutputBufferedAction();
    public event OutputBufferedAction? OnOutputBuffered;
    
    public delegate void InputBufferedAction();
    public event InputBufferedAction? OnInputBuffered;
    
    public delegate void KeyInputBufferedAction();
    public event KeyInputBufferedAction? OnKeyInputBuffered;
    

    public void BufferInput(string input)
    {
        long[] nums = SerialUtility.AsciiToInts(input);
        BufferCountRegister.Content += nums.Length;
        
        inputAccessPool.WaitOne();

        foreach (var num in nums)
        {
            InputBuffer.SendAsync(num);
        }
        
        inputAccessPool.Release();
        
        OnInputBuffered?.Invoke();
    }
    public async Task<long> GetInput()
    {
        long res;
        
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
    public void FlushInputBuffer()
    {
        InputBuffer.TryReceiveAll(out _);
        BufferCountRegister.Content = 0;
    }

    public void BufferKeyInput(Key key)
    {
        inputAccessPool.WaitOne();
        
        KeyInputBuffer.SendAsync(key);
        
        inputAccessPool.Release();
        
        OnKeyInputBuffered?.Invoke();
    }
    public async Task<Key> GetKeyInput()
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
    public void FlushKeyInputBuffer() => KeyInputBuffer.TryReceiveAll(out _);
    
    public void BufferOutput(int[] output)
    {
        string parsed = SerialUtility.IntsToAscii(output);
        
        outputAccessPool.WaitOne();

        OutputBuffer.Enqueue(parsed);

        outputAccessPool.Release();
        
        OnOutputBuffered?.Invoke();
    }
    public string GetOutput()
    {
        outputAccessPool.WaitOne();

        string output = OutputBuffer.Count == 0 ? "" : OutputBuffer.Dequeue();

        outputAccessPool.Release();

        return output;
    }
    public void FlushOutputBuffer() => OutputBuffer.Clear();

    public void FullFlush()
    {
        FlushInputBuffer();
        FlushKeyInputBuffer();
        FlushOutputBuffer();
    }
}