using UnityEngine;

public class Cube : MonoBehaviour
{
    private Vector3 _direction;
    private Vector3 _destination;
    private float _speed;
    private float _position;

    public Vector3 Position { set; get; }

    public Vector3 Direction { set; get; }

    public Vector3 Destination { set; get; }

    public float Speed { set; get; }

    private CubeJSON _cubeJSON;

    public void SetCubeJson(CubeJSON cubeJSON)
    {
        _cubeJSON = cubeJSON;
    }

    public CubeJSON GetCubeJson()
    {
        return _cubeJSON;
    }
}
