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
    [SerializeField]
    private AnimationEvent animEvent;
    // Update is called once per frame
    void Update()
    {

        text.text = "State : " + enemyChase.state + "\n\n"
            + "FindTargetVision : " + enemyChase.findTargetVision + "\n\n"
            + "IsPatrol : " + enemyChase.isPatrol + "\n\n"
            + "Distance : " + enemyChase.dis + "\n\n"
            + "IsAudioEvent : " + enemyChase.findTargetSound + "\n\n"
            + "Target : " + enemyChase.target + "\n\n"
            + "WayPoint : " + enemyChase.patrolPos + "\n\n"
            + "SetTarget : " + enemyChase.enemy.hasPath + "\n\n"
            + "Attack : " + enemyChase.attTarget + "\n\n"
            + "TargetList : ";

        for(int i=0; i< enemyChase.visibleTargets.Count; i++)
        {
            text.text +=  enemyChase.visibleTargets[i] + "\n";
        }            
    }
}
