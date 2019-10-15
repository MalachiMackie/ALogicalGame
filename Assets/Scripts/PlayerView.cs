using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerView : MonoBehaviour
    {
        private Transform _cameraTransform;

        private Collider _collider;

        private Face _lookedAtFace;

        private List<Face> _highlightedFaces;

        [SerializeField]
        private GameObject _wireTemplate;

        public void Start()
        {
            _collider = GetComponent<Collider>();
            _cameraTransform = FindObjectOfType<Camera>().transform;
        }

        public void FixedUpdate()
        {
            Debug.DrawRay(UnityEngine.Camera.main.transform.position, UnityEngine.Camera.main.transform.forward * 5, Color.red, 0.1f);
            Physics.Raycast(UnityEngine.Camera.main.transform.position, UnityEngine.Camera.main.transform.forward, out var raycastHit, 50);

            //Try look at object
            if(raycastHit.collider?.gameObject != null && raycastHit.collider?.gameObject.tag == "Face")
            {
                var lookedAtFace = raycastHit.collider.gameObject.GetComponentInParent<Face>();
                if(lookedAtFace != _lookedAtFace && lookedAtFace.CanLookAt)
                {
                    if (_highlightedFaces != null)
                    {
                        if (_highlightedFaces.Contains(lookedAtFace))
                        {
                            var faceIndex = _highlightedFaces.IndexOf(lookedAtFace);

                            for(int i = _highlightedFaces.Count - 1; i > faceIndex; i--)
                            {
                                _highlightedFaces[i].StopLookingAt();
                                _highlightedFaces.RemoveAt(i);
                            }
                        }
                        else if(!TryHighlightFace(lookedAtFace))
                        {
                            foreach (Face face in _highlightedFaces)
                            {
                                face.StopLookingAt();
                            }
                            _highlightedFaces = null;
                        }
                    }
                    else
                    {
                        _lookedAtFace?.StopLookingAt();
                    }
                    _lookedAtFace = lookedAtFace;

                    
                    _lookedAtFace.StartLookingAt();
                }
            }
            else if(_lookedAtFace != null)
            {
                _lookedAtFace.StopLookingAt();
                _lookedAtFace = null;
                if(_highlightedFaces != null)
                {
                    foreach (Face face in _highlightedFaces)
                    {
                        face.StopLookingAt();
                    }
                    _highlightedFaces = null;
                }
            }

            if(Input.GetMouseButtonDown(0))
            {
                if(_lookedAtFace is LogicFace logicFace)
                {
                    _highlightedFaces = new List<Face>()
                    {
                        _lookedAtFace
                    };
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if(_highlightedFaces?[0] is LogicFace firstLogicFace
                    && _lookedAtFace is LogicFace endLogicFace
                    && firstLogicFace != endLogicFace
                    && firstLogicFace.Mode != endLogicFace.Mode)
                {
                    //create wire with _highlightedFaces
                    CreateWire(_highlightedFaces);
                }

                if(_highlightedFaces != null)
                {
                    foreach (Face face in _highlightedFaces)
                    {
                        face.StopLookingAt();
                    }
                    _highlightedFaces = null;
                }
            }
        }

        private bool TryHighlightFace(Face face)
        {
            var lastFace = _highlightedFaces[_highlightedFaces.Count - 1];
            
            if(face is FloorFace firstFloorFace && lastFace is FloorFace lastFloorFace)
            {
                var xDif = Math.Abs(firstFloorFace.FloorPosition.x - lastFloorFace.FloorPosition.x);
                var zDif = Math.Abs(firstFloorFace.FloorPosition.z - lastFloorFace.FloorPosition.z);
                if (xDif == 1 ^ zDif == 1)
                {
                    _highlightedFaces.Add(face);
                    return true;
                }
                else if(xDif == 1 && zDif == 1)
                {
                    var gameManager = GetComponent<Player>().GameManager;
                    var newFace = gameManager.GetFloorFaceAtPosition(firstFloorFace.FloorPosition.x - Math.Sign(lastFloorFace.FloorPosition.x), firstFloorFace.FloorPosition.z);
                    if(newFace == null)
                    {
                        newFace = gameManager.GetFloorFaceAtPosition(firstFloorFace.FloorPosition.x, firstFloorFace.FloorPosition.z - Math.Sign(lastFloorFace.FloorPosition.z));
                    }

                    if(newFace == null)
                    {
                        return false;
                    }
                    newFace.StartLookingAt();
                    _highlightedFaces.Add(newFace);
                    _highlightedFaces.Add(face);
                    Debug.Log($"Found connecting face at {newFace.FloorPosition}");
                    return true;
                }
            }
            else
            {
                _highlightedFaces.Add(face);
                return true;
            }

            return false;
        }

        private void CreateWire(List<Face> path)
        {
            var wireObj = Instantiate(_wireTemplate);
            var wire = wireObj.GetComponent<Wire>();
            wire.Init(path);
        }
    }
}
