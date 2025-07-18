using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.Computer.Components;

public static class SerialIO
{
    private static readonly Semaphore inputAccessPool = new Semaphore(1, 1);
    private static readonly Semaphore outputAccessPool = new Semaphore(1, 1);
    private static Queue<int> InputBuffer { get; set; } = new Queue<int>();
    private static Queue<string> OutputBuffer { get; set; } = new Queue<string>();
    
    public static bool WaitingForInput { get; private set; }


    public delegate void WaitForInputStartAction();
    public static event WaitForInputStartAction OnWaitForInputStart;
    
    public delegate void WaitForInputEndAction();
    public static event WaitForInputEndAction OnWaitForInputEnd;

    public delegate void OutputBufferedAction();
    public static event OutputBufferedAction OnOutputBuffered;
    
    public delegate void InputBufferedAction();
    public static event InputBufferedAction OnInputBuffered;
    

    public static void BufferInput(string input)
    {
        input += (char)0;
        int[] nums = SerialUtility.AsciiToInts(input);
        
        inputAccessPool.WaitOne();

        foreach (var num in nums)
        {
            InputBuffer.Enqueue(num);
        }
        
        inputAccessPool.Release();
        
        OnInputBuffered?.Invoke();
    }
    public static int GetInput()
    {
        if (InputBuffer.Count == 0)
        {
            WaitingForInput = true;
            OnWaitForInputStart?.Invoke();

            while (InputBuffer.Count == 0) { }

            WaitingForInput = false;
            OnWaitForInputEnd?.Invoke();
        }

        inputAccessPool.WaitOne();

        var res = InputBuffer.Dequeue();

        inputAccessPool.Release();

        return res;
    }
    public static void FlushInput() => InputBuffer.Clear();

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
    public static void FlushOutput() => OutputBuffer.Clear();

    public static void FullFlush()
    {
        FlushInput();
        FlushOutput();
    }
}