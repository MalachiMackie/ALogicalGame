using System.Threading.Tasks;

public class AndOperator : LogicOperator
{
    protected async override Task CalculateOutputAsync(bool input1, bool input2)
    {
        await Task.Delay(0);
        _output = input1 && input2;
    }
}
