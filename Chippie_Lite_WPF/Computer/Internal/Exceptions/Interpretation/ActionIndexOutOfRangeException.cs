using System.Runtime.Serialization;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;

public class ActionIndexOutOfRangeException : InstructionInterpretationException
{
    public int GivenIndex { get; private set; }
    public int ArgumentCount { get; private set; }


    public ActionIndexOutOfRangeException(int givenIndex, int argumentCount)
    {
        GivenIndex = givenIndex;
        ArgumentCount = argumentCount;
    }
    protected ActionIndexOutOfRangeException(SerializationInfo info, StreamingContext context, int givenIndex, int argumentCount) : base(info, context)
    {
        GivenIndex = givenIndex;
        ArgumentCount = argumentCount;
    }
    public ActionIndexOutOfRangeException(string? message, int givenIndex, int argumentCount) : base(message)
    {
        GivenIndex = givenIndex;
        ArgumentCount = argumentCount;
    }
    public ActionIndexOutOfRangeException(string? message, Exception? innerException, int givenIndex, int argumentCount) : base(message, innerException)
    {
        GivenIndex = givenIndex;
        ArgumentCount = argumentCount;
    }
}