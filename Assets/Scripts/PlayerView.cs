using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerView : MonoBehaviour
    {
        private Transform _cameraTransform;

        private Collider _collider;

        private Face _selectedFace;

        private Face _lookedAtFace;

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
            if(raycastHit.collider?.gameObject != null && raycastHit.collider?.gameObject.tag == "Face")
            {
                var lookedAtFace = raycastHit.collider.gameObject.GetComponentInParent<Face>();
                if(lookedAtFace != _lookedAtFace && lookedAtFace.CanLookAt)
                {
                    _lookedAtFace?.StopLookingAt();
                    _lookedAtFace = lookedAtFace;
                    _lookedAtFace.StartLookingAt();
                }
            }
            else if(_lookedAtFace != null)
            {
                _lookedAtFace.StopLookingAt();
                _lookedAtFace = null;
            }

            if(Input.GetMouseButtonDown(0))
            {
                _selectedFace = _lookedAtFace;
            }

            if(Input.GetMouseButtonUp(0)
                && _selectedFace is LogicFace selectedLogicFace
                && _lookedAtFace is LogicFace lookedAtLogicFace
                && selectedLogicFace != lookedAtLogicFace
                && selectedLogicFace.Mode != lookedAtLogicFace.Mode)
            {
                ConnectFaces(selectedLogicFace, lookedAtLogicFace);
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
