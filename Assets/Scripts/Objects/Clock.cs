using Assets.Scripts.Behaviours;
using Assets.Scripts.Enums;
using Assets.Scripts.Helpers;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class Clock : MonoBehaviour, IHaveOutput
    {
        /// <summary>
        /// The period of the clock
        /// </summary>
        [SerializeField]
        private float _period;

        /// <summary>
        /// Whether we are waiting to change the output
        /// </summary>
        private bool _waiting;

        /// <summary>
        /// Gets or sets the Renderer
        /// </summary>
        public Renderer Renderer { get; private set; }

        /// <summary>
        /// The connected Output Logic Face
        /// </summary>
        private OutputLogicFace _outputFace;

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

                OutputUpdated?.Invoke(this, _output);
            }
        }

        /// <summary>
        /// The backing field for <see cref="GridPosition"/>
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
        /// Gets the collection of Logic Faces
        /// </summary>
        public List<FloorFace> MyFaces { get; } = new List<FloorFace>();

        /// <summary>
        /// The event for handling when the output is updated
        /// </summary>
        public event EventHandler<bool> OutputUpdated;

        /// <summary>
        /// Set the output
        /// </summary>
        public void SetOutput()
        {
            _waiting = true;
            StartCoroutine(Utils.DoAfterSeconds(_period / 2, () => { _waiting = false; Output = !Output; }));
        }

        /// <summary>
        /// Gets called before Start()
        /// </summary>
        public void Awake()
        {
            _outputFace = GetComponentInChildren<OutputLogicFace>();
            MyFaces.Add(_outputFace);
            SetGridPosition(GridPosition);
            Renderer = GetComponent<Renderer>();
        }

        /// <summary>
        /// Runs every frame
        /// </summary>
        public void Update()
        {
            if (!_waiting)
            {
                SetOutput();
            }
        }

        /// <summary>
        /// Sets the Grid Position
        /// </summary>
        /// <param name="position"></param>
        public void SetGridPosition(Vector3Int position)
        {
            GridPosition = position;
            switch (Direction)
            {
                case GridDirection.East:
                    {
                        _outputFace.GridPosition = new Vector3Int(position.x + 1, position.y, position.z);
                        break;
                    }
                case GridDirection.West:
                    {
                        _outputFace.GridPosition = new Vector3Int(position.x - 1, position.y, position.z);
                        break;
                    }
                case GridDirection.North:
                    {
                        _outputFace.GridPosition = new Vector3Int(position.x, position.y, position.z + 1);
                        break;
                    }
                case GridDirection.South:
                    {
                        _outputFace.GridPosition = new Vector3Int(position.x, position.y, position.z - 1);
                        break;
                    }
            }
        }

        /// <summary>
        /// Sets the Material Colour
        /// </summary>
        public void SetMaterialColourFromOutput()
        {
            Renderer.material.color = Output ? Constants.LogicGateOnColour : Constants.LogicGateOffColour;
        }
    }
}
