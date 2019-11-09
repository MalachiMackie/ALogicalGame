using Assets.Scripts;
using System;
using UnityEngine;

public class Clock : MonoBehaviour, IHaveOutput
{
    [SerializeField]
    private float Period = 1;

    private Renderer _renderer;

    private bool waiting;

    private bool _output;

    public bool Output
    {
        get => _output;
        private set
        {
            if (value)
            {
                _renderer.material.color = Constants.LogicGateOnColour;
            }
            else
            {
                _renderer.material.color = Constants.LogicGateOffColour;
            }
            _output = value;

            OutputUpdated?.Invoke(this, _output);
        }
    }

    [SerializeField]
    private LogicFace _outputFace;

    public LogicFace OutputFace => _outputFace;

    [SerializeField]
    private Vector3Int _gridPos;

    public Vector3Int GridPosition
    {
        get => _gridPos;
        set => _gridPos = value;
    }

    [SerializeField]
    private GridDirection _direction;

    public GridDirection Direction => _direction;

    public bool HasOutputHandler => OutputUpdated != null;

    public event EventHandler<bool> OutputUpdated;

    public void SetOutput()
    {
        waiting = true;
        StartCoroutine(Utils.DoAfterSeconds(Period / 2, () => { waiting = false; Output = !Output; }));
    }

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (!waiting)
        {
            SetOutput();
        }
    }
}
