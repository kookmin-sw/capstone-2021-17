using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class EnemyControl : MonoBehaviour
{
    //적의 상태들
    public enum State
    {
        Idle,
        Patrol,
        Move,
        Attack,
        Dizzy
    }    
    public State state;
    //적의 시야
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public bool hasDestination = false;   //Walk 애니메이션을 사용하기 위한 조건                   
    public bool findTargetVision = false;   //시야에 적이 들어왔는지 체크        
    public bool findTargetSound = false;    //오디오 센서에 적이 감지 됐는지        
    public bool isPatrol = false;   //순찰 중인지 체크
    public bool turnOnSensor = true;  //시야와 오디오 센서 온오프.      
    public Vector3 patrolPos;   //순찰 위치 기억    
    public float dis;   //플레이어와의 거리
    public Transform target;    //타겟의 위치
    public List<Transform> visibleTargets = new List<Transform>();  //시야에 들어온 적들의 List

    [SerializeField] public NavMeshAgent enemy;  //AI  
    [SerializeField] public Transform[] wayPoint;   //WayPoint - public EnemySpawnManager에서 동적 할당이 이루어져야됨.   

    //적의 판단 근거, 장애물인지 플레이어인지
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private AnimationSoundEvent[] animationEvent;  //오디오 센서를 위한 애니메이션 이벤트    
    [SerializeField] private EnemyAnimation anim;    
    [SerializeField] private EnemyNetBehaviour enemyNet;
    [SerializeField] private float minErrorWayPoint = 0.5f;    //순찰 지점거리의 최소 오차 
    [SerializeField] private AudioSource siren;                 //사이렌 오디오 소스
    private Collider[] targetsInViewRadius = new Collider[4];   //OverlapSphereNonAlloc을 위한 어레이
    private int targetsLength;  //타겟 리스트의 길이    
    private int animationEventLength = 0;   //AnimationSoundEvent 컴포넌트를 가진 오브젝트의 legnth
    float timer; //딜레이를 위한 타이머 변수

    void Awake()
    {
        //animationEvent = FindObjectsOfType<AnimationSoundEvent>();
        animationEventLength = animationEvent.Length;
        Debug.Log(animationEventLength);
        Console.IsOpen = false;        
        //기본 상태
        state = State.Dizzy;
    }
    
    void Update()
    {
        if(enemyNet != null && !NetworkServer.active) // Client에서는 Enemy를 조종하지 않음
        {
            return;
        }
        
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
            case State.Dizzy:
                DizzyState();
                break;
        }
        FindTargets();
    }   
    //Idle State
    void IdleState()
    {        
        //타이머, 5초 딜레이
        timer += Time.deltaTime;
        if (timer > 5f)
        {
            timer = 0.0f;
            enemy.isStopped = false;
            state = State.Patrol;
        }
    }

    void PatrolState()
    {        
        if (!isPatrol)
        {
            findTargetSound = false;
            //순찰중인지 판단
            isPatrol = true;            
            turnOnSensor = true;
            //웨이 포인트 중 하나를 랜덤으로 접근
            int random = Random.Range(0, wayPoint.Length);            
            patrolPos = wayPoint[random].position;            
            //순찰 시작
            enemy.SetDestination(patrolPos);            
            //move state로 전환
            state = State.Move;            
        }
    }

    void MoveState()
    {
        //순찰중이면
        if (isPatrol)
        {
            //경로를 갖고있고
            hasDestination = true;
            //경로에 도달하면
            /*순찰 지점에 정확하게 도착하는 걸 조건으로 걸면 
             * 제대로 동작이 되지 않는 경우가 많아 
             * 오차 범위를 설정해서 순찰을 진행하도록 한다.
             */
            if (DistanceXZ(transform.position, patrolPos) <= minErrorWayPoint)
            {
                //초기화
                hasDestination = false;
                isPatrol = false;
                state = State.Patrol;                
            }            
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
            */
        }
    }
    
    void AttackState()
    {
        siren.Stop();
        //NavMeshAgent 잠시 멈춤
        enemy.isStopped = true;
        //타겟 초기화
        visibleTargets.Clear();
        anim.PlayAttAnim();
        state = State.Idle;
    }

    void DizzyState()
    {
        //센서를 끈다
        turnOnSensor = false;
        siren.Stop();
        visibleTargets.Clear();
        anim.PlayDizzyAnim();
        state = State.Idle;        
    }

    //시야에 들어온 타겟을 찾는다.
    void FindTargets()
    {
        //센서가 켜지면
        if (turnOnSensor)
        {
            //센서들을 작동
            FindVisibleTargets();
            FindTargetWithSound();
            //센서에 들어온 적이 있으면
            if (findTargetVision || findTargetSound)
            {
                if (!siren.isPlaying)
                {
                    siren.Play();
                }
                isPatrol = false;
                //그 적을 타겟으로 삼는다.
                SetTargetWithSensor();
            }
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
        //공격 범위 설정
        //범위 내에 있으면 Attack 스테이트로, 아니면 그대로 추격
        if (dis <= 1.5f)
        {
            //변수초기화
            InitializeVar();
            state = State.Attack;
        }        
        else
        {
            hasDestination = true;
            state = State.Move;
        }
    }

    void FindTargetWithSound()
    {
        //오디오 이벤트가 발생하면
        if (findTargetSound)
        {
            //타겟리스트에 추가 -> 임시 구현 (여러 플레이어의 사운드를 탐지)
            for(int i=0; i<animationEventLength; i++)
            {                
                if (animationEvent[i].isInArea)
                {
                    Transform target = animationEvent[i].transform;
                    visibleTargets.Add(target);
                }                
            }            
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
        hasDestination = false;        
        findTargetVision = false;
        findTargetSound = false;
        turnOnSensor = false;
        isPatrol = false;
    }

    //position의 x와 z만 가지고 거리를 체크하는 함수
    float DistanceXZ(Vector3 posFirst, Vector3 posSecond)
    {
        posFirst.y = 0.0f;
        posSecond.y = 0.0f;

        return Vector3.Distance(posFirst, posSecond);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (CompareTag("Bullet"))
        {
            state = State.Dizzy;
        }
    }
}