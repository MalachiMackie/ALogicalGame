using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    public class FloorFace : MonoBehaviour, IHaveGridPosition
    {
        #region WireConnectionMaterials

        /// <summary>
        /// The material for no connections
        /// </summary>
        private Material _noConnectionsWireMat;

        /// <summary>
        /// The material for 4 connections
        /// </summary>
        private Material _crossWireMat;

        /// <summary>
        /// The eastWest material
        /// </summary>
        private Material _eastWestWireMat;

        /// <summary>
        /// The northSouth material
        /// </summary>
        private Material _northSouthWireMat;

        /// <summary>
        /// The northEast material
        /// </summary>
        private Material _northEastWireMat;

        /// <summary>
        /// The NorthWest material
        /// </summary>
        private Material _northWestWireMat;

        /// <summary>
        /// The SouthEast material
        /// </summary>
        private Material _southEastWireMat;

        /// <summary>
        /// The southWest material
        /// </summary>
        private Material _southWestWireMat;

        /// <summary>
        /// The West material
        /// </summary>
        private Material _westWireMat;

        /// <summary>
        /// The North material
        /// </summary>
        private Material _northWireMat;

        /// <summary>
        /// The east material
        /// </summary>
        private Material _eastWireMat;

        /// <summary>
        /// The south material
        /// </summary>
        private Material _southWireMat;

        /// <summary>
        /// The NorthWestSouth material
        /// </summary>
        private Material _northWestSouthWireMat;

        /// <summary>
        /// The NorthEastSouth material
        /// </summary>
        private Material _northEastSouthWireMat;

        /// <summary>
        /// The EastSouthWest material
        /// </summary>
        private Material _eastSouthWestWireMat;

        /// <summary>
        /// The EastNorthWest material
        /// </summary>
        private Material _eastNorthWestWireMat;

        /// <summary>
        /// The material for when we're looked at
        /// </summary>
        private Material _lookingAtMat;

        /// <summary>
        /// Invisible material
        /// </summary>
        private Material _invisibleMat;

        #endregion

        /// <summary>
        /// The current wire material
        /// </summary>
        private Material _wireMaterial;

        /// <summary>
        /// The Renderer
        /// </summary>
        private Renderer _renderer;

        /// <summary>
        /// Whether we're being looked at
        /// </summary>
        private bool _isLookedAt;

        /// <summary>
        /// Gets the Neighbours
        /// </summary>
        public readonly List<IHaveGridPosition> Neighbours = new List<IHaveGridPosition>();

        /// <summary>
        /// Gets or sets the position on the grid
        /// </summary>
        public Vector3Int GridPosition { get; set; }

        /// <summary>
        /// Gets or sets the Occupant
        /// </summary>
        public ICanBePlaced Occupant { get; private set; }

        /// <summary>
        /// Gets whether we have a wire
        /// </summary>
        public bool HasWire => ParentWire != null;

        /// <summary>
        /// Gets or sets the Parent Wire
        /// </summary>
        public Wire ParentWire { get; private set; }

        /// <summary>
        /// Get the wire material based on the neighbour connections
        /// </summary>
        /// <returns></returns>
        private Material GetWireMaterial()
        {
            if (!HasWire)
            {
                return _invisibleMat;
            }

            var connections = new List<GridDirection>(4);
            foreach (IHaveGridPosition neighbour in Neighbours)
            {
                if (neighbour is FloorFace neighbourFloor && !neighbourFloor.HasWire)
                {
                    continue;
                }

                var diff = new Vector3();
                int xDiff = neighbour.GridPosition.x - GridPosition.x;
                int zDiff = neighbour.GridPosition.z - GridPosition.z;
                diff.x = xDiff == 0 ? 0 : Mathf.Sign(xDiff);
                diff.z = zDiff == 0 ? 0 : Mathf.Sign(zDiff);

                GridDirection connection;

                if (diff.x == 1 && diff.z == 0)
                {
                    connection = GridDirection.East;
                }
                else if (diff.x == -1 && diff.z == 0)
                {
                    connection = GridDirection.West;
                }
                else if (diff.x == 0 && diff.z == 1)
                {
                    connection = GridDirection.North;
                }
                else if (diff.x == 0 && diff.z == -1)
                {
                    connection = GridDirection.South;
                }
                else
                {
                    throw new InvalidOperationException($"Cannot add Wire connection with position difference of {diff}");
                }

                connections.Add(connection);
            }

            switch (connections.Count)
            {
                default:
                    {
                        return _noConnectionsWireMat;
                    }
                case 1:
                    {
                        switch (connections[0])
                        {
                            case GridDirection.East:
                                return _eastWireMat;
                            case GridDirection.South:
                                return _southWireMat;
                            case GridDirection.West:
                                return _westWireMat;
                            default:
                                return _northWireMat;
                        }
                    }
                case 2:
                    {
                        if (connections.Contains(GridDirection.East) && connections.Contains(GridDirection.West))
                        {
                            return _eastWestWireMat;
                        }
                        else if (connections.Contains(GridDirection.North) && connections.Contains(GridDirection.South))
                        {
                            return _northSouthWireMat;
                        }
                        else if (connections.Contains(GridDirection.North) && connections.Contains(GridDirection.East))
                        {
                            return _northEastWireMat;
                        }
                        else if (connections.Contains(GridDirection.North) && connections.Contains(GridDirection.West))
                        {
                            return _northWestWireMat;
                        }
                        else if (connections.Contains(GridDirection.South) && connections.Contains(GridDirection.East))
                        {
                            return _southEastWireMat;
                        }
                        else
                        {
                            return _southWestWireMat;
                        }
                    }
                case 3:
                    {
                        if (connections.Contains(GridDirection.North)
                            && connections.Contains(GridDirection.East)
                            && connections.Contains(GridDirection.South))
                        {
                            return _northEastSouthWireMat;
                        }
                        else if (connections.Contains(GridDirection.West)
                            && connections.Contains(GridDirection.East)
                            && connections.Contains(GridDirection.South))
                        {
                            return _eastSouthWestWireMat;
                        }
                        else if (connections.Contains(GridDirection.North)
                            && connections.Contains(GridDirection.West)
                            && connections.Contains(GridDirection.South))
                        {
                            return _northWestSouthWireMat;
                        }
                        else
                        {
                            return _eastNorthWestWireMat;
                        }
                    }
                case 4:
                    {
                        return _crossWireMat;
                    }
            }
        }

        /// <summary>
        /// Initialize the floorface
        /// </summary>
        protected virtual void Initialize()
        {
            _renderer = GetComponent<Renderer>();
            gameObject.tag = "Face";

            _noConnectionsWireMat = Resources.Load<Material>("Materials/Wires/NoConnectionsWireMat");

            _crossWireMat = Resources.Load<Material>("Materials/Wires/CrossWireMat");

            _eastWestWireMat = Resources.Load<Material>("Materials/Wires/EastWestWireMat");
            _northSouthWireMat = Resources.Load<Material>("Materials/Wires/NorthSouthWireMat");

            _northEastWireMat = Resources.Load<Material>("Materials/Wires/NorthEastWireMat");
            _northWestWireMat = Resources.Load<Material>("Materials/Wires/NorthWestWireMat");
            _southEastWireMat = Resources.Load<Material>("Materials/Wires/SouthEastWireMat");
            _southWestWireMat = Resources.Load<Material>("Materials/Wires/SouthWestWireMat");

            _westWireMat = Resources.Load<Material>("Materials/Wires/WestWireMat");
            _northWireMat = Resources.Load<Material>("Materials/Wires/NorthWireMat");
            _eastWireMat = Resources.Load<Material>("Materials/Wires/EastWireMat");
            _southWireMat = Resources.Load<Material>("Materials/Wires/SouthWireMat");

            _northWestSouthWireMat = Resources.Load<Material>("Materials/Wires/NorthWestSouthWireMat");
            _northEastSouthWireMat = Resources.Load<Material>("Materials/Wires/NorthEastSouthWireMat");
            _eastSouthWestWireMat = Resources.Load<Material>("Materials/Wires/EastSouthWestWireMat");
            _eastNorthWestWireMat = Resources.Load<Material>("Materials/Wires/EastNorthWestWireMat");

            _lookingAtMat = Resources.Load<Material>("Materials/LookingAtMaterial");
            _invisibleMat = Resources.Load<Material>("Materials/InvisibleMaterial");

            _wireMaterial = _invisibleMat;

            StopLookingAt();
        }

        /// <summary>
        /// Runs before Start()
        /// </summary>
        public void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// Set the occupant
        /// </summary>
        /// <param name="proposedOccupant"></param>
        public void SetOccupant(ICanBePlaced proposedOccupant)
        {
            if (Occupant == null)
            {
                Occupant = proposedOccupant;
                Occupant.GridPosition = GridPosition;
                Occupant.transform.SetParent(transform);
                Occupant.transform.localPosition = new Vector3(0, Occupant.transform.localPosition.y, 0);
            }
        }

        /// <summary>
        /// Sets the Parent Wire
        /// </summary>
        /// <param name="parentWire"></param>
        public void SetParentWire(Wire parentWire)
        {
            if (HasWire)
            {
                ParentWire.RemoveFace(this);
            }
            ParentWire = parentWire;
            CheckForConnections();
            UpdateNeighbours();
        }

        /// <summary>
        /// Remove the parent wire
        /// </summary>
        public void RemoveWire()
        {
            ParentWire = null;
            CheckForConnections();
        }

        /// <summary>
        /// Check for neighbour wire connections and update the material
        /// </summary>
        public void CheckForConnections()
        {
            Material material = GetWireMaterial();
            if (material != _wireMaterial)
            {
                Material[] mats = _renderer.materials;
                mats[0] = material;
                _renderer.materials = mats;
                _wireMaterial = material;
            }
        }

        /// <summary>
        /// Tell our neighbours that to check for connections
        /// </summary>
        public void UpdateNeighbours()
        {
            foreach (IHaveGridPosition neighbour in Neighbours)
            {
                if (neighbour is FloorFace neighbourFloor)
                {
                    neighbourFloor.CheckForConnections();
                }
            }
        }

        /// <summary>
        /// Check whether we are adjascent to an object
        /// </summary>
        /// <param name="haveGridPosition"></param>
        /// <returns></returns>
        public bool IsAdjascentTo(IHaveGridPosition haveGridPosition)
        {
            return Neighbours.Contains(haveGridPosition);
        }

        /// <summary>
        /// Gets the neighbour wires
        /// </summary>
        /// <param name="caller"></param>
        /// <returns></returns>
        public IEnumerable<FloorFace> GetNeighbourWires(FloorFace caller = null)
        {
            var neighbourFaces = new List<FloorFace>();
            if (caller == null)
            {
                neighbourFaces.Add(this);
            }
            foreach (IHaveGridPosition neighbour in Neighbours)
            {
                if (neighbour is FloorFace neighbourFloorFace && neighbourFloorFace.HasWire && neighbourFloorFace != caller)
                {
                    neighbourFaces.Add(neighbourFloorFace);
                    IEnumerable<FloorFace> neighboursFaces = neighbourFloorFace.GetNeighbourWires(this);
                    neighbourFaces.AddRange(neighboursFaces);
                }
            }
            return neighbourFaces;
        }

        /// <summary>
        /// We're being watched
        /// </summary>
        public void StartLookingAt()
        {
            if (!_isLookedAt)
            {
                _isLookedAt = true;

                Material[] mats = _renderer.materials;
                mats[mats.Length - 1] = _lookingAtMat;
                _renderer.materials = mats;
            }
        }

        /// <summary>
        /// We're not being watched
        /// </summary>
        public void StopLookingAt()
        {
            _isLookedAt = false;

            if (_renderer != null)
            {
                Material[] mats = _renderer.materials;
                mats[mats.Length - 1] = _invisibleMat;
                _renderer.materials = mats;
            }
        }
    }
}
