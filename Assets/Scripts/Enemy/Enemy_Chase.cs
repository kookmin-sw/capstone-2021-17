using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Chase : MonoBehaviour
{
    enum State {
        Idle,
        FindTarget,
        Move,
        Attack
    }

    private State state;
    
    // Initialize Players Position
    GameObject[] player;
    //has Path
    public bool hasP = false;
    public bool SetTarget = false;
    public bool Check_Catched = false;
    //AI
    NavMeshAgent Enemy;        
    //For Sort by distance
    Dictionary<string, float> distance_target;
    // target
    GameObject target;

    void Start()
    {        
        state = State.FindTarget;
        Enemy = GetComponent<NavMeshAgent>();
        
        StartCoroutine("Run");
    }

    IEnumerator Run()
    {
        while (true)
        {
            switch (state)
            {
                case State.Idle:
                    yield return StartCoroutine("Idle");
                    break;
                case State.FindTarget:
                    yield return StartCoroutine("FindTargetState");
                    break;
                case State.Move:
                    yield return StartCoroutine("MoveState");
                    break;
                case State.Attack:
                    yield return StartCoroutine("AttackState");
                    break;

            }
        }
    }
    IEnumerator Idle()
    {
        yield return new WaitForSeconds(3f);
        state = State.FindTarget;
    }

    IEnumerator FindTargetState()
    {
        while (state == State.FindTarget)
        {
            player = GameObject.FindGameObjectsWithTag("Respawn");
            Check_Catched = false;
            distance_target = new Dictionary<string, float>();
            if(player.Length == 0)
            {
                state = State.Attack;
                break;
            }

            for (int i = 0; i < player.Length; i++)
            {
                distance_target.Add(player[i].name, Vector3.Distance(transform.position, player[i].transform.position));
            }
            var ordered = distance_target.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            target = GameObject.Find(ordered.First().Key);
            SetTarget = true;
            hasP = true;
            state = State.Move;
            yield return null;
        }
        
    }

    IEnumerator MoveState()
    {                      
        while(state == State.Move)
        {
            if(Check_Catched)
            {
                hasP = false;
                state = State.Idle;            
            }
            else
            {
                Enemy.SetDestination(target.transform.position);
                SetTarget = false;
                yield return null;
            }            
        }
        
    }

    IEnumerator AttackState()
    {
        yield return null;
    }
}
