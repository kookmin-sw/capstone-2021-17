using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Chase : MonoBehaviour
{
    //적의 시야
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    //적의 판단 근거, 장애물인지 플레이어인지
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    //적의 상태들
    enum State {
        Idle,
        FindTarget,
        Move,
        Attack
    }

    private State state;

    //시야에 들어온 적들의 List
    public List<Transform> visibleTargets = new List<Transform>();
    
    // Initialize Players Position
    GameObject[] player;
    //has Path
    public bool hasP = false;
    public bool SetTarget = false;
    //플레이어를 잡았는지 체크
    public bool Check_Catched = false;
    //시야에 적이 들어왔는지 체크
    public bool FindTarget_Vision = false;

    //AI
    NavMeshAgent Enemy;        
    //For Sort by distance
    Dictionary<string, float> distance_target;
    // target
    GameObject target;

    void Start()
    {        
        //기본 상태
        state = State.Idle;
        //추적 시스템을 이용하기 위해 초기화
        Enemy = GetComponent<NavMeshAgent>();
        
        //코루틴을 시작해서 FSM에 들어간다.
        StartCoroutine("Run");
    }

    IEnumerator Run()
    {
        //항시 시야가 가동된다.
        StartCoroutine("FindTargetsWithDelay",0f);

        //첫 코루틴을 시작하면 끝날때까지 while문을 돈다.
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
    //Idle State
    IEnumerator Idle()
    {
        yield return new WaitForSeconds(3f);
        state = State.FindTarget;
    }
  
    IEnumerator FindTargetState()
    {
        while (state == State.FindTarget)
        {
            //임시 태그를 이용해 플레이어들을 찾는다.
            player = GameObject.FindGameObjectsWithTag("Respawn");
            //잡았는지 확인하는 변수 초기화
            Check_Catched = false;
            //딕셔너리를 이용해 플레이어의 이름과 거리를 저장
            distance_target = new Dictionary<string, float>();

            //한명의 적도 발견하지 못한다면 Attack상태로 돌입
            // ** 현재 Attack 상태는 아무것도 아닌 상태 ** 
            if(player.Length == 0)
            {
                state = State.Attack;
                break;
            }

            for (int i = 0; i < player.Length; i++)
            {   
                // 위에 선언한 딕셔너리에 플레이어의 이름과 직선거리를 저장.
                distance_target.Add(player[i].name, Vector3.Distance(transform.position, player[i].transform.position));
            }
            // 거리 순으로 소팅
            var ordered = distance_target.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            // 그 중 가장 가까운 거리에 있는 적을 타겟으로 삼음
            target = GameObject.Find(ordered.First().Key);
            //타겟 설정 변수 설정
            SetTarget = true;
            //Path를 갖고 있다고 알림
            hasP = true;
            // 상태를 Move 상태로 변경
            state = State.Move;
            yield return null;
        }        
    }

    IEnumerator MoveState()
    {                      
        while(state == State.Move)
        {
            //적을 잡으면 다시 Idle 상태로 간다.
            if(Check_Catched)
            {
                //경로가 없다고 알림
                hasP = false;
                state = State.Idle;            
            }
            else
            {
                //계속해서 경로를 설정해서 플레이어가 움직여도 그 경로를 다시 설정한다.
                Enemy.SetDestination(target.transform.position);
                //타겟을 설정했으므로 타겟 설정 변수 초기화
                SetTarget = false;
                yield return null;
            }            
        }
        
    }
    
    IEnumerator AttackState()
    {
        yield return null;
    }

    //시야에 들어온 타겟을 찾는다.
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
            //시야에 들어온 적이 있으면
            if (FindTarget_Vision)
            {
                //그 적을 타겟으로 삼는다.
                SetTarget_Vision();
            }
        }
    }

    //시야에 새로운 적이 들어오면 들어온 적들 중 가장 가까운 타겟으로 타겟 변경
    void SetTarget_Vision()
    {
        //시야에 적이 들어왔으므로 적을 탐지하는 변수 초기화
        FindTarget_Vision = false;

        //위의 FindTargetState와 동일
        distance_target = new Dictionary<string, float>();

        for (int i = 0; i < visibleTargets.Count; i++)
        {
            distance_target.Add(visibleTargets[i].gameObject.name, Vector3.Distance(transform.position, visibleTargets[i].position));
        }                        
        var ordered = distance_target.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        target = GameObject.Find(ordered.First().Key);
        SetTarget = true;
        hasP = true;
        state = State.Move;
    }
    

    //시야에 적이 있는지 없는지 찾는다.
    void FindVisibleTargets()
    {        
        //시야에 들어온 타겟들을 초기화
        visibleTargets.Clear();

        //주변 시야 범위에 들어온 타겟들을 찾는다.
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        //타겟들의 크기만큼 for문을 돌면서 타겟을 설정하고 시야에 들어온 적들을 List에 넣는다.
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                    //적을 탐지하는 변수 설정
                    FindTarget_Vision = true;
                }
            }
        }
    }

    //Scene에서 시야각과 적의 위치를 잇는 선을 긋는다.
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
