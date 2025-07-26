using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Arguments.Base;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Chippie_Lite_WPF.Computer.Components.Actions;

public static class InstructionMiscActions
{
    public static void Beep(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 2);
        
        int freq = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        int dur = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        
        Console.Beep(freq, dur);
    }
    public static void Wait(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        int dur = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        
        Thread.Sleep(dur);
    }
    public static void SineWave(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 2);
        
        int freq = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        int dur = InstructionArgument.GetNumber(arguments[action.Indices[1]]);

        PlayAudioWave(freq, dur, SignalGeneratorType.Sin);
    }
    public static void TriangleWave(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 2);
        
        int freq = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        int dur = InstructionArgument.GetNumber(arguments[action.Indices[1]]);

        PlayAudioWave(freq, dur, SignalGeneratorType.Triangle);
    }
    public static void SquareWave(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 2);
        
        int freq = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        int dur = InstructionArgument.GetNumber(arguments[action.Indices[1]]);

        PlayAudioWave(freq, dur, SignalGeneratorType.Square);
    }
    public static void SawToothWave(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 2);
        
        int freq = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        int dur = InstructionArgument.GetNumber(arguments[action.Indices[1]]);

        PlayAudioWave(freq, dur, SignalGeneratorType.SawTooth);
    }
    public static void PinkNoise(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 2);
        
        int freq = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        int dur = InstructionArgument.GetNumber(arguments[action.Indices[1]]);

        PlayAudioWave(freq, dur, SignalGeneratorType.Pink);
    }
    public static void WhiteNoise(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 2);
        
        int freq = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        int dur = InstructionArgument.GetNumber(arguments[action.Indices[1]]);

        PlayAudioWave(freq, dur, SignalGeneratorType.White);
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