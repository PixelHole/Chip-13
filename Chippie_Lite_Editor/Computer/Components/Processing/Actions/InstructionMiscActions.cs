using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Arguments;
using Chippie_Lite_WPF.Computer.Utility;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Chippie_Lite_WPF.Computer.Components.Actions;

public static class InstructionMiscActions
{
    public static void Beep(InstructionAction action)
    {
        ActionUtility.CheckArgumentAndActionIndexCount(action, 2);
        
        int freq = (int)ActionUtility.InterpretRead(action.Arguments[0], action.Instruction);
        int dur = (int)ActionUtility.InterpretRead(action.Arguments[1], action.Instruction);
        
        Console.Beep(freq, dur);
    }
    public static void Wait(InstructionAction action)
    {
        ActionUtility.CheckArgumentAndActionIndexCount(action, 1);
        
        int dur = (int)ActionUtility.InterpretRead(action.Arguments[0], action.Instruction);
        
        Thread.Sleep(dur);
    }

    public static void AudioWave(InstructionAction action)
    {
        ActionUtility.CheckArgumentAndActionIndexCount(action, 2);
        
        int freq = (int)ActionUtility.InterpretRead(action.Arguments[0], action.Instruction);
        int dur = (int)ActionUtility.InterpretRead(action.Arguments[1], action.Instruction);
        SignalGeneratorType type = (SignalGeneratorType)(ActionUtility.InterpretRead(action.Arguments[2], action.Instruction) % 7);
        
        PlayAudioWave(freq, dur, type);
    }
    private static void PlayAudioWave(int frequency, int duration, SignalGeneratorType type)
    {
        var sineWave = new SignalGenerator()
        {
            Gain = 0.2f,
            Frequency = frequency,
            Type = type
        }.Take(TimeSpan.FromMilliseconds(duration));

        WaveOutEvent wo = new WaveOutEvent();
        wo.Init(sineWave);
        wo.Play();
        wo.PlaybackStopped += (_, _) => wo.Dispose();
    }
}