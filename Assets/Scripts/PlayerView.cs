using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerView : MonoBehaviour
    {
        private FloorFace _lookedAtFace;

        private FloorFace LookedAtFace
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

        private ObservableCollection<FloorFace> _highlightedFaces;

        private ObservableCollection<FloorFace> HighlightedFaces
        {
            get => _highlightedFaces;
            set
            {
                if (value == null && _highlightedFaces != null)
                {
                    foreach(FloorFace face in _highlightedFaces)
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

        public void Update()
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 5, Color.red, 0.1f);
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var raycastHit, 50);

            //Try look at object
            if(raycastHit.collider?.gameObject != null && raycastHit.collider?.gameObject.tag == "Face")
            {
                var lookedAtFace = raycastHit.collider.gameObject.GetComponentInParent<FloorFace>();
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
                if(LookedAtFace != null)
                {
                    HighlightedFaces = new ObservableCollection<FloorFace>();
                    HighlightedFaces.Add(LookedAtFace);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if(HighlightedFaces != null)
                {
                    CreateWire(HighlightedFaces);
                }

                HighlightedFaces = null;
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

        private void CreateWire(IEnumerable<FloorFace> path)
        {
            var wireObj = Instantiate(_wireTemplate);
            var wire = wireObj.GetComponent<Wire>();
            wire.Init(path);
        }
    }
}
