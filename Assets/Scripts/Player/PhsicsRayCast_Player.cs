using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhsicsRayCast_Player : MonoBehaviour
{
    [SerializeField] private Transform Player_tr;
    [SerializeField] public float distance=10;
    [SerializeField] private RaycastHit hit;
    [SerializeField] private LayerMask layerMask=-1;
    // Start is called before the first frame update
    void Start()
    {
        Player_tr=GetComponent<Transform>();
    }

    void RaycastHit()
    {
        Ray ray=new Ray();
        ray.origin=Player_tr.position;
        ray.direction=Player_tr.forward;
        if(Physics.Raycast(ray,out hit, distance, layerMask))
        {
            print("hit");
        }
    }
    public void RayLine()
    {
        if(hit.collider!=null){
            Debug.DrawLine(Player_tr.position,Player_tr.position+Player_tr.forward*hit.distance,Color.red);
        }else{
            Debug.DrawLine(Player_tr.position,Player_tr.position+Player_tr.forward*hit.distance,Color.white);
        }
    }
}
