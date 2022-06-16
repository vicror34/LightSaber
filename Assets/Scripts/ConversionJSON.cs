using UnityEngine;
using System.Collections.Generic;
using System;

public class ConversionJSON
{
    private string _stringJSON;

    public string GetJSON() { return _stringJSON; }

    public void SaveJSON(string path)
    {
        System.IO.File.WriteAllText(Application.dataPath + path, _stringJSON);
    }

    public ConversionJSON(string path)
    {
        string JSON = System.IO.File.ReadAllText(Application.dataPath + path);
        BeatSaberJSON beatSaberJSON = JsonUtility.FromJson<BeatSaberJSON>(JSON);
        ConvertToLightSaberJSON(beatSaberJSON);
    }

    private void ConvertToLightSaberJSON(BeatSaberJSON sageJSON)
    {
        LevelJSON levelJSON = new LevelJSON()
        {
            level = 0,
            name = "Level 0",
            size = "4x3",
            BPM = 124
        };
        List<CubeJSON> cubeJSONList = new List<CubeJSON>();
        foreach (BeatSaberObjectJSON levelObject in sageJSON.notes)
        {
            cubeJSONList.Add(ConvertToCubeJSON(levelObject));
        }
        cubeJSONList.Sort();
        levelJSON.cubeList = cubeJSONList.ToArray();
        _stringJSON = JsonUtility.ToJson(levelJSON);
    }

    private CubeJSON ConvertToCubeJSON(BeatSaberObjectJSON beatSaberObjectJSON)
    {
        CubeJSON cubeJSON = new CubeJSON();
        cubeJSON.time = beatSaberObjectJSON.time;
        cubeJSON.arrowIdentifier = JSONConversionUtils.GetCubeArrowIdentifier(beatSaberObjectJSON.cutDirection);
        cubeJSON.typeIdentifier = JSONConversionUtils.GetCubeTypeIdentifier(beatSaberObjectJSON.type);
        if (cubeJSON.typeIdentifier == ObjectType.Left)
        {
            cubeJSON.color = new Vector4(255, 0, 0, 255);
            cubeJSON.colorIntensity = 3.317389f;
        }
        if (cubeJSON.typeIdentifier == ObjectType.Right)
        {
            cubeJSON.color = new Vector4(0, 0, 255, 255);
            cubeJSON.colorIntensity = 11.31371f;
        }
        cubeJSON.positionIdentifier = JSONConversionUtils.GetCubePositionIdentifier(beatSaberObjectJSON.lineLayer, beatSaberObjectJSON.lineIndex);

        return cubeJSON;
    }
}

public static class JSONConversionUtils
{
    private static Dictionary<int, CubeArrow> _intToArrow = new Dictionary<int, CubeArrow>()
    {
        { 0, CubeArrow.Down },
        { 1, CubeArrow.Up },
        { 2, CubeArrow.Right },
        { 3, CubeArrow.Left },
        { 4, CubeArrow.RightDown },
        { 5, CubeArrow.LeftDown },
        { 6, CubeArrow.RightUp },
        { 7, CubeArrow.LeftUp },
        { 8, CubeArrow.Center }
    };

    public static CubeArrow GetCubeArrowIdentifier(int cutDirection)
    {
        return _intToArrow[cutDirection];
    }

    private static Dictionary<int, ObjectType> _intToType = new Dictionary<int, ObjectType>()
    {
        { 0, ObjectType.Left },
        { 1, ObjectType.Right }
    };

    public static ObjectType GetCubeTypeIdentifier(int type)
    {
        return _intToType[type];
    }

    public static Pair<int, int> GetCubePositionIdentifier(int lineLayer, int lineIndex)
    {
        return new Pair<int, int>(2 - lineLayer, lineIndex);
    }
}

[Serializable]
public class BeatSaberJSON
{
    public BeatSaberObjectJSON[] notes;
    public BeatSaberObjectJSON[] obstacles;
}

[Serializable]
public class BeatSaberObjectJSON
{
    public float time;
    public int lineIndex;
    public int lineLayer;
    public int type;
    public int cutDirection;
}
