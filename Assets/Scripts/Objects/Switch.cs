using Assets.Scripts.Behaviours;
using Assets.Scripts.Enums;
using Assets.Scripts.Helpers;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class Switch : MonoBehaviour, IHaveOutput
    {
        /// <summary>
        /// Whether the switch is on
        /// </summary>
        [SerializeField]
        private bool SwitchOn;

        /// <summary>
        /// Gets or sets the Renderer
        /// </summary>
        public Renderer Renderer { get; private set; }

        /// <summary>
        /// Backing field for <see cref="Output"/>
        /// </summary>
        private bool _output;

        /// <summary>
        /// Gets or sets the Output
        /// </summary>
        public bool Output
        {
            get => _output;
            private set
            {
                _output = value;
                SetMaterialColourFromOutput();

                OutputUpdated?.Invoke(this, value);
            }
        }

        /// <summary>
        /// Backing field for <see cref="GridPosition"/>
        /// </summary>
        [SerializeField]
        private Vector3Int _gridPosition;

        /// <summary>
        /// Gets or sets the Grid Position
        /// </summary>
        public Vector3Int GridPosition
        {
            get => _gridPosition;
            set => _gridPosition = value;
        }

        /// <summary>
        /// Backing field for <see cref="Direction"/>
        /// </summary>
        [SerializeField]
        private GridDirection _direction;

        /// <summary>
        /// Gets the Direction
        /// </summary>
        public GridDirection Direction => _direction;

        /// <summary>
        /// Gets the list of logic faces
        /// </summary>
        public List<FloorFace> MyFaces { get; } = new List<FloorFace>();

        /// <summary>
        /// Event for handling when the output is updated
        /// </summary>
        public event EventHandler<bool> OutputUpdated;

        /// <summary>
        /// Sets the Output
        /// </summary>
        private void SetOutput()
        {
            Output = SwitchOn;
        }

        /// <summary>
        /// Runs before Unity Start()
        /// </summary>
        public void Awake()
        {
            Renderer = GetComponent<Renderer>();
            SetOutput();
            MyFaces.Add(GetComponentInChildren<OutputLogicFace>());
        }

        /// <summary>
        /// Sets the Grid Position
        /// </summary>
        /// <param name="position"></param>
        public void SetGridPosition(Vector3Int position)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set the Material colour based on the output
        /// </summary>
        public void SetMaterialColourFromOutput()
        {
            Renderer.material.color = Output ? Constants.LogicGateOnColour : Constants.LogicGateOffColour;
        }
    }
}
