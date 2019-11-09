using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerView : MonoBehaviour
    {
        private HighlightMode _highlightMode;

        private FloorFace _lookedAtFace;

        private FloorFace LookedAtFace
        {
            get => _lookedAtFace;
            set
            {
                if (value == null && _lookedAtFace != null)
                {
                    _lookedAtFace.StopLookingAt();
                }
                _lookedAtFace = value;
                _lookedAtFace?.StartLookingAt();
            }
        }

        private ObservableCollection<FloorFace> _highlightedFaces;

        private ObservableCollection<FloorFace> HighlightedFaces
        {
            get => _highlightedFaces;
            set
            {
                if (_highlightedFaces != value)
                {
                    if (value != null)
                    {
                        if (_highlightedFaces != null)
                        {
                            _highlightedFaces.CollectionChanged -= OnHighlightedCollectionChanged;
                        }
                        value.CollectionChanged += OnHighlightedCollectionChanged;
                    }
                    else
                    {
                        foreach (FloorFace face in _highlightedFaces)
                        {
                            if(face != LookedAtFace)
                            {
                                face.StopLookingAt();
                            }
                        }
                    }
                    _highlightedFaces = value;
                }
            }
        }

        private void OnHighlightedCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
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

        private bool TryHighlightFace(FloorFace face)
        {
            var lastFace = HighlightedFaces[HighlightedFaces.Count - 1];
            if (!face.IsAdjascentTo(lastFace))
            {
                var xDif = Math.Abs(face.GridPosition.x - lastFace.GridPosition.x);
                var zDif = Math.Abs(face.GridPosition.z - lastFace.GridPosition.z);
                if (xDif != 1 || zDif != 1)
                {
                    return false;
                }

                var gameManager = GetComponent<Player>().GameManager;
                var newFace = gameManager.GetFloorFaceAtPosition(face.GridPosition.x - Math.Sign(lastFace.GridPosition.x), face.GridPosition.z);
                if (newFace == null)
                {
                    newFace = gameManager.GetFloorFaceAtPosition(face.GridPosition.x, face.GridPosition.z - Math.Sign(lastFace.GridPosition.z));
                }
                if (newFace == null)
                {
                    return false;
                }

                HighlightedFaces.Add(newFace);
            }

            HighlightedFaces.Add(face);
            return true;
        }

        public void Update()
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 5, Color.red, 0.1f);
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit raycastHit, 50);

            //Try look at object
            if (raycastHit.collider?.gameObject != null && raycastHit.collider?.gameObject.tag == "Face")
            {
                FloorFace lookedAtFace = raycastHit.collider.gameObject.GetComponentInParent<FloorFace>();
                if (lookedAtFace != LookedAtFace && lookedAtFace.CanLookAt)
                {
                    if (_highlightMode == HighlightMode.None)
                    {
                        LookedAtFace?.StopLookingAt();
                    }
                    else if (HighlightedFaces != null)
                    {
                        if(!TryHighlightFace(lookedAtFace))
                        {
                            HighlightedFaces = null;
                        }
                    }

                    LookedAtFace = lookedAtFace;
                }
            }
            else if (LookedAtFace != null)
            {
                LookedAtFace = null;
                HighlightedFaces = null;
            }

            if (Input.GetMouseButtonDown(0))
            {
                _highlightMode = HighlightMode.Create;
                if (LookedAtFace != null)
                {
                    HighlightedFaces = new ObservableCollection<FloorFace>()
                    {
                        LookedAtFace
                    };
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                _highlightMode = HighlightMode.Delete;
                if (LookedAtFace != null)
                {
                    HighlightedFaces = new ObservableCollection<FloorFace>()
                    {
                        LookedAtFace
                    };
                }
            }

            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                if(HighlightedFaces != null)
                {
                    if (_highlightMode == HighlightMode.Create)
                    {
                        Wire.CreateWire(HighlightedFaces);
                    }
                    else if (_highlightMode == HighlightMode.Delete)
                    {
                        var wires = HighlightedFaces.Where(x => x.HasWire).Select(x => x.ParentWire).ToList();
                        foreach(Wire wire in wires)
                        {
                            StartCoroutine(wire.RemoveWire(HighlightedFaces));
                        }
                    }
                }
                _highlightMode = HighlightMode.None;
                HighlightedFaces = null;
            }
        }
    }
}
