using System;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IHaveOutput : ICanBePlaced
    {
        /// <summary>
        /// Event for updating the output
        /// </summary>
        event EventHandler<bool> OutputUpdated;

        /// <summary>
        /// The output value
        /// </summary>
        bool Output { get; }

        /// <summary>
        /// Gets the Renderer
        /// </summary>
        Renderer Renderer { get; }

        /// <summary>
        /// Sets the material colour
        /// </summary>
        void SetMaterialColourFromOutput();
    }
}
