using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    public class FloorGrid : MonoBehaviour
    {
        /// <summary>
        /// The size of the grid
        /// </summary>
        [SerializeField]
        private Vector3Int _gridSize;

        /// <summary>
        /// The parent to match size
        /// </summary>
        [SerializeField]
        private GameObject _floorParent;

        /// <summary>
        /// The dictionary of floor pieces with their position
        /// </summary>
        private Dictionary<Vector3Int, FloorFace> _floorDictionary = new Dictionary<Vector3Int, FloorFace>();

        /// <summary>
        /// Run on start
        /// </summary>
        public void Start()
        {
            //Initialize the GridSize
            GameObject floorFaceTemplate = Resources.Load<GameObject>("Prefabs/FloorFace");

            if (_floorParent != null)
            {
                int gridX = (int)_floorParent.transform.localScale.x;
                int gridZ = (int)_floorParent.transform.localScale.z;
                if (gridX % 2 == 0 || gridZ % 2 == 0)
                {
                    throw new InvalidOperationException("Cannot set parent floor as the scale's are even, they must be odd");
                }

                _gridSize.x = gridX;
                _gridSize.z = gridZ;
                Debug.Log($"Floor Parent set, Changing Floor grid X to {_gridSize.z} and Floor Grid Z to {_gridSize.x}");
            }

            if (_gridSize.x <= 0)
            {
                throw new InvalidOperationException("Floor Grid X must be positive");
            }

            if (_gridSize.z <= 0)
            {
                throw new InvalidOperationException("Floor Grid Z must be positive");
            }

            int xSubtractor = _gridSize.x / 2;
            int zSubtractor = _gridSize.z / 2;

            //Grab Existing floorpieces
            FloorFace[] floorFaces = FindObjectsOfType<FloorFace>();

            foreach (FloorFace floorFace in floorFaces)
            {
                _floorDictionary.Add(floorFace.GridPosition, floorFace);
            }

            //Fill out the rest of the floor
            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int z = 0; z < _gridSize.z; z++)
                {
                    if (!_floorDictionary.TryGetValue(new Vector3Int(x, 0, z), out FloorFace existingFace))
                    {
                        GameObject floorFaceObj = Instantiate(floorFaceTemplate);
                        FloorFace floorFace = floorFaceObj.GetComponent<FloorFace>();
                        floorFace.GridPosition = new Vector3Int(x, 0, z);
                        floorFaceObj.transform.SetParent(transform);
                        floorFaceObj.transform.localPosition = new Vector3(x - xSubtractor, 0, z - zSubtractor);
                        _floorDictionary.Add(floorFace.GridPosition, floorFace);
                    }
                }
            }

            //Initialize each floor piece
            foreach (KeyValuePair<Vector3Int, FloorFace> floorFaceEntry in _floorDictionary)
            {
                FloorFace floorFace = floorFaceEntry.Value;
                int x = floorFaceEntry.Key.x;
                int z = floorFaceEntry.Key.z;

                if (_floorDictionary.TryGetValue(new Vector3Int(x - 1, 0, z), out FloorFace neighbour1))
                {
                    floorFace.Neighbours.Add(neighbour1);
                }
                if (_floorDictionary.TryGetValue(new Vector3Int(x + 1, 0, z), out FloorFace neighbour2))
                {
                    floorFace.Neighbours.Add(neighbour2);
                }
                if (_floorDictionary.TryGetValue(new Vector3Int(x, 0, z - 1), out FloorFace neighbour3))
                {
                    floorFace.Neighbours.Add(neighbour3);
                }
                if (_floorDictionary.TryGetValue(new Vector3Int(x, 0, z + 1), out FloorFace neighbour4))
                {
                    floorFace.Neighbours.Add(neighbour4);
                }
            }

            //Initialize the grid components
            IEnumerable<ICanBePlaced> gridComponents = FindObjectsOfType<MonoBehaviour>().OfType<ICanBePlaced>();

            foreach (ICanBePlaced gridComponent in gridComponents)
            {
                _floorDictionary[new Vector3Int(gridComponent.GridPosition.x, 0, gridComponent.GridPosition.z)].SetOccupant(gridComponent);
            }
        }

        /// <summary>
        /// Try place a component on the grid
        /// </summary>
        /// <param name="component"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool TryPlaceGridComponent(ICanBePlaced component, Vector3Int position)
        {
            if (_floorDictionary.TryGetValue(position, out FloorFace value) && value.Occupant == null)
            {
                foreach (FloorFace face in component.MyFaces)
                {
                    if (_floorDictionary.TryGetValue(face.GridPosition, out FloorFace existingFace))
                    {
                        Destroy(existingFace);
                        _floorDictionary[face.GridPosition] = face;
                    }
                    else
                    {
                        return false;
                    }
                }
                _floorDictionary[position].SetOccupant(component);
                //foreach(FloorFace face in component.MyFaces)
                //{
                //    face.transform.position = new Vector3(face.GridPosition.x - (GridX / 2), 0, face.GridPosition.z - (GridZ / 2));
                //}
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get the FloorFace at position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public FloorFace GetFloorFaceAtPosition(int x, int z)
        {
            _floorDictionary.TryGetValue(new Vector3Int(x, 0, z), out FloorFace face);
            return face;
        }
    }
}
