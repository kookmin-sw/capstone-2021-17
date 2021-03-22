using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private float MinDistance=0.5f;
    private float MaxDistance=2.5f;
    private float Smooth=10.0f;
    private Vector3 DollyDir;
    private float Distance;
    RaycastHit Hit;

    //처음 카메라의 위치와 거리를 입력
    private void Awake()
    {
        DollyDir= transform.localPosition.normalized;
        Distance=transform.localPosition.magnitude;
    }

    // 충돌이 되면 거리를 조정
    void Update () {
        Vector3 DesiredCameraPos = transform.parent.TransformPoint(DollyDir * MaxDistance);
        if (Physics.Linecast(transform.parent.position,DesiredCameraPos, out Hit))
        {
            Distance= Mathf.Clamp(Hit.distance,MinDistance,MaxDistance);
        } 
        else
        {
            Distance=MaxDistance;
        }
        transform.localPosition=Vector3.Lerp(transform.localPosition,DollyDir*Distance,Time.deltaTime*Smooth);
    }
}


