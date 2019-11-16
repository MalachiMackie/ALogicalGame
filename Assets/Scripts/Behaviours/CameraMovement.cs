using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    public class CameraMovement : MonoBehaviour
    {
        /// <summary>
        /// the Player transform
        /// </summary>
        [SerializeField]
        private Transform _playerTransform;

        /// <summary>
        /// Update each frame
        /// </summary>
        public void Update()
        {
            //Update position to player's position
            transform.position = _playerTransform.transform.position + new Vector3(0, 0.5f, 0);

            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, _playerTransform.eulerAngles.y, _playerTransform.eulerAngles.z);
            float xRotation = Input.GetAxis(Constants.VerticalLookAxis) * 0.1f * -1;

            if (GameManager.CanMove)
            {
                transform.Rotate(xRotation, 0, 0);
            }
        }
    }
}
