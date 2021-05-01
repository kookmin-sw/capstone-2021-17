using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DebugSample : MonoBehaviour
{
    [SerializeField]
    private Text text;
    
    [SerializeField]
    private AnimationSoundEvent animEvent;
    [SerializeField]
    private Enemy enemy;

    // Update is called once per frame
    void Update()
    {

        text.text = "State : " + enemy.enemyStateMachine.currentState + "\n\n"
            + "FindTargetVision : " + enemy.findTargetVision + "\n\n"           
            + "Distance : " + enemy.dis + "\n\n"            
            + "Target : " + enemy.target + "\n\n"
            + "WayPoint : " + enemy.patrol.patrolPos + "\n\n"
            + "SetTarget : " + enemy.navMeshAgent.hasPath + "\n\n"
            + "Sensor : " + enemy.turnOnSensor + "\n\n"
            + "TargetList : ";

        for(int i=0; i< enemy.visibleTargets.Count; i++)
        {
            text.text +=  enemy.visibleTargets[i] + "\n";
        }            
    }
}
