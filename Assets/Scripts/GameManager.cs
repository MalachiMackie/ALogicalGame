using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public CameraMode CameraMode = CameraMode.FirstPerson;

        public bool CameraInverted = true;

        public void Start()
        {
            Init();
        }

        private void Init()
        {
            //Do Loading things
            foreach (var needGameManager in FindObjectsOfType<GameObject>().OfType<INeedGameManger>())
            {
                needGameManager.GameManager = this;
            }
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            //Show the level
            
        }
    }
}
