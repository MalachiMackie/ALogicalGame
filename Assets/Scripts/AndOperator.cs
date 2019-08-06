public class AndOperator : LogicOperator
{
    protected override bool CalculateOutput(bool input1, bool input2)
    {
        return input1 && input2;
    }
}
