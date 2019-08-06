using System;
using UnityEngine;

public abstract class LogicOperator : MonoBehaviour
{
    private bool output;

    private bool _output
    {
        get => output;
        set {
            output = value;
            OutputUpdated.Invoke(this, output);
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

    private bool inputValue1;

    private bool _inputValue1
    {
        get => inputValue1;
        set
        {
            inputValue1 = value;
            _output = CalculateOutput(_inputValue1, _inputValue2);
        }
    }

    private bool inputValue2;

    private bool _inputValue2
    {
        get => inputValue2;
        set
        {
            inputValue2 = value;
            _output = CalculateOutput(_inputValue1, _inputValue2);
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

    protected abstract bool CalculateOutput(bool input1, bool input2);

    public event EventHandler<bool> OutputUpdated;

    private void Input1Updated(object sender, bool e)
    {
        _inputValue1 = e;
    }

    private void Input2Updated(object sender, bool e)
    {
        _inputValue2 = e;
    }
}
