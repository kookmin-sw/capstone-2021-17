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
    private int[] onTarget = new int[4]; // 아이템 타겟팅 여부

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
            onTarget[i] = 0; // 타겟팅 초기화
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
            Targeting(0);
            moveCursor(itemTarget);
            inventory.ActiveHandItem(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // 2
        {
            Debug.Log("2 click");
            itemTarget = 1;
            Targeting(1);
            moveCursor(itemTarget);
            inventory.ActiveHandItem(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // 3
        {
            Debug.Log("3 click");
            itemTarget = 2;
            Targeting(2);
            moveCursor(itemTarget);
            inventory.ActiveHandItem(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) // 4
        {
            Debug.Log("4 click");
            itemTarget = 3;
            Targeting(3);
            moveCursor(itemTarget);
            inventory.ActiveHandItem(3);
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
//아이템 타겟팅
    private void Targeting(int target)
    {
        for(int i=0; i<4; i++)
        {
            if(i==target)
            {
                onTarget[target]+=1;
            }
            else
            {
                onTarget[i] = 0;
            }
            
        }
    }

    private void ItemAction()
    { //같은 번호 2번 누를 시 사용, Q키로 아이템 버리기
        //커서의 위치와 누른 번호가 일치할 경우 아이템이 사용됨

        if(Input.GetKeyDown(KeyCode.Alpha1) && onTarget[0] > 1) // 타게팅을 두번하면 됨
        {
            inventory.UseItem(0);
            onTarget[0] = 0;
            
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && onTarget[1] > 1)
        {
            
            inventory.UseItem(1);
            onTarget[1] = 0;

        }
        else if(Input.GetKeyDown(KeyCode.Alpha3) && onTarget[2] > 1)
        {

            inventory.UseItem(2);
            onTarget[2] = 0;

        }
        else if(Input.GetKeyDown(KeyCode.Alpha4) && onTarget[3] > 1)
        {
            inventory.UseItem(3);
            onTarget[3] = 0;

        }
        else if(Input.GetKeyDown(KeyCode.Q))
        {
            inventory.DropItem(itemTarget);
            onTarget[itemTarget] = 0;
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
        else if(itemName == "Gun")
        {
            thisImage = sprites[1];
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