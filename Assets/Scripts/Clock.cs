using System;
using System.Threading.Tasks;

public class Clock : LogicOperator
{
    public float Period = 1;

    private bool waiting;

    protected async override Task CalculateOutputAsync(bool input1, bool input2)
    {
        waiting = true;
        await Task.Delay(TimeSpan.FromSeconds(Period / 2));
        print(DateTime.Now);
        waiting = false;
        _output = !_output;
    }

    protected override void CalculateOutput(bool input1, bool input2)
    {
        throw new InvalidOperationException("Clock does not have any outputs");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    async void Update()
    {
        if (!waiting)
        {
            await CalculateOutputAsync(InputValue1, InputValue2);
        }
    }
}
