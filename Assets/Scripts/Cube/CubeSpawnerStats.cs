using UnityEngine;

public class CubeSpawnerStats : MonoBehaviour
{
    public Vector3 spawningPoint = new Vector3(0.0f, 1.5f, -19.5f);
    public Vector3 playerPosition = new Vector3(0.0f, 0.0f, 0.0f);
    public float spacing = 0.5f;
    public float cubeSize = 0.75f;
    public float travelTime;
    public Vector3 cubeMoveDirection;
}
