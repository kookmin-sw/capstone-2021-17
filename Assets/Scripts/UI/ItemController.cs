using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    [SerializeField]
    private float range;  // 아이템 습득이 가능한 최대 거리

    private bool pickupActivated = false;  // 아이템 습득 가능할시 True 

    [SerializeField]
    private LayerMask layerMask;  // 특정 레이어를 가진 오브젝트에 대해서만 습득할 수 있어야 한다.

    [SerializeField]
    private Text actionText;  // 행동을 보여 줄 텍스트

    void Update()
    {
        //CheckItem();
       // TryAction();
    }

    /*private void TryAction()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            //CheckItem();
            CanPickUp();
        }
    }*/
    
   /* private void CheckItem()
    {
    }*/

    public void ItemInfoAppear()
    {
        pickupActivated = true;
        //actionText.gameObject.SetActive(true);
       // actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 " + "<color=yellow>" + "(E)" + "</color>";
    }

    public void ItemInfoDisappear()
    {
        pickupActivated = false;
    //    actionText.gameObject.SetActive(false);
    }

    /*private void CanPickUp()
    {
        if(pickupActivated)
        {
            if(hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 했습니다.");  // 인벤토리 넣기
                //Destroy(hitInfo.transform.gameObject);
                //ItemInfoDisappear();
            }
        }
    }*/
}
