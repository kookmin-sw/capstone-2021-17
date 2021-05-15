using UnityEngine;
using System.Collections;
using keypadSystem;
using Mirror;

public class RayCastCam : MonoBehaviour
{
    private GameObject raycasted_obj;

    [SerializeField]
    private GameObject player_obj;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private PlayerInventory inventory;

    private int rayLength = 2;
    public LayerMask layerMaskInteract;

    public GameObject cHNormal;
    public GameObject cHHighlight;

    private bool canPull = true;
    private KeypadItemController rayCastedKeypad;
    private MissionController missionController;
    private MovePlayerTestForEnemy movePlayerTestForEnemy;

    [SerializeField] private bool isKeypad = false;
    [SerializeField] private bool isMission = false;
    [SerializeField] private bool hitByEnemy = false;

    void Awake()
    {
        if (player_obj == null)
        {
            player_obj = transform.parent.gameObject;
        }
        if (cam == null)
        {
            cam = Camera.main;
        }
        movePlayerTestForEnemy = gameObject.GetComponentInChildren<MovePlayerTestForEnemy>();
    }

    void Update()
    {
        RaycastHit hit;
        hitByEnemy = movePlayerTestForEnemy.isHit;
        Vector3 camPos = cam.transform.position;
        Vector3 camDir = cam.transform.forward;
        //player_obj = transform.parent.gameObject;

        Debug.DrawRay(camPos, camDir * 4, Color.red);
        if (Physics.Raycast(camPos, camDir, out hit, rayLength*2, layerMaskInteract.value))
        {
            if (hitByEnemy)
            {
                if (isKeypad)
                {
                    isKeypad = false;
                    rayCastedKeypad.CloseKeypad();
                }
                else if (isMission)
                {
                    isMission = false;
                    missionController.CloseMission();
                }
            }
            else if (hit.collider.CompareTag("Lever"))
            {
                raycasted_obj = hit.collider.gameObject;
                CrosshairActive();

                if (Input.GetKeyDown("e") && canPull)
                {
                    LeverController leverController = raycasted_obj.GetComponent<LeverScript>().leverController;
                    leverController.doorNet.CmdPullDownLever(raycasted_obj);
                    StartCoroutine(Timer(1.0f));
                }
            }
            else if (hit.collider.CompareTag("ItemBox"))
            {
                raycasted_obj = hit.collider.gameObject;
                CrosshairActive();

                if (Input.GetKeyDown("e"))
                {
                    isKeypad = true;
                    rayCastedKeypad = raycasted_obj.GetComponent<KeypadItemController>();
                    rayCastedKeypad.ShowKeypad();
                }
                else if (Input.GetKeyDown("escape"))
                {
                    isKeypad = false;
                    rayCastedKeypad.CloseKeypad();
                }
            }

            else if (hit.collider.CompareTag("ItemHeal"))
            {
                raycasted_obj = hit.collider.gameObject;
                CrosshairActive();

                if (Input.GetKeyDown("e"))
                {
                    MapItemBehaviour itemBehaviour = raycasted_obj.GetComponent<MapItemBehaviour>();
                    DroppedItemNetBehaviour droppedItemBehaviour = raycasted_obj.GetComponent<DroppedItemNetBehaviour>();
                    HealPack healPack = new HealPack();
                    if (!inventory.IsFull())
                    {
                        inventory.AddItem(healPack);
                        if (itemBehaviour != null) itemBehaviour.SetActive(false);
                        else if (droppedItemBehaviour != null) droppedItemBehaviour.SetActive(false);
                    }
                    else
                    {
                        Debug.Log("Inventory is Full");
                    }
                }
            }

            else if (hit.collider.CompareTag("Gun"))
            {
                raycasted_obj = hit.collider.gameObject;
                CrosshairActive();

                if (Input.GetKeyDown("e"))
                {
                    MapItemBehaviour itemBehaviour = raycasted_obj.GetComponent<MapItemBehaviour>();
                    DroppedItemNetBehaviour droppedItemBehaviour = raycasted_obj.GetComponent<DroppedItemNetBehaviour>();
                    Gun gun = new Gun();
                    if (!inventory.IsFull())
                    {
                        inventory.AddItem(gun);
                        if (itemBehaviour != null) itemBehaviour.SetActive(false);
                        else if (droppedItemBehaviour != null) droppedItemBehaviour.SetActive(false);
                    }
                    else
                    {
                        Debug.Log("Inventory is Full");
                    }
                }
            }
       
            else if (hit.collider.CompareTag("Mission"))
            {
                raycasted_obj = hit.collider.gameObject;
                CrosshairActive();

                if (Input.GetKeyDown("e"))
                {
                    isMission = true;
                    missionController = raycasted_obj.GetComponentInChildren<MissionController>();
                    missionController.ShowMission();
                }
                else if (Input.GetKeyDown("escape"))
                {
                    isMission = false;
                    missionController.CloseMission();
                }
            }           
        }
        
        else
        {
            CrosshairNormal();
            if (isKeypad)
            {
                if (Input.GetKeyDown("escape") || hitByEnemy)
                {
                    isKeypad = false;
                    rayCastedKeypad.CloseKeypad();
                }
            }
            else if (isMission && Input.GetKeyDown("escape"))
            {
                if (Input.GetKeyDown("escape") || hitByEnemy)
                {
                    isMission = false;
                    missionController.CloseMission();
                }
            }
        }
    }

    void CrosshairActive()
    {
        cHNormal.SetActive(false);
        cHHighlight.SetActive(true);
    }

    void CrosshairNormal()
    {
        cHNormal.SetActive(true);
        cHHighlight.SetActive(false);
    }

    IEnumerator Timer(float waitTime)
    {
        canPull = false;
        yield return new WaitForSeconds(waitTime);
        canPull = true;
    }
}
