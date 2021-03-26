using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Popcron.Console;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    //적의 상태들
    public enum State
    {
        Idle,
        Patrol,
        Move,
        Attack
    }
    [Command("state")]
    public State state;

    //적의 시야
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public bool hasP = false;   //Path가 있는지    
    public bool isCatched = false;  //플레이어를 잡았는지 체크    
    [Command("setTarget")]
    public bool setTarget = false;  //타겟을 설정했는지    
    [Command("findTargetVision")]
    public bool findTargetVision = false;   //시야에 적이 들어왔는지 체크    
    [Command("findTargetSound")]
    public bool findTargetSound = false;    //오디오 센서에 적이 감지 됐는지    
    [Command("isPatrol")]
    public bool isPatrol = false;   //순찰 중인지 체크
    public bool attTarget = false;  //Attack 스테이트로 갈지 확인    

    public Vector3 patrolPos;   //순찰 위치 기억      
    [Command("distance")]
    public float dis;   //플레이어와의 거리

    [SerializeField] public NavMeshAgent enemy;  //AI
    public Transform target;    //타겟의 위치
    
    public List<Transform> visibleTargets = new List<Transform>();  //시야에 들어온 적들의 List

    [SerializeField] private Transform[] wayPoint;   //WayPoint    
    //적의 판단 근거, 장애물인지 플레이어인지
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private AnimationEvent animationEvent;  //오디오 센서를 위한 애니메이션 이벤트    
    [SerializeField] private MoveObjectTest anim;
    private Collider[] targetsInViewRadius = new Collider[4];   //OverlapSphereNonAlloc을 위한 어레이
    private int targetsLength;  //타겟 리스트의 길이

    float timer; //딜레이를 위한 타이머 변수
    void Awake()
    {
        Console.IsOpen = false;
        //기본 상태
        state = State.Idle;                
    }

    void Update()
    {
        FindTargets();
        switch (state)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Patrol:
                PatrolState();
                break;
            case State.Move:
                MoveState();
                break;
            case State.Attack:
                AttackState();
                break;
        }
    }
    private void OnEnable()
    {
        Parser.Register(this, "enemy");
    }

    private void OnDisable()
    {
        Parser.Unregister(this);
    }

    //Idle State
    void IdleState()
    {
        //NavMeshAgent 활성화
        enemy.isStopped = false;
        //타이머, 1초 딜레이
        timer += Time.deltaTime;
        if (timer > 1f)
        {
            timer = 0.0f;           
            state = State.Patrol;
        }        
    }

    void PatrolState()
    {        
        if (!isPatrol)
        {
            //웨이 포인트 중 하나를 랜덤으로 접근
            int random = Random.Range(0, 26);
            //순찰중인지 판단
            isPatrol = true;            
            patrolPos = wayPoint[random].position;                        
            //순찰 시작
            enemy.SetDestination(patrolPos);
            //move state로 전환
            state = State.Move;
            return;
        }
    }

    void MoveState()
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
                return;
            }
            else
            {
                return;
            }
        }
        //적을 잡으면 다시 Idle 상태로 간다.
        else if (isCatched)
        {
            //경로가 없다고 알림
            // 모든 변수 초기화
            InitializeVar();
            state = State.Idle;
            return;
        }
        else
        {
            //계속해서 경로를 설정해서 플레이어가 움직여도 그 경로를 다시 설정한다.
            enemy.SetDestination(target.position);
            //타겟을 설정했으므로 타겟 설정 변수 초기화
            /* NavMesh위에 플레이어가 있지 않을때
            if (!enemy.hasPath)
            {
                state = State.Idle;
            }
            else
            {
                setTarget = false;
            }*/
            setTarget = false;
        }
    }
    
    void AttackState()
    {
        //변수 초기화 - Idle 상태로 가기 때문
        InitializeVar();
        //NavMeshAgent를 잠시 멈춤.
        enemy.isStopped = true;    
        //애니메이션 출력
        anim.PlayAttAnim();
        state = State.Idle;
    }

    //시야에 들어온 타겟을 찾는다.
    void FindTargets()
    {
        FindVisibleTargets();
        FindTargetWithSound();
        //시야에 들어온 적이 있으면
        if (findTargetVision || findTargetSound)
        {
            isPatrol = false;
            //그 적을 타겟으로 삼는다.
            SetTargetWithSensor();
        }        
    }

    //시야에 새로운 적이 들어오면 들어온 적들 중 가장 가까운 타겟으로 타겟 변경
    void SetTargetWithSensor()
    {
        //시야에 적이 들어왔으므로 적을 탐지하는 변수 초기화
        findTargetVision = false;
        findTargetSound = false;
        //타겟을 정하기 위한 인덱스 변수
        int targetIndex = 0;
        
        //타겟들의 거리 값
        dis = Vector3.Distance(transform.position, visibleTargets[0].position);

        //가장 짧은 거리를 찾기 위한 for문
        for (int i = 1; i < visibleTargets.Count; i++)
        {
            float temp = Vector3.Distance(transform.position, visibleTargets[i].position);
            if (dis > temp)
            {
                dis = temp;
                targetIndex = i;
            }
        }
        target = visibleTargets[targetIndex];
        setTarget = true;
        //공격 범위 설정
        if (dis <= 1.5f)
        {            
            //범위 내에 있으면 어택
            state = State.Attack;
        }
        else
        {
            //아니면 그대로 추격
            hasP = true;
            state = State.Move;
        }  
    }

    void FindTargetWithSound()
    {
        //오디오 이벤트가 발생하면
        if (animationEvent.audioEvent)
        {
            //타겟리스트에 추가
            Transform target = animationEvent.transform;
            visibleTargets.Add(target);
        }
    }

    //시야에 적이 있는지 없는지 찾는다.
    void FindVisibleTargets()
    {        
        //시야에 들어온 타겟들을 초기화
        visibleTargets.Clear();        
        //주변 시야 범위에 들어온 타겟들을 찾는다.
        targetsLength = Physics.OverlapSphereNonAlloc(transform.position, viewRadius, targetsInViewRadius, targetMask);
        
        //타겟들의 크기만큼 for문을 돌면서 타겟을 설정하고 시야에 들어온 적들을 List에 넣는다.
        for (int i = 0; i < targetsLength; i++)
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

    //변수 초기화 함수
    void InitializeVar()
    {
        hasP = false;
        isCatched = false;
        setTarget = false;
        findTargetVision = false;
        findTargetSound = false;
        isPatrol = false;
        attTarget = false;
    }
}