using Assets.Scripts;
using Assets.Scripts.Settings;
using UnityEngine;

public class CameraMovement : MonoBehaviour, INeedGameManger
{
    [SerializeField]
    private Transform _player;

    [SerializeField]
    private float _positionDamping;

    [SerializeField]
    private float _rotationDamping;

    public GameManager GameManager { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.CameraMode == CameraMode.ThirdPerson)
        {
            transform.position = Vector3.Lerp(transform.position, _player.TransformPoint(Constants.ThirdPersonCameraPositionOffset), Time.deltaTime * _positionDamping);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(_player.rotation.eulerAngles + Constants.RotationOffset), Time.deltaTime * _rotationDamping); ;
        }
        else
        {
            transform.position = _player.transform.position + Constants.FirstPersonCameraPositionOffset;

            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, _player.eulerAngles.y, _player.eulerAngles.z);
            var xRotation = Input.GetAxis(Constants.VerticalLookAxis) * 0.1f;
            xRotation = Settings.FirstPersonInvertCamera ? xRotation * -1 : xRotation;

            if(GameManager.CanMove)
            {
                transform.Rotate(xRotation, 0, 0);
            }
        };
    }
}
