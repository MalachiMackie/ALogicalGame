using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class LogicFace : MonoBehaviour, ICanBeLookedAt
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

        private MeshRenderer MeshRenderer { get; set; }

        public IHaveInput HaveInput { get; private set; }

        public IHaveOutput HaveOutput { get; private set; }

        private bool _lookedAt;

        [SerializeField]
        private Material _lookingAtMat;

        [SerializeField]
        private Material _notLookingAtMat;

        public void Start()
        {
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

            MeshRenderer = GetComponent<MeshRenderer>();
        }

        public void StartLookingAt()
        {
            if (!_lookedAt)
            {
                _lookedAt = true;
                MeshRenderer.material = _lookingAtMat;
            }
        }

        public void StopLookingAt()
        {
            _lookedAt = false;
            MeshRenderer.material = _notLookingAtMat;
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
