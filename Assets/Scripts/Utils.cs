using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3 EuclideanDistance(Vector3 p1, Vector3 p2)
    {
        return p1 - p2;
    }

    public static void ScaleObject(ref GameObject obj, Vector3 scale)
    {
        obj.transform.localScale = new Vector3(scale.x * obj.transform.localScale.x, scale.y * obj.transform.localScale.y, scale.z * obj.transform.localScale.z);
    }
}
