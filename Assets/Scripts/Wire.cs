using Assets.Scripts;
using UnityEngine;

public class Wire : MonoBehaviour
{
    public int output;

    private Renderer _renderer;

    [SerializeField]
    private LogicOperator _inputOperator;

    public IHaveInput InputOperator
    {
        get => _inputOperator;
        set
        {
            //if (_inputOperator != null)
            //{
            //    _inputOperator.OutputUpdated -= UpdateOutput;
            //}

            //_inputOperator = value;
            //_inputOperator.OutputUpdated += UpdateOutput;
            //if (OutputOperator != null)
            //{
            //    //Fix this - do better
            //    if (output == 1)
            //    {
            //        _inputOperator.OutputUpdated += OutputOperator.SetInput1;
            //    }
            //    else if (output == 2)
            //    {
            //        _inputOperator.OutputUpdated += OutputOperator.SetInput2;
            //    }

            //}
        }
    }

    [SerializeField]
    private LogicOperator _outputOperator;

    public IHaveInput OutputOperator
    {
        get => _outputOperator;
        set
        {
            //if (InputOperator != null)
            //{
            //    InputOperator.OutputUpdated -= OutputOperator.SetInput1;
            //    InputOperator.OutputUpdated -= OutputOperator.SetInput2;
            //}

            //_outputOperator = value;
            //if (InputOperator != null)
            //{
            //    if (output == 1)
            //    {
            //        InputOperator.OutputUpdated += OutputOperator.SetInput1;
            //    }
            //    else if (output == 2)
            //    {
            //        InputOperator.OutputUpdated += OutputOperator.SetInput2;
            //    }
            //}
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();

        //if (OutputOperator != null && InputOperator != null)
        //{
        //    InputOperator.OutputUpdated += UpdateOutput;
        //    if (output == 1)
        //    {
        //        InputOperator.OutputUpdated += OutputOperator.SetInput1;
        //    }
        //    else if (output == 2)
        //    {
        //        InputOperator.OutputUpdated += OutputOperator.SetInput2;
        //    }
            
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateOutput(object sender, bool output)
    {
        if (output)
        {
            _renderer.material.color = Constants.LogicGateOnColour;
        }
        else
        {
            _renderer.material.color = Constants.LogicGateOffColour;
        }
    }

    //public static Wire Create(LogicOutput input, LogicOperator logicOperator)
    //{
    //    var newObject = Instantiate(_prefab) as GameObject;
    //    var newWire = newObject.GetComponent<Wire>();

    //    newWire.InputOperator = input;
    //    newWire.OutputOperator = logicOperator;

    //    return newWire;
    //}
}
