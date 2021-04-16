using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DebugSample : MonoBehaviour
{
    [SerializeField]
    private Text text;

    [SerializeField]
    private EnemyControl enemyControl;
    [SerializeField]
    private AnimationSoundEvent animEvent;
    // Update is called once per frame
    void Update()
    {

        text.text = "State : " + enemyControl.state + "\n\n"
            + "FindTargetVision : " + enemyControl.findTargetVision + "\n\n"
            + "IsPatrol : " + enemyControl.isPatrol + "\n\n"
            + "Distance : " + enemyControl.dis + "\n\n"
            + "IsAudioEvent : " + enemyControl.findTargetSound + "\n\n"
            + "Target : " + enemyControl.target + "\n\n"
            + "WayPoint : " + enemyControl.patrolPos + "\n\n"
            + "SetTarget : " + enemyControl.enemy.hasPath + "\n\n"
            + "Sensor : " + enemyControl.turnOnSensor + "\n\n"
            + "TargetList : ";

        for(int i=0; i< enemyControl.visibleTargets.Count; i++)
        {
            text.text +=  enemyControl.visibleTargets[i] + "\n";
        }            
    }
}
