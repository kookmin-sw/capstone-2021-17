using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DebugSample : MonoBehaviour
{
    [SerializeField]
    private Text text;

    [SerializeField]
    private EnemyChase enemyChase;
    // Update is called once per frame
    void Update()
    {

        text.text = "State : " + enemyChase.state + "\n"
            + "SetTarget : " + enemyChase.setTarget + "\n"
            + "FindTargetVision : " + enemyChase.findTargetVision + "\n"
            + "IsPatrol : " + enemyChase.isPatrol + "\n"
            + "Distance : " + enemyChase.dis;
    }
}
