using System;
using System.Threading.Tasks;
using UnityEngine;

public abstract class LogicOperator : MonoBehaviour
{
    private bool output;

    protected bool _output
    {
        get => output;
        set
        {
            output = value;
            OutputUpdated?.Invoke(this, output);
            if (output)
            {
                GetComponent<Renderer>().material.color = new Color(0, 1, 0);
            }
            else
            {
                GetComponent<Renderer>().material.color = new Color(1, 0, 0);
            }
            
        }
    }

    private LogicOperator _inputOperator1;

    public LogicOperator InputOperator1
    {
        get => _inputOperator1;
        set
        {
            _inputOperator1 = value;
            _inputOperator1.OutputUpdated += Input1Updated;
        }
    }

    private LogicOperator _inputOperator2;

    public LogicOperator InputOperator2
    {
        get => _inputOperator2;
        set
        {
            _inputOperator2 = value;
            _inputOperator2.OutputUpdated += Input2Updated;
        }
    }

    private bool _inputValue1;

    public bool InputValue1
    {
        get => _inputValue1;
        set
        {
            _inputValue1 = value;
            CalculateOutput(InputValue1, InputValue2);
        }
    }

    private bool _inputValue2;

    public bool InputValue2
    {
        get => _inputValue2;
        set
        {
            _inputValue2 = value;
            CalculateOutput(InputValue1, InputValue2);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {

    }

    public bool GetOutput() => _output;

    protected abstract void CalculateOutput(bool input1, bool input2);

    protected abstract Task CalculateOutputAsync(bool input1, bool input2);

    public event EventHandler<bool> OutputUpdated;

    private void Input1Updated(object sender, bool e)
    {
        InputValue1 = e;
    }

    private void Input2Updated(object sender, bool e)
    {
        InputValue2 = e;
    }
}
