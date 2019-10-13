using UnityEngine;

namespace Assets.Scripts
{
    public interface ICanBePlaced
    {
        Vector3Int GridPos { get; set; }

        Transform transform { get; }
    }
}
