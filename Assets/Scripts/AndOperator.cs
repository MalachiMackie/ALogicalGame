using System;
using System.Threading.Tasks;

public class AndOperator : LogicOperator
{
    protected override void CalculateOutput(bool input1, bool input2)
    {
        _output = input1 && input2;
    }

    protected override Task CalculateOutputAsync(bool input1, bool input2)
    {
        throw new NotImplementedException();
    }
}
