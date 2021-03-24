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

        text.text = "State : " + enemyChase.state + "\n"
            + "SetTarget : " + enemyChase.setTarget + "\n"
            + "FindTargetVision : " + enemyChase.findTargetVision + "\n"
            + "IsPatrol : " + enemyChase.isPatrol + "\n"
            + "Distance : " + enemyChase.dis + "\n"
            + "IsAudioEvent : " + enemyChase.findTargetSound + "\n"
            + "Target : " + enemyChase.target + "\n"
            + "TargetList : ";

        for(int i=0; i< enemyChase.visibleTargets.Count; i++)
        {
            text.text +=  enemyChase.visibleTargets[i] + "\n";
        }
            
    }
}
