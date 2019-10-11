using System;

namespace Assets.Scripts
{
    public interface IHaveOutput
    {
        LogicFace OutputFace { get; }

        event EventHandler<bool> OutputUpdated;

        bool Output { get; }
    }
}
