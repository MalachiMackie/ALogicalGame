using UnityEngine;

namespace Assets.Scripts
{
    public interface ICanBePlaced : IHaveGridPosition
    {
        Transform transform { get; }

        GridDirection Direction { get; }
    }
}
