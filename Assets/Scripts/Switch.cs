using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Switch : MonoBehaviour, IHaveOutput
    {
        [SerializeField]
        private bool SwitchOn;

        private bool _output;

        private Renderer _renderer;

        [SerializeField]
        private LogicFace _outputFace;

        public LogicFace OutputFace
        {
            get => _outputFace;
            private set => _outputFace = value;
        }

        public bool Output
        {
            get => _output;
            private set
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

        private void SetOutput()
        {
            Output = SwitchOn;
        }

        public event EventHandler<bool> OutputUpdated;

        public void Start()
        {
            _renderer = GetComponent<Renderer>();
            SetOutput();
        }

        public void Update()
        {

        }

        public void SetOutputFace(LogicFace outputFace)
        {
            _outputFace = outputFace;
        }
    }
}
