using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    public class Player : MonoBehaviour
    {
        /// <summary>
        /// The player rotating speed
        /// </summary>
        [SerializeField]
        private float _rotateSpeed;

        /// <summary>
        /// The player movement force
        /// </summary>
        [SerializeField]
        private float _moveForce;

        /// <summary>
        /// The player max speed
        /// </summary>
        [SerializeField]
        private float _maxSpeed;

        /// <summary>
        /// The max distance the player can look
        /// </summary>
        [SerializeField]
        private float _maxLookDistance;

        /// <summary>
        /// The rigidbody
        /// </summary>
        private Rigidbody _rigidBody;

        /// <summary>
        /// Unity Start Method
        /// </summary>
        public void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();

            PlayerMovement movement = gameObject.AddComponent<PlayerMovement>();
            PlayerView view = gameObject.AddComponent<PlayerView>();

            movement.InitProperties(_rigidBody, _maxSpeed, _rotateSpeed, _moveForce);
            view.InitProperties(_maxLookDistance);
        }
    }
}
