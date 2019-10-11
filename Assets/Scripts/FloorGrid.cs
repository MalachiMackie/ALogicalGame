using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class FloorGrid : MonoBehaviour
    {
        [SerializeField]
        private int _gridX;

        [SerializeField]
        private int _gridZ;

        [SerializeField]
        private GameObject _floorFaceTemplate;

        [SerializeField]
        private GameObject _floorParent;

        public void Start()
        {
            if(_floorParent != null)
            {
                var gridX = (int)_floorParent.transform.localScale.x;
                var gridZ = (int)_floorParent.transform.localScale.z;
                if(gridX % 2 == 0 || gridZ % 2 == 0) throw new InvalidOperationException("Cannot set parent floor as the scale's are even, they must be odd");
                _gridX = gridX;
                _gridZ = gridZ;
                Debug.Log($"Floor Parent set, Changing Floor grid X to {_gridX} and Floor Grid Z to {_gridZ}");
            }

            if(_gridX <= 0)
            {
                throw new InvalidOperationException("Floor Grid X must be positive");
            }

            if(_gridZ <= 0)
            {
                throw new InvalidOperationException("Floor Grid Z must be positive");
            }

            var xSubtractor = _gridX / 2;
            var ZSubtractor = _gridZ / 2;

            for(int x = 0; x < _gridX; x++)
            {
                for(int z = 0; z < _gridZ; z++)
                {
                    var floorFace = Instantiate(_floorFaceTemplate);
                    floorFace.transform.SetParent(transform);
                    floorFace.transform.localPosition = new Vector3(x - xSubtractor, 0, z - ZSubtractor);
                }
            }
        }

        public void Update()
        {

        }

        public static Vector2 GetFloorPosition(Vector2 dirtyPosition)
        {
            return dirtyPosition;
        }
    }
}
