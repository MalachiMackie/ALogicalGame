using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        private Player _player;

        private Rigidbody _rigidbody;

        public void Update()
        {

        }

        public void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _player = GetComponent<Player>();
        }

        public void FixedUpdate()
        {
            transform.Rotate(0, GetRotationInput(), 0);

            if (_rigidbody.velocity.magnitude < _player.MaxSpeed)
            {
                _rigidbody.AddRelativeForce(GetMovementInput());
            }
        }

        private float GetRotationInput()
        {
            float rotateInput = Input.GetAxis(Constants.HoriztonalLookAxis);
            rotateInput *= 0.05f;

            return rotateInput * _player.RotateSpeed;
        }

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

            moveInput *= _player.MoveForce;

            return moveInput;
        }
    }
}
