using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class LogicFace : Face
    {
        [SerializeField]
        private LogicConnectorMode _mode;

        public LogicConnectorMode Mode => _mode;

        [SerializeField]
        private LogicInputPosition _position;

        public LogicInputPosition Position
        {
            get => _position;
            private set => _position = value;
        }

        public IHaveInput HaveInput { get; private set; }

        public IHaveOutput HaveOutput { get; private set; }

        protected override void Start()
        {
            base.Start();
            if(Mode == LogicConnectorMode.Input)
            {
                var inputParent = GetComponentInParent<IHaveInput>();
                HaveInput = inputParent;
            }
            else if(Mode == LogicConnectorMode.Output)
            {
                var outputParent = GetComponentInParent<IHaveOutput>();
                HaveOutput = outputParent;
            }
        }

        public void ConnectTo(LogicFace otherFace)
        {
            if(Mode == otherFace.Mode)
            {
                Debug.LogError("Cannot connect two faces of the same type");
                throw new InvalidOperationException("Cannot connect two faces of the same type");
            }

            if (Mode == LogicConnectorMode.Input && otherFace.HaveOutput != HaveInput)
            {
                otherFace.ConnectTo(this);
            }
            else if (Mode == LogicConnectorMode.Output && otherFace.HaveInput != HaveOutput)
            {
                HaveOutput.OutputUpdated += (sender, e) =>
                {
                    otherFace.HaveInput.SetInput(e, otherFace);
                };
                otherFace.HaveInput.SetInput(HaveOutput.Output, otherFace);
                Debug.Log($"connected {HaveOutput.ToString()}'s output to {otherFace.HaveInput.ToString()}'s input");
            }
        }
    }
}
