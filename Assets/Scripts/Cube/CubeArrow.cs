using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CubeArrow
{
    Left,
    Right,
    Up,
    Down,
    Center,
    LeftUp,
    RightUp,
    LeftDown,
    RightDown
}

public static class CubeRotation
{
    private static Dictionary<CubeArrow, Vector3> _arrowToRotation = new Dictionary<CubeArrow, Vector3>()
        {
            {CubeArrow.Left, new Vector3(0, 0, 90)},
            {CubeArrow.Right, new Vector3(0, 0, 270) },
            {CubeArrow.Up, new Vector3(0, 0, 180) },
            {CubeArrow.Down, Vector3.zero },
            {CubeArrow.Center, Vector3.zero },
            {CubeArrow.LeftUp, new Vector3(0, 0, 135) },
            {CubeArrow.RightUp, new Vector3(0, 0, 225) },
            {CubeArrow.LeftDown, new Vector3(0, 0, 45) },
            {CubeArrow.RightDown, new Vector3(0, 0, 315) }
        };

    public static Vector3 GetCubeRotation(CubeArrow cubeArrow)
    {
        return _arrowToRotation[cubeArrow];
    }
}
