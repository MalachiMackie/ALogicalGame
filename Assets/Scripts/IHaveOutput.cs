using System;

namespace Assets.Scripts
{
    public interface IHaveOutput : ICanBePlaced
    {
        LogicFace OutputFace { get; }

        event EventHandler<bool> OutputUpdated;

        bool Output { get; }
    }
}
