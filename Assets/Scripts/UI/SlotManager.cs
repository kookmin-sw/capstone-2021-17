using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SlotManager : MonoBehaviour 
{
    public Image[] slot = new Image[4];
    public Sprite[] sprites = new Sprite[2];
    private Sprite thisImage;
    private bool[] isEmpty = new bool[4];
    private Color[] color = new Color[4];

    public GameObject Cursor; //아이템 선택 이미지
    private int itemTarget; //커서가 위치해있는 아이템슬롯

    public static SlotManager instance;

    public PlayerInventory inventory;


	void Awake () 
    {
        for(int i=0; i<4; i++)
        {
            isEmpty[i] = true;
            color[i] = slot[i].color;
            color[i].a = 0; // 인벤토리 빈 상태로 보이도록
            slot[i].color = color[i];
        }
        instance = this;
    }

    void Update()
    {
        if(inventory == null)
        {
            return;
        }

        //아이템창 활성화 & 커서이동
        if (Input.GetKeyDown(KeyCode.Alpha1)) // 1
        {
            Debug.Log("1 click"); //디버그용으로 삭제해도 무관
            itemTarget = 0;
            moveCursor(itemTarget);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // 2
        {
            Debug.Log("2 click");
            itemTarget = 1;
            moveCursor(itemTarget);

        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) // 3
        {
            Debug.Log("3 click");
            itemTarget = 2;
            moveCursor(itemTarget);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) // 4
        {
            Debug.Log("4 click");
            itemTarget = 3;
            moveCursor(itemTarget);
        }
        
        if(isEmpty[itemTarget]==false)
        {
            ItemAction();
        }
    }


    //아이템 선택 커서 이동
    private void moveCursor(int i)
    {//i번재 슬롯으로 커서 이동
        Vector3 position = Cursor.transform.localPosition;
        position.y = -6f;
        if(i==0){
            position.x = -179.5f;
        }
        if(i==1){
            position.x = -60f;
        }
        if(i==2){
            position.x = 55f;
        }
        if(i==3){
            position.x = 169f;
        }
         Cursor.transform.localPosition = position;

    }

    private void ItemAction(){ //클릭으로 사용, Q키로 아이템 버리기
        if (Input.GetMouseButtonDown(0))
        {
            inventory.UseItem(itemTarget); //아이템 사용
        }
        else if(Input.GetKeyDown(KeyCode.Q))
        {
            inventory.DropItem(itemTarget);
        }
    }

    public void AddItem(Item item)
    {
        for (int i=0;i<4 ; i++)
        {
            if (isEmpty[i] == true)
            {
                SetThisImage(item);
                slot[i].sprite = thisImage;
                color[i].a = 1f; // 알파값 1로
                slot[i].color = color[i];
                isEmpty[i] = false;
                break;
            }
            else
            {
                continue;
            }
        }
    }
    public void AddItem(int i, Item item)
    {
        if (isEmpty[i] == true)
        {
            SetThisImage(item);
            slot[i].sprite = thisImage;
            color[i].a = 1f;
            slot[i].color = color[i];
            isEmpty[i] = false;
        }
    }

    void SetThisImage(Item item)
    {
        string itemName = item.GetType().Name;
        Debug.Log("Pick UP : " +itemName);
        if(itemName == "HealPack")
        {
            thisImage = sprites[0];
        }
    }

    //아이템 삭제
    public void RemoveItem()
    {
        for(int i=3; i>=0; i--)
        {
            if(isEmpty[i]==false)
            {
                slot[i].sprite = null;
                color[i].a = 0;
                slot[i].color = color[i];
                isEmpty[i] = true;
                break;
            }
            else
            {
                continue;
            }
        }
    }
    public void RemoveItem(int i)
    {
        if (isEmpty[i] == false)
        {
            slot[i].sprite = null;
            color[i].a = 0;
            slot[i].color = color[i];
            isEmpty[i] = true;
        }
    }

}