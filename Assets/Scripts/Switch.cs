using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Switch : LogicOutput
    {
        [SerializeField]
        private bool SwitchOn;

        private bool _output;

        private Renderer _renderer;

        public override bool Output
        {
            get => _output;
            protected set
            {
                _output = value;
                if (value)
                {
                    _renderer.material.color = Constants.LogicGateOnColour;
                }
                else
                {
                    _renderer.material.color = Constants.LogicGateOffColour;
                }
                OutputUpdated?.Invoke(this, value);
            }
        }

        public override void SetOutput()
        {
            Output = SwitchOn;
        }

        public override event EventHandler<bool> OutputUpdated;

        protected override void Start()
        {
            _renderer = GetComponent<Renderer>();
            SetOutput();
        }

        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                SwitchOn = !SwitchOn;
                SetOutput();
            }
        }
    }
}
