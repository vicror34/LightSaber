using System;
using UnityEngine;

[Serializable]
public class LevelJSON
{
    public int level;
    public string name;
    public string size;
    public CubeJSON[] cubeList;
}
