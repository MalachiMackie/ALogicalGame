using Assets.Scripts;
using System;
using System.Threading.Tasks;
using UnityEngine;

public abstract class LogicOperator : MonoBehaviour, IHaveInput, IHaveOutput, ICanBePlaced
{
    private bool _output;

    private Renderer _renderer;

    public bool Output
    {
        get => _output;
        protected set
        {
            _output = value;
            OutputUpdated?.Invoke(this, _output);
            SetColourFromOutput();
        }
    }

    private bool _primaryInputValue;

    public bool PrimanyInputValue
    {
        get => _primaryInputValue;
        set
        {
            _primaryInputValue = value;
            CalculateOutputAsync(PrimanyInputValue, SecondaryInputValue);
        }
    }

    private bool _secondaryInputValue;

    public bool SecondaryInputValue
    {
        get => _secondaryInputValue;
        set
        {
            _secondaryInputValue = value;
            CalculateOutputAsync(PrimanyInputValue, SecondaryInputValue);
        }
    }

    [SerializeField]
    private LogicFace _primaryInputFace;

    public LogicFace PrimaryInputFace => _primaryInputFace;

    [SerializeField]
    private LogicFace _secondaryInputFace;

    public LogicFace SecondaryInputFace => _secondaryInputFace;

    [SerializeField]
    private LogicFace _outputFace;

    public LogicFace OutputFace => _outputFace;

    [SerializeField]
    private Vector3Int _gridPos;

    public Vector3Int GridPos
    {
        get => _gridPos;
        set => _gridPos = value;
    }

    protected virtual void Start()
    {
        _renderer = GetComponent<Renderer>();
        SetColourFromOutput();
    }

    void Update()
    {

    }

    public bool GetOutput() => Output;

    protected abstract Task CalculateOutputAsync(bool input1, bool input2);

    public event EventHandler<bool> OutputUpdated;

    private void SetColourFromOutput()
    {
        if (Output)
        {
            _renderer.material.color = Constants.LogicGateOnColour;
        }
        else
        {
            _renderer.material.color = Constants.LogicGateOffColour;
        }
    }

    public void SetInput(bool input, LogicFace sender)
    {
        if(sender == PrimaryInputFace)
        {
            PrimanyInputValue = input;
        }
        else if(sender == SecondaryInputFace)
        {
            SecondaryInputValue = input;
        }
    }
}
