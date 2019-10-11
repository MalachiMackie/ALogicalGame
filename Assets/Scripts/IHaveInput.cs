namespace Assets.Scripts
{
    public interface IHaveInput
    {
        LogicFace PrimaryInputFace { get; }

        LogicFace SecondaryInputFace { get; }

        void SetInput(bool input, LogicFace sender);
    }
}
