using Assets.Scripts;
using UnityEngine;

public class Player : MonoBehaviour, INeedGameManger
{
    public GameManager GameManager { get; set; }

    [SerializeField]
    private float _rotateSpeed;
    public float RotateSpeed { get => _rotateSpeed; }

    [SerializeField]
    private float _moveForce;
    public float MoveForce { get => _moveForce; }

    [SerializeField]
    private float _maxSpeed;
    public float MaxSpeed { get => _maxSpeed; }

    // Start is called before the first frame update
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
}
