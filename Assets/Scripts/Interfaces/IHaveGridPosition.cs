using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IHaveGridPosition
    {
        /// <summary>
        /// Gets or sets the GridPosition
        /// </summary>
        Vector3Int GridPosition { get; set; }
    }
}
