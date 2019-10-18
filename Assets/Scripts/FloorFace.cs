using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class FloorFace : Face
    {
        public Vector3Int FloorPosition;

        private ICanBePlaced Occupant;

        public bool HasOccupant => Occupant != null;

        public List<FloorFace> Neighbours;

        public bool HasWire { get; private set; }

        public LogicFace ConnectedFace;

        private Material _wireMaterial;

        private Material WireMaterial
        {
            get => _wireMaterial;
            set
            {
                if(value != _wireMaterial)
                {
                    var mats = Renderer.materials;
                    mats[0] = value;
                    Renderer.materials = mats;
                }
            }
        } 

        private Material _crossWireMat;

        private Material _eastWestWireMat;
        private Material _northSouthWireMat;

        private Material _northEastWireMat;
        private Material _northWestWireMat;
        private Material _southEastWireMat;
        private Material _southWestWireMat;

        private Material _westWireMat;
        private Material _northWireMat;
        private Material _eastWireMat;
        private Material _southWireMat;

        private Material _northWestSouthWireMat;
        private Material _northEastSouthWireMat;
        private Material _eastSouthWestWireMat;
        private Material _eastNorthWestWireMat;

        protected override void Start()
        {
            base.Start();
            _wireMaterial = _invisibleMat;

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
        }

        public void SetOccupant(ICanBePlaced proposedOccupant)
        {
            if (Occupant == null)
            {
                Occupant = proposedOccupant;
                Occupant.GridPos = FloorPosition;
                Occupant.transform.SetParent(transform);
                Occupant.transform.localPosition = new Vector3(0, Occupant.transform.localPosition.y, 0);
            }
        }

        public void AddWire()
        {
            HasWire = true;
        }

        public void RemoveWire()
        {
            HasWire = false;
        }

        public void CheckForConnections()
        {
            WireMaterial = GetWireMaterial();
        }

        private WireConnection GetConnection(Vector3 diff)
        {
            if (diff.x == 1 && diff.z == 0)
            {
                return WireConnection.East;
            }
            else if (diff.x == -1 && diff.z == 0)
            {
                return WireConnection.West;
            }
            else if (diff.x == 0 && diff.z == 1)
            {
                return WireConnection.North;
            }
            else if (diff.x == 0 && diff.z == -1)
            {
                return WireConnection.South;
            }
            throw new InvalidOperationException($"Cannot add Wire connection with position difference of {diff}");
        }

        private Material GetWireMaterial()
        {
            if(!HasWire)
            {
                return _invisibleMat;
            }

            var connections = new List<WireConnection>(4);
            foreach(FloorFace neighbour in Neighbours)
            {
                if (neighbour.HasWire || (neighbour.HasOccupant && ConnectedFace != null && (ConnectedFace.HaveInput == neighbour.Occupant || ConnectedFace.HaveOutput == neighbour.Occupant)))
                {
                    var diff = new Vector3();
                    var xDiff = neighbour.FloorPosition.x - FloorPosition.x;
                    var zDiff = neighbour.FloorPosition.z - FloorPosition.z;
                    diff.x = xDiff == 0 ? 0 : Mathf.Sign(xDiff);
                    diff.z = zDiff == 0 ? 0 : Mathf.Sign(zDiff);

                    connections.Add(GetConnection(diff));
                }
            }

            switch (connections.Count)
            {
                default:
                    {
                        return _invisibleMat;
                    }
                case 1:
                    {
                        switch (connections[0])
                        {
                            case WireConnection.East:
                                return _eastWireMat;
                            case WireConnection.South:
                                return _southWireMat;
                            case WireConnection.West:
                                return _westWireMat;
                            default:
                                return _northWireMat;
                        }
                    }
                case 2:
                    {
                        if (connections.Contains(WireConnection.East) && connections.Contains(WireConnection.West))
                        {
                            return _eastWestWireMat;
                        }
                        else if(connections.Contains(WireConnection.North) && connections.Contains(WireConnection.South))
                        {
                            return _northSouthWireMat;
                        }
                        else if(connections.Contains(WireConnection.North) && connections.Contains(WireConnection.East))
                        {
                            return _northEastWireMat;
                        }
                        else if(connections.Contains(WireConnection.North) && connections.Contains(WireConnection.West))
                        {
                            return _northWestWireMat;
                        }
                        else if(connections.Contains(WireConnection.South) && connections.Contains(WireConnection.East))
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
                        if(connections.Contains(WireConnection.North)
                            && connections.Contains(WireConnection.East)
                            && connections.Contains(WireConnection.South))
                        {
                            return _northEastSouthWireMat;
                        }
                        else if(connections.Contains(WireConnection.West)
                            && connections.Contains(WireConnection.East)
                            && connections.Contains(WireConnection.South))
                        {
                            return _eastSouthWestWireMat;
                        }
                        else if(connections.Contains(WireConnection.North)
                            && connections.Contains(WireConnection.West)
                            && connections.Contains(WireConnection.South))
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
    }
}
