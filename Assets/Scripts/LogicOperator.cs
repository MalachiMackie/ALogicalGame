using Assets.Scripts;
using System;
using System.Threading.Tasks;
using UnityEngine;

public abstract class LogicOperator : MonoBehaviour
{
    private bool output;

    private Renderer _renderer;

    protected bool _output
    {
        get => output;
        set
        {
            output = value;
            OutputUpdated?.Invoke(this, output);
            SetColourFromOutput();
        }
    }

    private bool _inputValue1;

    public bool InputValue1
    {
        get => _inputValue1;
        set
        {
            _inputValue1 = value;
            CalculateOutputAsync(InputValue1, InputValue2);
            Debug.Log($"Input 1 set to {value}");
        }
    }

    private bool _inputValue2;

    public bool InputValue2
    {
        get => _inputValue2;
        set
        {
            _inputValue2 = value;
            CalculateOutputAsync(InputValue1, InputValue2);
            Debug.Log($"Input 2 set to {value}");
        }
    }

    protected virtual void Start()
    {
        _renderer = GetComponent<Renderer>();
        SetColourFromOutput();
    }

    void Update()
    {

    }

    public bool GetOutput() => _output;

    protected abstract Task CalculateOutputAsync(bool input1, bool input2);

    public event EventHandler<bool> OutputUpdated;

    private void SetColourFromOutput()
    {
        if (_output)
        {
            _renderer.material.color = Constants.LogicGateOnColour;
        }
        else
        {
            _renderer.material.color = Constants.LogicGateOffColour;
        }
    }

    public void SetInput1(object sender, bool input)
    {
        if (sender is Wire)
        {

        }
        InputValue1 = input;
    }

    public void SetInput2(object sender, bool input)
    {
        if (sender is Wire)
        {
            //check for input position
        }
        InputValue2 = input;
    }
}
