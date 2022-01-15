using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CubeSpawner
{
    private CubeSpawnerStats _stats;
    private StringVector3Dictionary _spawningMap;
    private CubeJSON[] _objectSpawnList;
    private List<Cube> _cubeList;

    private GameObject _cubePrefab;
    //private CubeRotation _cubeRotation;

    public CubeSpawner(ref CubeSpawnerStats stats, string jsonString)
    {
        LevelJSON levelJson = JsonUtility.FromJson<LevelJSON>(jsonString);
        _stats = stats;

        _cubeList = new List<Cube>();
        _objectSpawnList = new CubeJSON[levelJson.cubeList.Length];
        _objectSpawnList = levelJson.cubeList;
        //_cubeRotation = new CubeRotation();

        //TO CHANGE
        //_cubeStats = cubePrefab.GetComponent<CubeStats>();

        int width = int.Parse(levelJson.size.Split('x')[0]);
        int height = int.Parse(levelJson.size.Split('x')[1]);

        GenerateSpawningMap(width, height);
    }

    public void SetPrefabs(GameObject prefab)
    {
        _cubePrefab = prefab;
    }

    private void GenerateSpawningMap(int width, int height)
    {
        Vector3 startingPoint = _stats.spawningPoint + new Vector3((width - 1) * _stats.spacing, (height - 1) * _stats.spacing, 0);

        _spawningMap = new StringVector3Dictionary();

        //Debug.Log(width + " " + height + " " + startingPoint + " " + _stats.spawningPoint);
        //Debug.Log(" " + _stats.spacing);
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                //_spawningMap.Add(GenerateName(startingPoint), startingPoint);
                _spawningMap.Add(new Pair<int, int>(i, j).ToString(), startingPoint);
                startingPoint -= 2 * new Vector3(_stats.spacing, 0, 0);
            }
            startingPoint += 2 * new Vector3(_stats.spacing, 0, 0) * width;
            startingPoint -= 2 * new Vector3(0, _stats.spacing, 0);
        }
    }

    private string GenerateName(Vector3 point)
    {
        Vector3 euclideanDistance = Utils.EuclideanDistance(point, _stats.spawningPoint);
        string name = "";
        name += euclideanDistance.x != 0 && euclideanDistance.x > 0 ? new string('L', (int)(Mathf.Abs(euclideanDistance.x) + _stats.spacing)) : new string('R', (int)(Mathf.Abs(euclideanDistance.x) + _stats.spacing));
        name += euclideanDistance.y != 0 && euclideanDistance.y > 0 ? new string('U', (int)(Mathf.Abs(euclideanDistance.y) + _stats.spacing)) : new string('D', (int)(Mathf.Abs(euclideanDistance.y) + _stats.spacing));

        return name;
    }

    private GameObject InstantiateObject(GameObject obj, Pair<int, int> positionIdentifier, Quaternion rotation)
    {
        GameObject iObj = Object.Instantiate(obj, _spawningMap[positionIdentifier.ToString()], rotation);
        iObj.name = positionIdentifier.ToString();
        return iObj;
    }

    private void SetMaterials(ref GameObject gameObject, Cube cubeStats)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {

            GameObject child = gameObject.transform.GetChild(i).gameObject;
            if (cubeStats.GetCubeJson().arrowIdentifier == CubeArrow.Center)
            {
                if (child.name == "Center")
                {
                    child.SetActive(true);
                }
            }
            else
            {
                if (child.name == "Arrow")
                {
                    child.SetActive(true);
                }
            }
            Material material = child.GetComponent<Renderer>().material;
            if (material != null)
            {
                Color color = cubeStats.GetCubeJson().color;
                if (material.IsKeywordEnabled("_EMISSION"))
                {
                    color /= Mathf.Max(color.r, color.g, color.b);
                    material.SetColor("_EmissionColor", color * cubeStats.GetCubeJson().colorIntensity);
                } else
                {
                    color /= Mathf.Max(color.r, color.g, color.b);
                    material.SetColor("_BaseColor", color);
                }
            }
        }
    }

    private Cube SpawnObject(CubeJSON cubeProperties)
    {
        Vector3 rotation = CubeRotation.GetCubeRotation(cubeProperties.arrowIdentifier);
        Quaternion cubeRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        GameObject cubeObject = InstantiateObject(_cubePrefab, cubeProperties.positionIdentifier, cubeRotation);

        Cube cube = cubeObject.GetComponent<Cube>();
        float distance = Mathf.Abs(_stats.playerPosition.z - _stats.spawningPoint.z);
        cube.Position = cubeObject.transform.position;
        cube.Destination = _stats.playerPosition;
        cube.Speed = distance / _stats.travelTime;
        cube.Direction = _stats.cubeMoveDirection;
        cube.SetCubeJson(cubeProperties);

        SetMaterials(ref cubeObject, cube);

        Utils.ScaleObject(ref cubeObject, new Vector3(_stats.cubeSize, _stats.cubeSize, _stats.cubeSize));

        return cube;
    }

    public void SetCubeStats(Cube cubeStats, float speed, Vector3 direction)
    {
        cubeStats.Speed = speed;
        cubeStats.Direction = direction;
    }

    private void CubeDebug()
    {
        for (int index = 0; index < _cubeList.Count; index++)
        {
            if (_cubeList[index].Position.z >= 0)
            {
                //Debug.Log(Time.time);
            }
        }
    }

    private int currentIndex = 0;

    private void SpawnEverywhere()
    {
        foreach (var key in _spawningMap.Keys)
        {
            string str = Regex.Replace(key.ToString(), "[(,)]", "");
            CubeJSON cube = new CubeJSON()
            {
                positionIdentifier = new Pair<int, int>(int.Parse(str.Split(' ')[0]), int.Parse(str.Split(' ')[1])),
                arrowIdentifier = CubeArrow.Center,
                typeIdentifier = ObjectType.Left,
                color = new Vector4(255, 0, 0, 255),
                colorIntensity = 3.317389f,
                time = 5.0f
            };
            SpawnObject(cube);
        }
    }

    public void Update(float time)
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SpawnEverywhere();
        }

        CubeDebug();
        for (int index = currentIndex; index < _objectSpawnList.Length; index++)
        {
            float spawnTime = time + _stats.travelTime;
            if (_objectSpawnList[index].time / 2.06666666667f <= spawnTime)
            {
                //Debug.Log(_cubeSpawnList[index].arrowIdentifier);
                _cubeList.Add(SpawnObject(_objectSpawnList[index]));
                currentIndex++;
            }
        }
    }
}
