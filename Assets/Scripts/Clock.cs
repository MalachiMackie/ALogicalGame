using Assets.Scripts;
using System;
using UnityEngine;

public class Clock : LogicOutput
{
    [SerializeField]
    private float Period = 1;

    private Renderer _renderer;

    private bool waiting;

    private bool _output;

    public override bool Output
    {
        get => _output;
        protected set
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

    public override event EventHandler<bool> OutputUpdated;

    public override void SetOutput()
    {
        waiting = true;
        StartCoroutine(Utils.DoAfterSeconds(Period / 2, () => { waiting = false; Output = !Output; }));
    }

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!waiting)
        {
            SetOutput();
        }
    }
}
