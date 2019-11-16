using Assets.Scripts.Interfaces;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// The floor Grid
        /// </summary>
        private FloorGrid _floorGrid;

        /// <summary>
        /// Whether the player can move
        /// </summary>
        public static bool CanMove = true;

        /// <summary>
        /// Initialize the level
        /// </summary>
        private void InitLevel()
        {
            foreach (INeedGameManger needGameManager in FindObjectsOfType<MonoBehaviour>().OfType<INeedGameManger>())
            {
                needGameManager.GameManager = this;
            }
            _floorGrid = FindObjectOfType<FloorGrid>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        /// <summary>
        /// Start the GameManager
        /// </summary>
        public void Start()
        {
            InitLevel();
        }

        /// <summary>
        /// Run every frame
        /// </summary>
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CanMove = !CanMove;
            }
        }

        /// <summary>
        /// Get the floor piece at a position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public FloorFace GetFloorFaceAtPosition(int x, int z)
        {
            return _floorGrid.GetFloorFaceAtPosition(x, z);
        }

        /// <summary>
        /// Try place a component on th grid
        /// </summary>
        /// <param name="component"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool TryPlaceFloorComponent(ICanBePlaced component, Vector3Int position)
        {
            return _floorGrid.TryPlaceGridComponent(component, position);
        }
    }
}
