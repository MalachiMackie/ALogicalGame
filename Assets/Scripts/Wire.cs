using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    public void Init(List<Face> path)
    {
        if(path[0] is LogicFace firstFace
            && path[path.Count - 1] is LogicFace lastFace
            && firstFace.Mode != lastFace.Mode
            && firstFace != lastFace)
        {
            firstFace.ConnectTo(lastFace);
        }
    }
}
