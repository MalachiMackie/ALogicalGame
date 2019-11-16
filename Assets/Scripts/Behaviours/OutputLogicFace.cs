using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    public class OutputLogicFace : FloorFace
    {
        /// <summary>
        /// Gets or sets the output component
        /// </summary>
        public IHaveOutput OutputComponent { get; private set; }

        /// <summary>
        /// Initialize the Output Logic Face
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            IHaveOutput outputParent = GetComponentInParent<IHaveOutput>();
            OutputComponent = outputParent;
            Neighbours.Add(OutputComponent);

            switch (OutputComponent.Direction)
            {
                case GridDirection.North:
                    {
                        GridPosition = new Vector3Int(OutputComponent.GridPosition.x, 0, OutputComponent.GridPosition.z + 1);
                        break;
                    }
                case GridDirection.East:
                    {
                        GridPosition = new Vector3Int(OutputComponent.GridPosition.x + 1, 0, OutputComponent.GridPosition.z);
                        break;
                    }
                case GridDirection.South:
                    {
                        GridPosition = new Vector3Int(OutputComponent.GridPosition.x, 0, OutputComponent.GridPosition.z - 1);
                        break;
                    }
                case GridDirection.West:
                    {
                        GridPosition = new Vector3Int(OutputComponent.GridPosition.x - 1, 0, OutputComponent.GridPosition.z);
                        break;
                    }
            }
        }
    }
}
