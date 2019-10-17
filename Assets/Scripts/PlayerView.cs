using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerView : MonoBehaviour
    {
        private Face _lookedAtFace;

        private Face LookedAtFace
        {
            get => _lookedAtFace;
            set
            {
                if(value == null && _lookedAtFace != null)
                {
                    _lookedAtFace.StopLookingAt();
                }
                _lookedAtFace = value;
                _lookedAtFace?.StartLookingAt();
            }
        }

        private ObservableCollection<Face> _highlightedFaces;

        private ObservableCollection<Face> HighlightedFaces
        {
            get => _highlightedFaces;
            set
            {
                if (value == null && _highlightedFaces != null)
                {
                    foreach(Face face in _highlightedFaces)
                    {
                        face.StopLookingAt();
                    }
                    _highlightedFaces.CollectionChanged -= OnHighlightedCollectionChanged;
                }
                else if(value != _highlightedFaces)
                {
                    value.CollectionChanged += OnHighlightedCollectionChanged;
                }

                _highlightedFaces = value;
            }
        }

        [SerializeField]
        private GameObject _wireTemplate;

        public void Start()
        {
            
        }

        public void FixedUpdate()
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 5, Color.red, 0.1f);
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var raycastHit, 50);

            //Try look at object
            if(raycastHit.collider?.gameObject != null && raycastHit.collider?.gameObject.tag == "Face")
            {
                var lookedAtFace = raycastHit.collider.gameObject.GetComponentInParent<Face>();
                if (lookedAtFace != LookedAtFace && lookedAtFace.CanLookAt)
                {
                    if (HighlightedFaces != null)
                    {
                        if (HighlightedFaces.Contains(lookedAtFace))
                        {
                            //Remove all faces after the face we just looked at
                            for (int i = HighlightedFaces.Count - 1; i > HighlightedFaces.IndexOf(lookedAtFace); i--)
                            {
                                HighlightedFaces.RemoveAt(i);
                            }
                        }
                        else if (!TryHighlightFace(lookedAtFace))
                        {
                            HighlightedFaces = null;
                        }
                    }
                    else
                    {
                        LookedAtFace?.StopLookingAt();
                    }

                    LookedAtFace = lookedAtFace;
                }
            }
            else if(LookedAtFace != null)
            {
                LookedAtFace = null;
                HighlightedFaces = null;
            }

            if(Input.GetMouseButtonDown(0))
            {
                if(LookedAtFace is LogicFace)
                {
                    HighlightedFaces = new ObservableCollection<Face>();
                    HighlightedFaces.Add(LookedAtFace);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if(HighlightedFaces?[0] is LogicFace firstLogicFace
                    && LookedAtFace is LogicFace endLogicFace
                    && firstLogicFace != endLogicFace
                    && firstLogicFace.Mode != endLogicFace.Mode)
                {
                    //create wire with _highlightedFaces
                    CreateWire(HighlightedFaces);
                }

                HighlightedFaces = null;
            }
        }

        private bool TryHighlightFace(Face face)
        {
            var lastFace = HighlightedFaces[HighlightedFaces.Count - 1];
            
            if(face is FloorFace firstFloorFace && lastFace is FloorFace lastFloorFace)
            {
                var xDif = Math.Abs(firstFloorFace.FloorPosition.x - lastFloorFace.FloorPosition.x);
                var zDif = Math.Abs(firstFloorFace.FloorPosition.z - lastFloorFace.FloorPosition.z);
                if (xDif == 1 ^ zDif == 1)
                {
                    HighlightedFaces.Add(face);
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
                    HighlightedFaces.Add(newFace);
                    HighlightedFaces.Add(face);
                    Debug.Log($"Found connecting face at {newFace.FloorPosition}");
                    return true;
                }
            }
            else
            {
                HighlightedFaces.Add(face);
                return true;
            }

            return false;
        }

        private void OnHighlightedCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            Debug.Log($"{args.NewItems} {DateTime.Now}");
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        (args.NewItems[0] as Face).StartLookingAt();
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        (args.OldItems[0] as Face).StopLookingAt();
                        break;
                    }
            }
        }

        private void CreateWire(IEnumerable<Face> path)
        {
            var wireObj = Instantiate(_wireTemplate);
            var wire = wireObj.GetComponent<Wire>();
            wire.Init(path);
        }
    }
}
