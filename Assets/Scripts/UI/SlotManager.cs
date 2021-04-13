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

    public static SlotManager instance; 


	void Awake () 
    {
        for(int i=0; i<4; i++){
            isEmpty[i] = true;
        }
        instance = this;
    }
    
    //힐 아이콘 추가
    public void AddHeal()
    {
        thisImage = sprites[0];
        AddItem();
    }

    //아이템 창에 추가
    public void AddItem()
    {
        for(int i=0; i<4; i++)
        {
            if(isEmpty[i]==true)
            {
                slot[i].sprite = thisImage;
                slot[i].gameObject.SetActive(true);
                isEmpty[i] = false;
                break;
            }
            else
            {
                continue;
            }
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
                slot[i].gameObject.SetActive(false);
                isEmpty[i] = true;
                break;
            }
            else
            {
                continue;
            }
        }
    }    
}