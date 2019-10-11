using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerView : MonoBehaviour
    {
        private Transform _cameraTransform;

        private Collider _collider;

        private LogicFace _selectedFace;

        private ICanBeLookedAt _lookedAt;

        public void Start()
        {
            _collider = GetComponent<Collider>();
            _cameraTransform = FindObjectOfType<Camera>().transform;
        }

        public void Update()
        {
            Debug.DrawRay(UnityEngine.Camera.main.transform.position, UnityEngine.Camera.main.transform.forward * 5, Color.red, 0.1f);
            Physics.Raycast(UnityEngine.Camera.main.transform.position, UnityEngine.Camera.main.transform.forward, out var raycastHit, 50);

            //Try look at object
            if(raycastHit.collider?.gameObject != null && raycastHit.collider?.gameObject.tag == "CanLookAt")
            {
                var lookedAt = raycastHit.collider.gameObject.GetComponentInParent<ICanBeLookedAt>();
                if(lookedAt != _lookedAt)
                {
                    _lookedAt?.StopLookingAt();
                    _lookedAt = lookedAt;
                    _lookedAt.StartLookingAt();
                }
            }
            else if(_lookedAt != null)
            {
                _lookedAt.StopLookingAt();
                _lookedAt = null;
            }

            //If looked at object is a logic face
            if(_lookedAt is LogicFace lookedAtFace)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    _selectedFace = lookedAtFace;
                }

                if(Input.GetMouseButtonUp(0)
                    && _selectedFace != lookedAtFace
                    && lookedAtFace.Mode != _selectedFace.Mode)
                {
                    ConnectFaces(_selectedFace, lookedAtFace);
                }
            }

            if(Input.GetMouseButtonUp(0) && _selectedFace != null)
            {
                _selectedFace = null;
            }
        }

        private void ConnectFaces(LogicFace firstFace, LogicFace secondFace)
        {
            if (firstFace.Mode == secondFace.Mode)
            {
                Debug.LogError("Cannot connect two faces of the same mode");
                throw new InvalidOperationException("Cannot connect two faces of the same mode");
            }
            firstFace.ConnectTo(secondFace);
        }
    }
}
