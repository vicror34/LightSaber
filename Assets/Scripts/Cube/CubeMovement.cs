using UnityEngine;

[RequireComponent(typeof(Cube))]
public class CubeMovement : MonoBehaviour
{
    private Cube _cube;

    private void Awake()
    {
        _cube = GetComponent<Cube>();
    }

    private void Update()
    {
        if (!LevelManager.IsPaused())
        {
            transform.position += _cube.Speed * _cube.Direction * Time.deltaTime;
            _cube.Position = transform.position;
            if (Vector3.Angle((_cube.Destination + _cube.Direction * 5.0f) - transform.position, _cube.Direction) >= 90.0f)
            {
                ScoreManager.TakeDamage(10.0f);
                AudioManager.Instance.PlayOnce(AudioManager.AudioSources.HURT);
                Destroy(gameObject);
            }
        }
    }
}
