using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    public class PlayerMovement : MonoBehaviour
    {
        /// <summary>
        /// The Players max speed
        /// </summary>
        private float _maxSpeed;

        /// <summary>
        /// The players rotation speed
        /// </summary>
        private float _rotateSpeed;

        /// <summary>
        /// The Players move force
        /// </summary>
        private float _moveForce;

        /// <summary>
        /// The players rigid body
        /// </summary>
        private Rigidbody _rigidBody;

        /// <summary>
        /// Get the Rotation Input
        /// </summary>
        /// <returns></returns>
        private float GetRotationInput()
        {
            float rotateInput = Input.GetAxis(Constants.HoriztonalLookAxis);
            rotateInput *= 0.05f;

            return rotateInput * _rotateSpeed;
        }

        /// <summary>
        /// Get the movement input
        /// </summary>
        /// <returns></returns>
        private Vector3 GetMovementInput()
        {
            var moveInput = new Vector3();
            if (Input.GetKey(KeyCode.Comma))
            {
                moveInput += new Vector3(0, 0, 1);
            }

            if (Input.GetKey(KeyCode.O))
            {
                moveInput += new Vector3(0, 0, -1);
            }

            if (Input.GetKey(KeyCode.A))
            {
                moveInput += new Vector3(-1, 0, 0);
            }

            if (Input.GetKey(KeyCode.E))
            {
                moveInput += new Vector3(1, 0, 0);
            }

            moveInput *= _moveForce;

            return moveInput;
        }

        /// <summary>
        /// Initialize <see cref="PlayerMovement"/>'s properties
        /// </summary>
        /// <param name="rigidBody"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="rotateSpeed"></param>
        /// <param name="moveForce"></param>
        public void InitProperties(Rigidbody rigidBody, float maxSpeed, float rotateSpeed, float moveForce)
        {
            _rigidBody = rigidBody;
            _maxSpeed = maxSpeed;
            _rotateSpeed = rotateSpeed;
            _moveForce = moveForce;
        }

        /// <summary>
        /// Runs 50 times per second
        /// </summary>
        public void FixedUpdate()
        {
            if (!GameManager.CanMove)
            {
                return;
            }

            transform.Rotate(0, GetRotationInput(), 0);

            if (_rigidBody.velocity.magnitude < _maxSpeed)
            {
                _rigidBody.AddRelativeForce(GetMovementInput());
            }
        }
    }
}
