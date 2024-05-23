using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class Enemy : MonoBehaviour
{        
    //�þ� ���� ������
    [Range(0, 360)] 
    [SerializeField] private float viewAngle;
    [SerializeField] private float viewRadius;

    [SerializeField] private float dis = 1000f;   //�÷��̾���� �Ÿ�  
    [SerializeField] private bool findTargetSound = false;    //����� ������ ���� ���� �ƴ��� 
    private bool hasDestination = false;   //Walk �ִϸ��̼��� ����ϱ� ���� ����    
    private bool findTargetVision = false;   //�þ߿� ���� ���Դ��� üũ         
    private int randomIndex;    //������ ���� ����
    private int targetsLength;  //Ÿ�� ����Ʈ�� ����   
    
    //���� �Ǵ� �ٰ�, ��ֹ����� �÷��̾�����
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private Transform[] wayPoint;        //WayPoint - public EnemySpawnManager���� ���� �Ҵ��� �̷�����ߵ�.
    [SerializeField] private List<AnimationSoundEvent> animationEvent = new List<AnimationSoundEvent>();  //����� ������ ���� �ִϸ��̼� �̺�Ʈ
    [SerializeField] private EnemyNetBehaviour enemyNet;
    [SerializeField] private EnemyAnimation anim;       //���ʹ��� ���ϸ��̼��� ��Ʈ���ϴ� Ŭ����
    [SerializeField] private AudioSource siren;           //���̷� ����� �ҽ�
    [SerializeField] private NavMeshAgent navMeshAgent;   //AI
    [SerializeField] private Transform target;            //Ÿ���� ��ġ    
    [SerializeField] private Transform memTarget;       //���� Ÿ���� ����մϴ�.
    [SerializeField] private Collider enemyCollider;    //���� �� Ʈ���Ű� �۵��ǵ��� collider�� �޽��ϴ�.
    [SerializeField] private List<Transform> visibleTargets = new List<Transform>();  //�þ߿� ���� ������ List        

    private StateMachine enemyStateMachine;
    private PatrolState patrol;    
    private IdleState idle;
    private AttackState attack;
    private DizzyState dizzy;
    private ChaseState chase;            
    private Collider[] targetsInViewRadius = new Collider[4];   //OverlapSphereNonAlloc�� ���� ���

    #region Public Methods
    public void AddAnimationSoundEvent(AnimationSoundEvent animationSoundEvent)
    {
        animationEvent.Add(animationSoundEvent);
    }
    public void FindTargets()   //���� ������ �þ߷� �÷��̾ ã�´�.
    {
        //�������� �۵�
        FindVisibleTargets();
        FindTargetWithSound();
        //������ ���� �÷��̾ ������
        if (findTargetVision || findTargetSound)
        {
            //�� �÷��̾ Ÿ������ ��´�.
            SetTargetWithSensor();
        }
    }

    //���� �濡�� ��ġ�� �ʵ��� priority�� �����մϴ�.
    public void SetAgentPriority(int priority)
    {
        navMeshAgent.avoidancePriority = priority;
    }
    public void SetWayPoints(Transform[] wayPoints)
    {
        wayPoint = wayPoints;
    }
    
    //navmeshagent�� ���¸� Ȯ���� ���߰� �ϰų� �����̰� �մϴ�.
    public bool SetNavMeshAgent(bool tf)
    {
        return navMeshAgent.isStopped = tf;
    }
    //���� �ʱ�ȭ
    public void InitializeAll()
    {                
        findTargetVision = false;       //���� �ʱ�ȭ
        findTargetSound = false;        //���� �ʱ�ȭ
        navMeshAgent.speed = 0.5f;      //�ӵ� �ʱ�ȭ
        visibleTargets.Clear();         //Ÿ�� ����Ʈ �ʱ�ȭ
        target = null;                  //Ÿ�� �ʱ�ȭ        
    }

    //�÷��̾� Ÿ���� ��ġ�� �̵��մϴ�.
    public void MoveToTarget()
    {                
        if (target != null)
        {
            navMeshAgent.speed += navMeshAgent.speed * 0.0005f;     //���ʹ��� �ӵ��� ���� ������ŵ�ϴ�.
            anim.SetBlnedTree(navMeshAgent.speed);                  //���� Ʈ�� �� ����
            navMeshAgent.SetDestination(target.position);
            if (!navMeshAgent.hasPath)
            {
                ChangeToIdle();
            }
        }
        else
        {
            ChangeToIdle();
        }        
    }

    //���� �� ����ϴ� ��������Ʈ�� �̵��մϴ�.
    public void MoveToWayPoint()
    {
        if (!navMeshAgent.isStopped)
        {
            randomIndex = Random.Range(0, 26);
            anim.SetBlnedTree(navMeshAgent.speed);      //Blend Tree �ʱ�ȭ        
            navMeshAgent.SetDestination(wayPoint[randomIndex].position);
            if (!navMeshAgent.pathPending)
            {
                ChangeToIdle();
            }
        }
        else
        {
            ChangeToIdle();
        }                        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {            
            ChangeToDizzy();
        }        
    }

    public void ChangeToIdle()
    {
        navMeshAgent.ResetPath();        
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

    //�������� ���̷� �÷��̸� ��Ʈ���մϴ�.
    public void SirenPlay()
    {
        if(enemyNet != null)
        {
            enemyNet.SirenPlay();
        }
        else
        {
            SirenPlaySync();
        }
    }

    public void SirenPlaySync()
    {
        if (!siren.isPlaying)
        {
            siren.Play();
        }
    }

    public void SirenStop()
    {
        if (enemyNet != null)
        {
            enemyNet.SirenStop();
        }
        else
        {
            SirenStopSync();
        }
    }

    public void SirenStopSync()
    {
        siren.Stop();
    }

    //���ʹ� ���� ����
    public void SoundSensorDetect()
    {
        findTargetSound = true;
    }

    public void SoundSensorOff()
    {
        findTargetSound = false;
    }

    public void SetCollider()
    {
        enemyCollider.isTrigger = enemyCollider.isTrigger ? false : true;
    }

    //x�� z��ǥ�� ������ �Ÿ��� ����
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

    public void MemoState() //���� ������Ʈ�� ����մϴ�.
    {
        enemyStateMachine.latestState = enemyStateMachine.currentState;
    }

    public bool IsLatestStateAtt()
    {
        return enemyStateMachine.latestState == attack;
    }

    public bool IsLatestStateDizzy()
    {
        return enemyStateMachine.latestState == dizzy;
    }

    public void PlayWalkAnimation()
    {
        anim.PlayWalkAnim();
    }
    public void StopWalkAnimation()
    {
        anim.StopWalkAnim();
    }
    public void PlayAttAnimation()
    {
        anim.PlayAttAnim();
    }
    public void PlayDizzyAnimation()
    {
        anim.PlayDizzyAnim();
    }

    public void StopDizzyAnimation()
    {
        anim.StopDizzyAnim();
    } 
    #endregion

    #region Private Methods
    private void SetTargetWithSensor()   //�þ߿� ���ο� �÷��̾ ������ ���� ���� �� ���� ����� Ÿ������ Ÿ�� ����
    {
        //������ ���� �������Ƿ� ���� Ž���ϴ� ���� �ʱ�ȭ
        findTargetVision = false;
        findTargetSound = false;
        memTarget = target;
        //Ÿ���� ���ϱ� ���� �ε��� ����
        int targetIndex = 0;

        //Ÿ�ٵ��� �Ÿ� ��
        dis = Vector3.Distance(transform.position, visibleTargets[0].position);

        //���� ª�� �Ÿ��� ã�� ���� for��
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

    private void FindVisibleTargets()     //�þ߿� �÷��̾ �ִ��� ������ ã�´�.
    {
        //�þ߿� ���� Ÿ�ٵ��� �ʱ�ȭ
        visibleTargets.Clear();
        //�ֺ� �þ� ������ ���� Ÿ�ٵ��� ã�´�.
        targetsLength = Physics.OverlapSphereNonAlloc(transform.position, viewRadius, targetsInViewRadius, targetMask);

        //Ÿ�ٵ��� ũ�⸸ŭ for���� ���鼭 Ÿ���� �����ϰ� �þ߿� ���� ������ List�� �ִ´�.
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
                    //���� Ž���ϴ� ���� ����
                    findTargetVision = true;
                }
            }
        }
    }

    private void FindTargetWithSound()    //�ֺ��� �Ҹ��� ������ Ȯ���Ѵ�.
    {
        //����� �̺�Ʈ�� �߻��ϸ�
        if (findTargetSound)
        {            
            //Ÿ�ٸ���Ʈ�� �߰� -> �ӽ� ���� (���� �÷��̾��� ���带 Ž��)
            for (int i = 0; i < animationEvent.Count; i++)
            {                
                if (animationEvent[i] && animationEvent[i].CheckInArea())
                {                    
                    Transform target = animationEvent[i].transform;
                    visibleTargets.Add(target);
                    findTargetSound = true;
                }
            }
        }
    }

    //������ ���ϸ��̼ǿ� ����ϴ� ��� �� �ʱ�ȭ

    private void ChangeToDizzy()
    {     
        enemyStateMachine.ChangeState(dizzy);
    }

    #endregion

    private void Awake()
    {
        enemyStateMachine = new StateMachine();
        idle = new IdleState(this);
        patrol = new PatrolState(this);
        attack = new AttackState(this);
        dizzy = new DizzyState(this);
        chase = new ChaseState(this);

        enemyStateMachine.Initialize(idle);
    }

    private void FixedUpdate()
    {
        if (enemyNet != null && !NetworkServer.active) // Client������ Enemy�� �������� ����
        {
            return;
        }        
        enemyStateMachine.currentState.LogicUpdate();
    }
}