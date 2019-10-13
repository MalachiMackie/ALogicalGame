using UnityEngine;

namespace Assets.Scripts
{
    public class FloorFace : Face
    {
        public Vector3Int FloorPosition;

        private ICanBePlaced Occupant;

        public void SetOccupant(ICanBePlaced proposedOccupant)
        {
            if(Occupant == null)
            {
                Occupant = proposedOccupant;
                Occupant.GridPos = FloorPosition;
                Occupant.transform.SetParent(transform);
                Occupant.transform.localPosition = new Vector3(0, Occupant.transform.localPosition.y, 0);
            }
        }
    }
}
