using UnityEngine;

namespace Assets.Scripts
{
    public static class Constants
    {
        public static Color LogicGateOnColour = new Color(0, 1, 0);

        public static Color LogicGateOffColour = new Color(1, 0, 0);

        public static Vector3 ThirdPersonCameraPositionOffset = new Vector3(0, 1.25f, -4);

        public static Vector3 FirstPersonCameraPositionOffset = new Vector3(0, 0.5f, 0);

        public static Vector3 RotationOffset = new Vector3(20, 0, 0);

        public static string HoriztonalLookAxis = "HorizontalLook";

        public static string VerticalLookAxis = "VerticalLook";
    }
}
