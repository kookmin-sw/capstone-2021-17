using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    //적의 시야
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    //적의 판단 근거, 장애물인지 플레이어인지
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    //적의 상태들
    enum State 
    {
        Idle,
        Patrol,        
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
    public bool setTarget = false;
    //플레이어를 잡았는지 체크
    public bool isCatched = false;
    //시야에 적이 들어왔는지 체크
    public bool findTargetVision = false;
    //순찰 중인지 체크
    public bool isPatrol = false;
    //순찰 위치 기억
    public Vector3 patrolPos;
    //AI
    NavMeshAgent enemy;        
    //For Sort by distance
    Dictionary<string, float> distanceTarget;
    //WayPoint
    [SerializeField]
    private Transform[] wayPoint;
    // target
    public GameObject target;

    void Awake()
    {        
        //기본 상태
        state = State.Idle;
        //추적 시스템을 이용하기 위해 초기화
        enemy = GetComponent<NavMeshAgent>();        
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
                    yield return StartCoroutine("IdleState");
                    break;
                case State.Patrol:
                    yield return StartCoroutine("PatrolState");
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
    IEnumerator IdleState()
    {
        yield return new WaitForSeconds(5f);
        state = State.Patrol;
    }
  
    IEnumerator PatrolState()
    {
        if (!isPatrol)
        {
            //순찰 시작 전 1초 기다리기
            yield return new WaitForSeconds(1f);
            //웨이 포인트 중 하나를 랜덤으로 접근
            int random = Random.Range(0, 26);
            //순찰중인지 판단
            isPatrol = true;            
            patrolPos = wayPoint[random].position;
            //디버깅용 
            Debug.Log(wayPoint[random].gameObject.name);
            //move state로 전환
            state = State.Move;
            //순찰 시작
            enemy.SetDestination(patrolPos);            
        }
    }        
    IEnumerator MoveState()
    {                      
        while(state == State.Move)
        {
            //순찰중이면
            if (isPatrol)
            {
                //경로를 갖고있고
                hasP = true;     
                //경로에 도달하면
                if ((int)transform.position.x == (int)patrolPos.x && (int)transform.position.z == (int)patrolPos.z)
                {
                    //초기화
                    hasP = false;
                    isPatrol = false;
                    state = State.Patrol;
                }
                else
                {
                    yield return null;
                }                                
            }
            //적을 잡으면 다시 Idle 상태로 간다.
            else if(isCatched)
            {
                //경로가 없다고 알림
                // 모든 변수 초기화
                hasP = false;
                isPatrol = false;
                setTarget = false;
                isCatched = false;
                state = State.Idle;            
            }
            else
            {
                //계속해서 경로를 설정해서 플레이어가 움직여도 그 경로를 다시 설정한다.
                enemy.SetDestination(target.transform.position);
                //타겟을 설정했으므로 타겟 설정 변수 초기화
                setTarget = false;
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
            FindVisibleTargets();
            //시야에 들어온 적이 있으면
            if (findTargetVision)
            {
                isPatrol = false;
                //그 적을 타겟으로 삼는다.
                SetTargetWithVision();
            }
            yield return new WaitForSeconds(delay);
        }
    }

    //시야에 새로운 적이 들어오면 들어온 적들 중 가장 가까운 타겟으로 타겟 변경
    void SetTargetWithVision()
    {
        //시야에 적이 들어왔으므로 적을 탐지하는 변수 초기화
        findTargetVision = false;

        //타겟과 적의 위치를 이름에 맞게 정렬하기 위한 딕셔너리
        distanceTarget = new Dictionary<string, float>();

        //눈에 들어온 타겟만큼 딕셔너리에 추가
        for (int i = 0; i < visibleTargets.Count; i++)
        {
            distanceTarget.Add(visibleTargets[i].gameObject.name, Vector3.Distance(transform.position, visibleTargets[i].position));
        }                        
        //딕셔너리를 거리에 따라 정렬
        var ordered = distanceTarget.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        target = GameObject.Find(ordered.First().Key);        
        setTarget = true;
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
                    findTargetVision = true;
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