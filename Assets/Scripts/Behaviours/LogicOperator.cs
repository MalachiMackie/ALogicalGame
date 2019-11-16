using Assets.Scripts.Enums;
using Assets.Scripts.Helpers;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    public abstract class LogicOperator : MonoBehaviour, IHaveInput, IHaveOutput
    {
        /// <summary>
        /// The value of the primary input value
        /// </summary>
        private bool _primaryInputValue;

        /// <summary>
        /// The value of the secondary input value
        /// </summary>
        private bool _secondaryInputValue;

        /// <summary>
        /// The primary input face
        /// </summary>
        private InputLogicFace _primaryInputFace;

        /// <summary>
        /// The secondary input face
        /// </summary>
        private InputLogicFace _secondaryInputFace;

        /// <summary>
        /// Output Updated Event handler
        /// </summary>
        public event EventHandler<bool> OutputUpdated;

        /// <summary>
        /// Backing field for <see cref="Output"/>
        /// </summary>
        private bool _output;

        /// <summary>
        /// Gets or sets the Output Value
        /// </summary>
        public bool Output
        {
            get => _output;
            protected set
            {
                _output = value;
                OutputUpdated?.Invoke(this, _output);
                SetMaterialColourFromOutput();
            }
        }

        /// <summary>
        /// Gets the Renderer
        /// </summary>
        public Renderer Renderer { get; private set; }

        /// <summary>
        /// Backing field for <see cref="GridPosition"/>
        /// </summary>
        [SerializeField]
        private Vector3Int _gridPos;

        /// <summary>
        /// Gets or sets the Grid Position
        /// </summary>
        public Vector3Int GridPosition
        {
            get => _gridPos;
            set => _gridPos = value;
        }

        /// <summary>
        /// the backing field for <see cref="Direction"/>
        /// </summary>
        [SerializeField]
        private GridDirection _direction;

        /// <summary>
        /// Get the Grid Direction
        /// </summary>
        public GridDirection Direction => _direction;

        /// <summary>
        /// Gets My faces
        /// </summary>
        public List<FloorFace> MyFaces { get; } = new List<FloorFace>();

        /// <summary>
        /// Calculate the output
        /// </summary>
        /// <param name="input1"></param>
        /// <param name="input2"></param>
        /// <returns></returns>
        protected abstract void CalculateOutput(bool input1, bool input2);

        /// <summary>
        /// Unity Start Method
        /// </summary>
        public virtual void Start()
        {
            Renderer = GetComponent<Renderer>();
            SetMaterialColourFromOutput();

            InputLogicFace[] inputFaces = GetComponentsInChildren<InputLogicFace>();
            _primaryInputFace = inputFaces.Single(x => x.Position == LogicInputPosition.First);
            _secondaryInputFace = inputFaces.Single(x => x.Position == LogicInputPosition.Second);

            MyFaces.Add(GetComponentInChildren<OutputLogicFace>());
            MyFaces.Add(_primaryInputFace);
            MyFaces.Add(_secondaryInputFace);
        }

        /// <summary>
        /// Set input value
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sender"></param>
        public void SetInput(bool input, InputLogicFace sender)
        {
            if (sender == _primaryInputFace)
            {
                _primaryInputValue = input;
            }
            else if (sender == _secondaryInputFace)
            {
                _secondaryInputValue = input;
            }
            CalculateOutput(_primaryInputValue, _secondaryInputValue);
        }

        /// <summary>
        /// Set the Grid Position
        /// </summary>
        /// <param name="position"></param>
        public void SetGridPosition(Vector3Int position)
        {
            //TODO
            throw new NotImplementedException();
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
