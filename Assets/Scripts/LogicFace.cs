using UnityEngine;

namespace Assets.Scripts
{
    public class LogicFace : FloorFace
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

        protected override void Awake()
        {
            base.Awake();

            if (Mode == LogicConnectorMode.Input)
            {
                var inputParent = GetComponentInParent<IHaveInput>();
                HaveInput = inputParent;
                Neighbours.Add(HaveInput);

                int positionModifier = 1;
                if (Position == LogicInputPosition.Second)
                {
                    positionModifier = -1;
                }
                switch (HaveInput.Direction)
                {
                    case GridDirection.North:
                        {
                            GridPosition = new Vector3Int(HaveInput.GridPosition.x + positionModifier, 0, HaveInput.GridPosition.z);
                            break;
                        }
                    case GridDirection.East:
                        {
                            GridPosition = new Vector3Int(HaveInput.GridPosition.x, 0, HaveInput.GridPosition.z + (-1 * positionModifier));
                            break;
                        }
                    case GridDirection.South:
                        {
                            GridPosition = new Vector3Int(HaveInput.GridPosition.x + positionModifier, 0, HaveInput.GridPosition.z);
                            break;
                        }
                    case GridDirection.West:
                        {
                            GridPosition = new Vector3Int(HaveInput.GridPosition.x, 0, HaveInput.GridPosition.z + positionModifier);
                            break;
                        }
                }
            }
            else if (Mode == LogicConnectorMode.Output)
            {
                var outputParent = GetComponentInParent<IHaveOutput>();
                HaveOutput = outputParent;
                Neighbours.Add(HaveOutput);

                switch (HaveOutput.Direction)
                {
                    case GridDirection.North:
                        {
                            GridPosition = new Vector3Int(HaveOutput.GridPosition.x, 0, HaveOutput.GridPosition.z + 1);
                            break;
                        }
                    case GridDirection.East:
                        {
                            GridPosition = new Vector3Int(HaveOutput.GridPosition.x + 1, 0, HaveOutput.GridPosition.z);
                            break;
                        }
                    case GridDirection.South:
                        {
                            GridPosition = new Vector3Int(HaveOutput.GridPosition.x, 0, HaveOutput.GridPosition.z - 1);
                            break;
                        }
                    case GridDirection.West:
                        {
                            GridPosition = new Vector3Int(HaveOutput.GridPosition.x - 1, 0, HaveOutput.GridPosition.z);
                            break;
                        }
                }
            }
            
        }
    }
}
