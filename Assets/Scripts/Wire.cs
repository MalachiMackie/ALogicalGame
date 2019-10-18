using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wire : MonoBehaviour
{
    private LogicFace _firstFace;

    private LogicFace _lastFace;

    public void Init(IEnumerable<Face> path)
    {

        if (path.ElementAt(0) is LogicFace firstFace
            && path.ElementAt(path.Count() - 1) is LogicFace lastFace
            && firstFace.Mode != lastFace.Mode
            && firstFace != lastFace)
        {
            for(int i = 1; i < path.Count() - 1; i++)
            {
                Face face = path.ElementAt(i);
                
                if(face is FloorFace floorFace)
                {
                    floorFace.AddWire();
                    if (i == 1)
                    {
                        floorFace.ConnectedFace = firstFace;
                    }
                    else if (i == path.Count() - 2)
                    {
                        floorFace.ConnectedFace = lastFace;
                    }
                }
            }
            
            foreach(Face face in path)
            {
                if(face is FloorFace floorFace)
                {
                    floorFace.CheckForConnections();
                }
            }

            //var wireDiffs = new List<(Vector3 tailDiff, Vector3 headDiff, FloorFace floorFace)>();

            //for (int i = 1; i < path.Count() - 1; i++)
            //{
            //    FloorFace floorFace = path.ElementAt(i) as FloorFace ?? throw new InvalidOperationException($"Face at position {i} is not a floor face");
            //    Face tailFace = path.ElementAt(i - 1);
            //    Face headFace = path.ElementAt(i + 1);

            //    var tailDiff = new Vector3();
            //    var headDiff = new Vector3();

            //    if (!(tailFace is FloorFace tailFloorFace))
            //    {
            //        var xDiff = tailFace.transform.position.x - floorFace.transform.position.x;
            //        var zDiff = tailFace.transform.position.z - floorFace.transform.position.z;

            //        if (Mathf.Abs(xDiff) > 0.25f)
            //        {
            //            tailDiff.x = Mathf.Sign(xDiff);
            //        }

            //        if(Mathf.Abs(zDiff) > 0.25f)
            //        {
            //            tailDiff.z = Mathf.Sign(zDiff);
            //        }
            //    }
            //    else
            //    {
            //        var xDiff = tailFloorFace.FloorPosition.x - floorFace.FloorPosition.x;
            //        var zDiff = tailFloorFace.FloorPosition.z - floorFace.FloorPosition.z;
            //        tailDiff.x = xDiff == 0 ? 0 : Mathf.Sign(xDiff);
            //        tailDiff.z = zDiff == 0 ? 0 : Mathf.Sign(zDiff);
            //    }

            //    if (!(headFace is FloorFace headFloorFace))
            //    {
            //        var xDiff = headFace.transform.position.x - floorFace.transform.position.x;
            //        var zDiff = headFace.transform.position.z - floorFace.transform.position.z;

            //        if (Mathf.Abs(xDiff) > 0.25f)
            //        {
            //            headDiff.x = Mathf.Sign(xDiff);
            //        }

            //        if(Mathf.Abs(zDiff) > 0.25f)
            //        {
            //            headDiff.z = Mathf.Sign(zDiff);
            //        }
            //    }
            //    else
            //    {
            //        var xDiff = headFloorFace.FloorPosition.x - floorFace.FloorPosition.x;
            //        var zDiff = headFloorFace.FloorPosition.z - floorFace.FloorPosition.z;
            //        headDiff.x = xDiff == 0 ? 0 : Mathf.Sign(xDiff);
            //        headDiff.z = zDiff == 0 ? 0 : Mathf.Sign(zDiff);
            //    }

            //    wireDiffs.Add((tailDiff, headDiff, floorFace));
            //}

            _firstFace = firstFace;
            _lastFace = lastFace;

            firstFace.ConnectTo(lastFace);
            transform.position = firstFace.transform.position;

            //foreach((Vector3 tailDiff, Vector3 headDiff, FloorFace floorFace) in wireDiffs)
            //{
            //    //make delay?
            //    floorFace.AddWire(tailDiff, headDiff); 
            //}
        }
    }
}
