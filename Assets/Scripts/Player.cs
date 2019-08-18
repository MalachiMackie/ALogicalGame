using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField]
    private float _rotateSpeed;

    [SerializeField]
    private float _moveForce;

    [SerializeField]
    private float _maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        var moveInput = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            moveInput += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveInput += new Vector3(0, 0, -1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveInput += new Vector3(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveInput += new Vector3(1, 0, 0);
        }

        int rotateInput = 0;

        if (Input.GetKey(KeyCode.Q))
        {
            rotateInput--;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotateInput++;
        }

        transform.Rotate(0, rotateInput * _rotateSpeed, 0);

        moveInput *= _moveForce;

        if (_rigidbody.velocity.magnitude < _maxSpeed)
        {
            _rigidbody.AddRelativeForce(moveInput);
        }

    }
}
