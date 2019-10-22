using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public CameraMode CameraMode = CameraMode.FirstPerson;

        public bool CameraInverted = true;

        private FloorGrid _floorGrid;

        public static bool CanMove = true;

        public void Start()
        {
            InitLevel();
        }

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                CanMove = !CanMove;
            }
        }

        private void InitLevel()
        {
            foreach (var needGameManager in FindObjectsOfType<GameObject>().OfType<INeedGameManger>())
            {
                needGameManager.GameManager = this;
            }
            _floorGrid = FindObjectOfType<FloorGrid>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;   
        }

        public FloorFace GetFloorFaceAtPosition(int x, int z) => _floorGrid.GetFloorFaceAtPosition(x, z);
    }
}
