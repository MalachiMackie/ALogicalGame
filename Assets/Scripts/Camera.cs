using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField]
    private Transform _player;

    [SerializeField]
    private Vector3 _positionOffset;

    [SerializeField]
    private Vector3 _rotationOffset;

    [SerializeField]
    private float _positionDamping;

    [SerializeField]
    private float _rotationDamping;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var wantedPosition = _player.TransformPoint(0, _positionOffset.y, _positionOffset.z);

        var wantedRotation = Quaternion.Euler(_player.rotation.eulerAngles + _rotationOffset);

        transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * _positionDamping);
        transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * _rotationDamping);
    }
}
