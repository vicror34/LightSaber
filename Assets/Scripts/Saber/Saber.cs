using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saber : MonoBehaviour
{
    public ObjectType type;
    private Transform _saberTip;
    private Transform _saberBase;

    private Vector3 _previousPosition;
    private Vector3 _previousPositionBackup;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Sliceable"))
        {
            GameObject otherGameObject = other.gameObject;
            SliceableObject sliceableObject = otherGameObject.GetComponent<SliceableObject>();
            if (sliceableObject.GetSliceableType() == SliceableType.Cube)
            {
                CubeJSON cubeJson = otherGameObject.GetComponent<Cube>().GetCubeJson();
                if (cubeJson.typeIdentifier == type)
                {
                    Vector3 currentPosition = _saberTip.transform.position;

                    if (_previousPosition == currentPosition)
                    {
                        _previousPosition = _previousPositionBackup;
                    }

                    //Vector3 A = otherGameObject.transform.position - _saberTip.transform.position;
                    //Vector3 B = otherGameObject.transform.position - _saberBase.transform.position;
                    //Vector3 planeNormal = Vector3.Normalize(Vector3.Cross(B, A));
                    //Vector3 planeProjection = Vector3.ProjectOnPlane(otherGameObject.transform.up, planeNormal);
                    //Debug.Log(planeProjection);

                    if (cubeJson.arrowIdentifier != CubeArrow.Center)
                    {
                        Vector3 sliceDirection = Vector3.Normalize(currentPosition - _previousPosition);
                        Vector3 planeProjection = Vector3.ProjectOnPlane(sliceDirection, otherGameObject.transform.forward);
                        if (Vector3.Angle(planeProjection, otherGameObject.transform.up) <= 45.0f)
                        {
                            //Debug.Log("1:" + Vector3.Angle(planeProjection, otherGameObject.transform.up));

                            sliceDirection = Vector3.Normalize(otherGameObject.transform.position - currentPosition);
                            planeProjection = Vector3.ProjectOnPlane(sliceDirection, otherGameObject.transform.forward);
                            //Debug.Log("2: " + Vector3.Angle(planeProjection, otherGameObject.transform.up));
                            if (Vector3.Angle(planeProjection, otherGameObject.transform.up) <= 45.0f)
                            {
                                Debug.Log("CUBE DESTROYED");
                                Destroy(otherGameObject);
                            }
                        }
                    } else
                    {
                        Debug.Log("CUBE DESTROYED");
                        Destroy(otherGameObject);
                    }
                    //Vector3 rayOrigin = sliceDirection * -1.0f + _previousPosition; // - (currentPosition - otherGameObject.transform.position)


                    ////Debug.Log(LayerMask.NameToLayer("Sliceable"));
                    //Debug.DrawRay(rayOrigin, sliceDirection / 2, Color.green, 30);

                    ////if (Physics.Raycast(rayOrigin, sliceDirection, out RaycastHit hit, 1.0f, 1 << LayerMask.NameToLayer("Sliceable")))
                    ////    {
                    ////        Debug.Log("E FOARTE BINE");
                    ////    }

                    //Debug.DrawLine(rayOrigin, otherGameObject.transform.position, new Color(0, 0, 0), 60, false);

                }
                //Debug.Log(currentPosition + " " + _previousPosition + " " + _saberTip.transform.position);
                //Debug.DrawLine(_previousPosition, currentPosition, new Color(0, 0, 0), 60, false);
            }
        }
    }

    private void Start()
    {
        _saberTip = transform.Find("SaberTip");
        _saberBase = transform.Find("SaberBase");
        _previousPosition = _saberTip.transform.position;
        _previousPositionBackup = _saberTip.transform.position;
    }

    private void Update()
    {
        _previousPositionBackup = _previousPosition == _saberTip.transform.position ? _previousPositionBackup : _previousPosition;
        _previousPosition = _saberTip.transform.position;
    }
}
