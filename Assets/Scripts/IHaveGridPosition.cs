using UnityEngine;

namespace Assets.Scripts
{
    public interface IHaveGridPosition
    {
        Vector3Int GridPosition { get; set; }
    }
}
