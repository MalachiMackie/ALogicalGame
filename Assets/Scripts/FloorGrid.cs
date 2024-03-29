﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class FloorGrid : MonoBehaviour
    {
        public static int GridX { get; private set; }

        public static int GridZ { get; private set; }

        [SerializeField]
        private int _gridX;

        [SerializeField]
        private int _gridZ;

        [SerializeField]
        private GameObject _floorFaceTemplate;

        [SerializeField]
        private GameObject _floorParent;

        private Dictionary<Vector3Int, FloorFace> _floorDictionary = new Dictionary<Vector3Int, FloorFace>();

        public void Start()
        {
            if (_floorParent != null)
            {
                var gridX = (int)_floorParent.transform.localScale.x;
                var gridZ = (int)_floorParent.transform.localScale.z;
                if (gridX % 2 == 0 || gridZ % 2 == 0) throw new InvalidOperationException("Cannot set parent floor as the scale's are even, they must be odd");
                _gridX = gridX;
                _gridZ = gridZ;
                Debug.Log($"Floor Parent set, Changing Floor grid X to {_gridZ} and Floor Grid Z to {_gridX}");
            }

            if (_gridX <= 0)
            {
                throw new InvalidOperationException("Floor Grid X must be positive");
            }

            if (_gridZ <= 0)
            {
                throw new InvalidOperationException("Floor Grid Z must be positive");
            }

            GridX = _gridX;
            GridZ = _gridZ;

            var xSubtractor = GridX / 2;
            var zSubtractor = GridZ / 2;

            var logicFaces = FindObjectsOfType<FloorFace>();

            foreach(FloorFace floorFace in logicFaces)
            {
                _floorDictionary.Add(floorFace.GridPosition, floorFace);
            }

            for (int x = 0; x < GridX; x++)
            {
                for (int z = 0; z < GridZ; z++)
                {
                    if (!_floorDictionary.TryGetValue(new Vector3Int(x, 0, z), out var existingFace))
                    {
                        var floorFaceObj = Instantiate(_floorFaceTemplate);
                        var floorFace = floorFaceObj.GetComponent<FloorFace>();
                        floorFace.GridPosition = new Vector3Int(x, 0, z);
                        floorFaceObj.transform.SetParent(transform);
                        floorFaceObj.transform.localPosition = new Vector3(x - xSubtractor, 0, z - zSubtractor);
                        _floorDictionary.Add(floorFace.GridPosition, floorFace);
                    }
                }
            }

            foreach (KeyValuePair<Vector3Int, FloorFace> floorFaceEntry in _floorDictionary)
            {
                var floorFace = floorFaceEntry.Value;
                var x = floorFaceEntry.Key.x;
                var z = floorFaceEntry.Key.z;

                if (_floorDictionary.TryGetValue(new Vector3Int(x - 1, 0, z), out var neighbour1))
                {
                    floorFace.Neighbours.Add(neighbour1);
                }
                if (_floorDictionary.TryGetValue(new Vector3Int(x + 1, 0, z), out var neighbour2))
                {
                    floorFace.Neighbours.Add(neighbour2);
                }
                if (_floorDictionary.TryGetValue(new Vector3Int(x, 0, z - 1), out var neighbour3))
                {
                    floorFace.Neighbours.Add(neighbour3);
                }
                if (_floorDictionary.TryGetValue(new Vector3Int(x, 0, z + 1), out var neighbour4))
                {
                    floorFace.Neighbours.Add(neighbour4);
                }
            }

            var gridComponents = FindObjectsOfType<MonoBehaviour>().OfType<ICanBePlaced>();

            foreach (var gridComponent in gridComponents)
            {
                _floorDictionary[new Vector3Int(gridComponent.GridPosition.x, 0, gridComponent.GridPosition.z)].SetOccupant(gridComponent);
            }
        }

        public void Update()
        {

        }

        public FloorFace GetFloorFaceAtPosition(int x, int z)
        {
            _floorDictionary.TryGetValue(new Vector3Int(x, 0, z), out var face);
            return face;
        }

        //public static Vector3 GetFloorTransformPos(Vector3 position) => new Vector3(position.x - (GridX / 2), position.y + 0.5f, position.z - (GridZ / 2));
    }
}
