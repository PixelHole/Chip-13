namespace Chippie_Lite_WPF.Computer.Components;

public struct ConditionContainer
{
    public string Number { get; set; }
    public ConditionOperator? Operator { get; set; }


    public ConditionContainer(string number, ConditionOperator? @operator)
    {
        Number = number;
        Operator = @operator;
    }
}