using Assets.Scripts.Behaviours;
using Assets.Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface ICanBePlaced : IHaveGridPosition
    {
        /// <summary>
        /// Gets the transform of the <see cref="ICanBePlaced"/>
        /// </summary>
        Transform transform { get; }

        /// <summary>
        /// Gets the direction
        /// </summary>
        GridDirection Direction { get; }

        /// <summary>
        /// Gets a list of connected faces
        /// </summary>
        List<FloorFace> MyFaces { get; }

        /// <summary>
        /// Sets the position on the grid
        /// </summary>
        /// <param name="position"></param>
        void SetGridPosition(Vector3Int position);
    }
}
