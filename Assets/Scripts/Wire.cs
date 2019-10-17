using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wire : MonoBehaviour
{
    public void Init(IEnumerable<Face> path)
    {
        var pathList = path.ToList();

        if(pathList[0] is LogicFace firstFace
            && pathList[pathList.Count() - 1] is LogicFace lastFace
            && firstFace.Mode != lastFace.Mode
            && firstFace != lastFace)
        {
            firstFace.ConnectTo(lastFace);
        }
    }
}
