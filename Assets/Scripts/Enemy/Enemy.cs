using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class Enemy : MonoBehaviour
{    
    public float minErrorWayPoint = 0.5f;   //순찰 지점거리의 최소 오차
    
    [Range(0, 360)] [SerializeField] private float viewAngle;
    [SerializeField] private float viewRadius;
    private float dis;   //플레이어와의 거리  
    private bool hasDestination = false;   //Walk 애니메이션을 사용하기 위한 조건
    private bool isChasing = false;        //Run 애니메이션을 사용하기 위한 조건
    private bool findTargetVision = false;   //시야에 적이 들어왔는지 체크
    private bool findTargetSound = false;    //오디오 센서에 적이 감지 됐는지      
    private int randomIndex;
    private int targetsLength;  //타겟 리스트의 길이
    private int animationEventLength;   //AnimationSoundEvent 컴포넌트를 가진 오브젝트의 legnth        
    
    //적의 판단 근거, 장애물인지 플레이어인지
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private Transform[] wayPoint;        //WayPoint - public EnemySpawnManager에서 동적 할당이 이루어져야됨. 
    [SerializeField] private AnimationSoundEvent[] animationEvent;  //오디오 센서를 위한 애니메이션 이벤트        
    [SerializeField] private EnemyNetBehaviour enemyNet;
    [SerializeField] private EnemyAnimation anim;       //에너미의 에니메이션을 컨트롤하는 클래스
    [SerializeField] private AudioSource siren;           //사이렌 오디오 소스
    [SerializeField] private NavMeshAgent navMeshAgent;   //AI    
    [SerializeField] private Transform target;            //타겟의 위치    
    [SerializeField] private Transform memTarget;

    private StateMachine enemyStateMachine;    
    private PatrolState patrol;
    private List<Transform> visibleTargets = new List<Transform>();  //시야에 들어온 적들의 List        
    private IdleState idle;
    private AttackState attack;
    private DizzyState dizzy;
    private ChaseState chase;            
    private Collider[] targetsInViewRadius = new Collider[4];   //OverlapSphereNonAlloc을 위한 어레이

    #region Public Methods
    public void FindTargets()   //사운드 센서와 시야로 플레이어를 찾는다.
    {
        //센서들을 작동
        FindVisibleTargets();
        FindTargetWithSound();
        //센서에 들어온 플레이어가 있으면
        if (findTargetVision || findTargetSound)
        {
            //그 플레이어를 타겟으로 삼는다.
            SetTargetWithSensor();
        }
    }

    public void SetWayPoints(Transform[] wayPoints)
    {
        wayPoint = wayPoints;
    }
    
    public void InitializeAll()
    {        
        findTargetVision = false;       //센서 초기화
        findTargetSound = false;        //센서 초기화
        navMeshAgent.speed = 0.5f;      //속도 초기화
        visibleTargets.Clear();         //타겟 리스트 초기화
        target = null;                  //타겟 초기화
    }

    //플레이어 타겟의 위치로 이동합니다.
    public void MoveToTarget()
    {
        navMeshAgent.speed += navMeshAgent.speed * 0.0005f;     //에너미의 속도를 점차 증가시킵니다.
        anim.SetBlnedTree(navMeshAgent.speed);                  //블랜드 트리 값 변경
        navMeshAgent.SetDestination(target.position);
    }

    //순찰 시 사용하는 웨이포인트로 이동합니다.
    public void MoveToWayPoint()
    {
        randomIndex = Random.Range(0, 26);
        anim.SetBlnedTree(navMeshAgent.speed);      //Blend Tree 초기화
        navMeshAgent.SetDestination(wayPoint[randomIndex].position);
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

    /*public void OnTriggerEnter(Collider other)
    {
        if (CompareTag("Bullet"))
        {
            ChangeToDizzy();
        }
    }*/

    public void ChangeToIdle()
    {
        enemyStateMachine.ChangeState(idle);
    }

    public void ChangeToPatrol()
    {        
        enemyStateMachine.ChangeState(patrol);
    }

    public void ChangeToChase()
    {
        enemyStateMachine.ChangeState(chase);
    }

    public void ChangeToAttack()
    {
        if (dis <= 1.5f)
        {
            enemyStateMachine.ChangeState(attack);
        }
    }

    public void SirenPlay()
    {
        if (!siren.isPlaying)
        {
            siren.Play();
        }
    }

    public void SirenStop()
    {
        siren.Stop();
    }

    public void SoundSensorDetect()
    {
        findTargetSound = true;
    }

    public float DistanceXZ()
    {
        Vector3 enemyPos = transform.position;
        Vector3 wayPointPos = wayPoint[randomIndex].position;
        enemyPos.y = 0.0f;
        wayPointPos.y = 0.0f;

        return Vector3.Distance(enemyPos, wayPointPos);
    }

    public void SetHasDestination(bool hasDestination)
    {
        this.hasDestination = hasDestination;
    }
    public bool GetHasDestination()
    {
        return hasDestination;
    }

    public void SetIsChasing(bool run)
    {
        isChasing = run;
    }

    public bool GetIsChasing()
    {
        return isChasing;
    }
    #endregion

    #region Private Methods
    private void SetTargetWithSensor()   //시야에 새로운 플레이어가 들어오면 들어온 적들 중 가장 가까운 타겟으로 타겟 변경
    {
        //센서에 적이 들어왔으므로 적을 탐지하는 변수 초기화
        findTargetVision = false;
        findTargetSound = false;
        memTarget = target;
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
        if (target != memTarget)
        {
            ChangeToChase();
        }
    }

    private void FindVisibleTargets()     //시야에 플레이어가 있는지 없는지 찾는다.
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

    private void FindTargetWithSound()    //주변에 소리가 났는지 확인한다.
    {
        //오디오 이벤트가 발생하면
        if (findTargetSound)
        {
            //타겟리스트에 추가 -> 임시 구현 (여러 플레이어의 사운드를 탐지)
            for (int i = 0; i < animationEventLength; i++)
            {
                if (animationEvent[i].CheckInArea())
                {
                    Transform target = animationEvent[i].transform;
                    visibleTargets.Add(target);
                }
            }
        }
    }

    //센서와 에니메이션에 사용하는 모든 것 초기화

    private void ChangeToDizzy()
    {
        enemyStateMachine.ChangeState(dizzy);
    }

    #endregion

    private void Awake()
    {
        animationEvent = FindObjectsOfType<AnimationSoundEvent>();
        animationEventLength = animationEvent.Length;
        Debug.Log(animationEventLength);
        enemyStateMachine = new StateMachine();
        idle = new IdleState(this);
        patrol = new PatrolState(this);
        attack = new AttackState(this, anim);
        dizzy = new DizzyState(this, anim);
        chase = new ChaseState(this);

        enemyStateMachine.Initialize(idle);
    }

    private void FixedUpdate()
    {
        if (enemyNet != null && !NetworkServer.active) // Client에서는 Enemy를 조종하지 않음
        {
            return;
        }        
        enemyStateMachine.currentState.LogicUpdate();
    }
}