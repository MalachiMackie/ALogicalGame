using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    public class InputLogicFace : FloorFace
    {
        /// <summary>
        /// The Logic Input Position
        /// </summary>
        [SerializeField]
        private LogicInputPosition _position;

        /// <summary>
        /// Gets the Logic Input Position
        /// </summary>
        public LogicInputPosition Position => _position;

        /// <summary>
        /// Gets or sets the Input Component
        /// </summary>
        public IHaveInput InputComponent { get; private set; }

        /// <summary>
        /// Initializes the Input Logic Face
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            IHaveInput inputParent = GetComponentInParent<IHaveInput>();
            InputComponent = inputParent;
            Neighbours.Add(InputComponent);

            int positionModifier = 1;
            if (_position == LogicInputPosition.Second)
            {
                positionModifier = -1;
            }
            switch (InputComponent.Direction)
            {
                case GridDirection.North:
                    {
                        GridPosition = new Vector3Int(InputComponent.GridPosition.x + positionModifier, 0, InputComponent.GridPosition.z);
                        break;
                    }
                case GridDirection.East:
                    {
                        GridPosition = new Vector3Int(InputComponent.GridPosition.x, 0, InputComponent.GridPosition.z + (-1 * positionModifier));
                        break;
                    }
                case GridDirection.South:
                    {
                        GridPosition = new Vector3Int(InputComponent.GridPosition.x + positionModifier, 0, InputComponent.GridPosition.z);
                        break;
                    }
                case GridDirection.West:
                    {
                        GridPosition = new Vector3Int(InputComponent.GridPosition.x, 0, InputComponent.GridPosition.z + positionModifier);
                        break;
                    }
            }
        }
    }
}
