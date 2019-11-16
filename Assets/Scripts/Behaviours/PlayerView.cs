using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Objects;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    public class PlayerView : MonoBehaviour, INeedGameManger
    {
        /// <summary>
        /// Mode for highlighting
        /// </summary>
        private HighlightMode _highlightMode;

        /// <summary>
        /// The currently looked at face
        /// </summary>
        private FloorFace _lookedAtFace;

        /// <summary>
        /// The collection of highlightedFaces
        /// </summary>
        private ObservableCollection<FloorFace> _highlightedFaces;

        /// <summary>
        /// The Max Look Distance
        /// </summary>
        private float _maxLookDistance;

        /// <summary>
        /// Gets or sets the Game Manager
        /// </summary>
        public GameManager GameManager { get; set; }

        /// <summary>
        /// start or stop looking at newly added and removed faces
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnHighlightedCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        (args.NewItems[0] as FloorFace).StartLookingAt();
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        (args.OldItems[0] as FloorFace).StopLookingAt();
                        break;
                    }
            }
        }

        /// <summary>
        /// Unhighlight all faces in <see cref="_highlightedFaces"/>
        /// </summary>
        private void UnHighlightFaces()
        {
            if (_highlightedFaces == null)
            {
                return;
            }

            foreach (FloorFace face in _highlightedFaces)
            {
                if (face != _lookedAtFace)
                {
                    face.StopLookingAt();
                }
            }
        }

        /// <summary>
        /// Try to highlight a face
        /// </summary>
        /// <param name="face"></param>
        /// <returns></returns>
        private bool TryHighlightFace(FloorFace face)
        {
            FloorFace lastFace = _highlightedFaces[_highlightedFaces.Count - 1];
            if (!face.IsAdjascentTo(lastFace))
            {
                int xDif = Math.Abs(face.GridPosition.x - lastFace.GridPosition.x);
                int zDif = Math.Abs(face.GridPosition.z - lastFace.GridPosition.z);
                if (xDif != 1 || zDif != 1)
                {
                    return false;
                }

                FloorFace newFace = GameManager.GetFloorFaceAtPosition(face.GridPosition.x - Math.Sign(lastFace.GridPosition.x), face.GridPosition.z);
                if (newFace == null)
                {
                    newFace = GameManager.GetFloorFaceAtPosition(face.GridPosition.x, face.GridPosition.z - Math.Sign(lastFace.GridPosition.z));
                }
                if (newFace == null)
                {
                    return false;
                }

                _highlightedFaces.Add(newFace);
            }

            _highlightedFaces.Add(face);
            return true;
        }

        /// <summary>
        /// Initializes the properties
        /// </summary>
        /// <param name="maxLookDistance"></param>
        public void InitProperties(float maxLookDistance)
        {
            _maxLookDistance = maxLookDistance;
        }

        /// <summary>
        /// Update every frame
        /// </summary>
        public void Update()
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * _maxLookDistance, Color.red, 0.1f);
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit raycastHit, 50);

            //Try look at face
            if (raycastHit.collider?.gameObject != null && raycastHit.collider?.gameObject.tag == "Face" && raycastHit.distance <= _maxLookDistance)
            {
                FloorFace lookedAtFace = raycastHit.collider.gameObject.GetComponentInParent<FloorFace>();
                if (lookedAtFace != _lookedAtFace)
                {
                    if (_highlightMode == HighlightMode.None || _highlightMode == HighlightMode.Place)
                    {
                        _lookedAtFace?.StopLookingAt();
                    }
                    else if (_highlightedFaces != null)
                    {
                        if (!TryHighlightFace(lookedAtFace))
                        {
                            foreach (FloorFace face in _highlightedFaces)
                            {
                                if (face != _lookedAtFace)
                                {
                                    face.StopLookingAt();
                                }
                            }
                            _highlightedFaces = null;
                        }
                    }

                    _lookedAtFace = lookedAtFace;
                    _lookedAtFace.StartLookingAt();
                }
            }
            else if (_lookedAtFace != null)
            {
                _lookedAtFace.StopLookingAt();
                _lookedAtFace = null;

                UnHighlightFaces();
                _highlightedFaces = null;
            }

            if (Input.GetMouseButtonDown(0) && GameManager.CanMove)
            {
                if (_highlightMode == HighlightMode.Place)
                {
                    //Place an object
                    GameObject clockTemplate = Resources.Load<GameObject>("Prefabs/Clock");
                    GameObject newClockObj = Instantiate(clockTemplate);
                    Clock clock = newClockObj.GetComponent<Clock>();
                    clock.SetGridPosition(_lookedAtFace.GridPosition);
                    if (!GameManager.TryPlaceFloorComponent(clock, _lookedAtFace.GridPosition))
                    {
                        Destroy(newClockObj);
                    }

                }
                else
                {
                    //Start highlighting faces for wire creation
                    _highlightMode = HighlightMode.Create;
                    if (_lookedAtFace != null)
                    {
                        _highlightedFaces = new ObservableCollection<FloorFace>()
                    {
                        _lookedAtFace
                    };
                        _highlightedFaces.CollectionChanged += OnHighlightedCollectionChanged;
                    }
                }
            }

            //Start Highlighting faces in deletion mode
            if (Input.GetMouseButtonDown(1) && GameManager.CanMove)
            {
                _highlightMode = HighlightMode.Delete;
                if (_lookedAtFace != null)
                {
                    _highlightedFaces = new ObservableCollection<FloorFace>()
                    {
                        _lookedAtFace
                    };
                    _highlightedFaces.CollectionChanged += OnHighlightedCollectionChanged;
                }
            }

            //Activate the highlighted faces
            if ((Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) && GameManager.CanMove)
            {
                if (_highlightedFaces != null)
                {
                    if (_highlightMode == HighlightMode.Create)
                    {
                        Wire.CreateWire(_highlightedFaces);
                    }
                    else if (_highlightMode == HighlightMode.Delete)
                    {
                        var wires = _highlightedFaces.Where(x => x.HasWire).Select(x => x.ParentWire).ToList();
                        foreach (Wire wire in wires)
                        {
                            StartCoroutine(wire.RemoveWire(_highlightedFaces));
                        }
                    }
                }
                _highlightMode = HighlightMode.None;

                UnHighlightFaces();
                _highlightedFaces = null;
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                _highlightMode = HighlightMode.Place;
            }
        }
    }
}
