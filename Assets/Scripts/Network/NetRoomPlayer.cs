using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class NetRoomPlayer : NetworkRoomPlayer
{
    /*public TMP_Text nickname;
    public Image profile_image;
    public TMP_Text readyTxt;*/

    public string nickname;
    public bool isReady;

    public TMP_Text Nickname_txt; // 에디터 내에서 지정
    public Image Profile_image; // 에디터 내에서 지정
    public TMP_Text Readystatus_txt; // 에디터 내에서 지정

    public RectTransform Rect_Trans;

    public string playerSpaceObjectName = "PlayerSpace";


    private GameObject playerSpace;

    public void Init()
    {
        
        playerSpace = GameObject.Find(playerSpaceObjectName+ index); // ex) playerspace0


        Debug.Log(index);
        RectTransform playerSpaceRectTrans = playerSpace.GetComponentInChildren<RectTransform>();


        if (playerSpaceRectTrans == null)
        {
            Debug.LogError("PlayerSpace must get Rect Transform! - heeunAn");
            return;
        }

        Rect_Trans.localPosition = playerSpaceRectTrans.localPosition;

        playerSpace.SetActive(false);


        UpdateUI();
    }

    public void UpdateUI()
    {
        Nickname_txt.text = nickname;
        Readystatus_txt.text = "Not Ready";
    }


}
