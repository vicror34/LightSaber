using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public int squareArrayCount = 10;
    public int pillarArrayCount = 20;
    public float rotateSpeed = 0.25f;
    public float moveSpeed = 0.25f;

    [SerializeField]
    private GameObject _squareArray;

    [SerializeField]
    private GameObject _pillar;

    private struct Pillar
    {
        public GameObject pillar;
        public float moveDirection;
        public float initialHeight;
        public bool canChange;
    }

    private List<GameObject> _squareArrays = new List<GameObject>();
    private Pillar[] _pillars;

    private void RotateArrays(float deltaTime)
    {
        for (int i = 0; i < squareArrayCount; i++)
        {
            _squareArrays[i].transform.Rotate(new Vector3(0, 0, rotateSpeed * deltaTime * (i + 1)));
        }

        if (
            (Mathf.Repeat(_squareArrays[squareArrayCount - 1].transform.rotation.eulerAngles.z + 180, 360) - 180 >= 135.0f && rotateSpeed > 0.0f) ||
            (Mathf.Repeat(_squareArrays[squareArrayCount - 1].transform.rotation.eulerAngles.z + 180, 360) - 180 <= -45.0f && rotateSpeed < 0.0f)
            )
        {
            rotateSpeed *= -1.0f;
        }

    }

    private void MovePillars(float deltaTime)
    {
        for (int i = 0; i < pillarArrayCount * 2; i++)
        {
            if (_pillars[i].pillar.transform.position.y >= 1.0f + _pillars[i].initialHeight && _pillars[i].canChange)
            {
                _pillars[i].moveDirection *= -1.0f;
                _pillars[i].canChange = false;
            }

            if (_pillars[i].pillar.transform.position.y <= -1.0f + _pillars[i].initialHeight && _pillars[i].canChange)
            {
                _pillars[i].moveDirection *= -1.0f;
                _pillars[i].canChange = false;
            }

            if (_pillars[i].moveDirection < 0.0f && _pillars[i].pillar.transform.position.y < 1.0f + _pillars[i].initialHeight)
            {
                _pillars[i].canChange = true;
            }

            if (_pillars[i].moveDirection > 0.0f && _pillars[i].pillar.transform.position.y > -1.0f + _pillars[i].initialHeight)
            {
                _pillars[i].canChange = true;
            }

            Vector3 prevPos = _pillars[i].pillar.transform.position;
            prevPos.y = prevPos.y + deltaTime * moveSpeed * _pillars[i].moveDirection * Random.Range(0.0f, 1.0f);
            _pillars[i].pillar.transform.position = prevPos;
        }
    }


    private void Awake()
    {
        for (int i = 0; i < squareArrayCount; i++)
        {
            GameObject obj = Instantiate(_squareArray, new Vector3(0, 0, -42 + (-5.0f) * i), Quaternion.Euler(new Vector3(0, 0, 45.0f + 5.0f * i)));
            _squareArrays.Add(obj);
        }

        _pillars = new Pillar[pillarArrayCount * 2];
        for (int i = 0, j = 0; i < pillarArrayCount * 2; i += 2, j++)
        {
            GameObject obj = Instantiate(_pillar, new Vector3(7.5f, j % 2 == 0 ? -20 : -19, -8 + (-1.0f) * i), Quaternion.identity);
            Pillar pillar = new Pillar();
            pillar.pillar = obj;
            pillar.initialHeight = j % 2 == 0 ? -20 : -19 + Random.Range(-1.0f, 1.0f);
            pillar.moveDirection = 1.0f;
            pillar.canChange = true;
            _pillars[i] = pillar;

            obj = Instantiate(_pillar, new Vector3(-9, j % 2 == 0 ? -20 : -19, -8 + (-1.0f) * i), Quaternion.identity);
            pillar = new Pillar();
            pillar.pillar = obj;
            pillar.initialHeight = j % 2 == 0 ? -20 : -19 + Random.Range(-1.0f, 1.0f);
            pillar.moveDirection = 1.0f;
            pillar.canChange = true;
            _pillars[i + 1] = pillar;
        }
    }

    private void Update()
    {
        RotateArrays(Time.deltaTime);
        MovePillars(Time.deltaTime);
    }
}
