using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class Constants
    {
        /// <summary>
        /// Colour for the on position
        /// </summary>
        public static Color LogicGateOnColour = new Color(0, 1, 0);

        /// <summary>
        /// Colour for the off position
        /// </summary>
        public static Color LogicGateOffColour = new Color(1, 0, 0);

        /// <summary>
        /// the Unity Input name for the Horizontal Look axis
        /// </summary>
        public static string HoriztonalLookAxis = "HorizontalLook";

        /// <summary>
        /// the Unity Input name for the Vertical Look axis
        /// </summary>
        public static string VerticalLookAxis = "VerticalLook";
    }
}
