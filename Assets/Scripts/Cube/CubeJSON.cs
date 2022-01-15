using System;
using UnityEngine;

[Serializable]
public class CubeJSON : IComparable<CubeJSON>
{
    public Pair<int, int> positionIdentifier;
    public CubeArrow arrowIdentifier;
    public ObjectType typeIdentifier;
    public Vector4 color;
    public float colorIntensity;
    public float time;

    public int CompareTo(CubeJSON other)
    {
        if (other == null)
        {
            return 1;
        } else
        {
            return this.time.CompareTo(other.time);
        }
    }
}
