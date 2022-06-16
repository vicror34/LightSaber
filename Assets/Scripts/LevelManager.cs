using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LevelManager : MonoBehaviour
{
    public GameObject cubePrefab;

    private CubeSpawnerStats _stats;
    private CubeSpawner _cubeSpawner;
    private LevelJSON _levelJSON;
    //private AudioManager _audioManager;

    private static float _timer = 0.0f;
    private static int _BPM;
    private static bool _paused = true;

    public static int BPM { get; private set; }

    public static bool IsPaused()
    {
        return _paused;
    }

    public static float GetCurrentTime()
    {
        return _timer;
    }

    void Start()
    {
        //CubeJSON cubeJSON = new CubeJSON()
        //{
        //    positionIdentifier = new Pair<int, int>(0, 2),
        //    arrowIdentifier = CubeArrow.Center,
        //    typeIdentifier = SwordType.LeftSword,
        //    color = new Vector4(255, 0, 0, 255),
        //    colorIntensity = 3.317389f,
        //    time = 5.0f
        //};
        //CubeJSON cubeJSON1 = new CubeJSON()
        //{
        //    positionIdentifier = new Pair<int, int>(0, 0),
        //    arrowIdentifier = CubeArrow.Left,
        //    typeIdentifier = SwordType.RightSword,
        //    color = new Vector4(0, 0, 255, 255),
        //    colorIntensity = 11.31371f,
        //    time = 1.0f
        //};
        //LevelJSON levelJSON = new LevelJSON()
        //{
        //    level = 0,
        //    name = "Level 0",
        //    size = "4x3",
        //};
        //List<CubeJSON> cubeJSONList = new List<CubeJSON>();
        //cubeJSONList.Add(cubeJSON);
        //cubeJSONList.Add(cubeJSON1);
        //cubeJSONList.Sort();
        //levelJSON.cubeList = cubeJSONList.ToArray();

        //string stringJSON = JsonUtility.ToJson(levelJSON);
        ConversionJSON levelJSON = new ConversionJSON("/JSONs/SevenNationArmy.json");
        //levelJSON.SaveJSON("/JSONs/SevenNationArmyLS.json");
        string stringJSON = levelJSON.GetJSON();
        //Debug.Log(stringJSON);
        AudioManager.Instance.Load(this.gameObject, "Audio/SevenNationArmy", AudioManager.AudioSources.SONG);
        AudioManager.Instance.Load(this.gameObject, "Audio/Cut", AudioManager.AudioSources.HIT);
        AudioManager.Instance.Load(this.gameObject, "Audio/Hurt", AudioManager.AudioSources.HURT);
        //_audioManager.Load(this.gameObject, "Audio/SevenNationArmy");
        //_audioManager = new AudioManager(this.gameObject, "Audio/SevenNationArmy");
        _stats = GetComponent<CubeSpawnerStats>();
        _stats.cubeMoveDirection = Vector3.Normalize(_stats.cubeMoveDirection);
        _levelJSON = JsonUtility.FromJson<LevelJSON>(stringJSON);
        _BPM = _levelJSON.BPM;
        _cubeSpawner = new CubeSpawner(ref _stats, _levelJSON);
        _cubeSpawner.SetPrefabs(cubePrefab);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            _paused = !_paused;
            AudioManager.Instance.TogglePlay();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            AudioManager.Instance.PlayOnce(AudioManager.AudioSources.HIT);
        }

        if (!_paused)
        {
            _cubeSpawner.Update(_timer);
            _timer += Time.deltaTime;
        }
    }
}
