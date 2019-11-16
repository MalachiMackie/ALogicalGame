using Assets.Scripts.Behaviours;

namespace Assets.Scripts.Objects.Operators
{
    public class AndOperator : LogicOperator
    {
        /// <summary>
        /// Calculate the output for the And Operator based on two inputs
        /// </summary>
        /// <param name="input1"></param>
        /// <param name="input2"></param>
        /// <returns></returns>
        protected override void CalculateOutput(bool input1, bool input2)
        {
            Output = input1 && input2;
        }
    }
}
