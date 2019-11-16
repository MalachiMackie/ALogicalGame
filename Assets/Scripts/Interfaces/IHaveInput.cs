using Assets.Scripts.Behaviours;

namespace Assets.Scripts.Interfaces
{
    public interface IHaveInput : ICanBePlaced
    {
        /// <summary>
        /// Sets the input
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sender">The input face that is setting the value</param>
        void SetInput(bool input, InputLogicFace sender);
    }
}
