using UnityEngine;
using System;


//Add Component
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]

public class ThirdPersonCharacter : MonoBehaviour
{
    [SerializeField] float MovingTurnSpeed = 600;
    [SerializeField] float StationaryTurnSpeed = 120;
    [SerializeField] float JumpPower = 8f;
    [Range(1f, 4f)] [SerializeField] float GravityMultiplier = 2f;
    [SerializeField] float RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    [SerializeField] float MoveSpeedMultiplier = 2f;
    [SerializeField] float AnimSpeedMultiplier = 1f;
    [SerializeField] float GroundCheckDistance = 0.3f;

    Rigidbody PlayerRigidbody;
    Animator PlayerAnimator;
    

    CapsuleCollider Capsule;
    float CapsuleHeight;
    Vector3 CapsuleCenter;
    Vector3 GroundNormal;

    const float k_Half = 0.5f;
    float TurnAmount;
    float ForwardAmount;
    
    public bool IsGrounded;
    float OrigGroundCheckDistance;

    public bool IsHit;
    public bool IsDie;

    bool Crouching;
    Vector3 PresentMove;
    
    //NetPlayer
    [SerializeField]
    NetGamePlayer NetPlayer;

    public AudioSource soundSource;
    
    //Player State
    public enum State
    {
        Idle,
        Crouch,
        Move,
        Jump,
        Hit,
        Die
    }
    
    //Player State Idle Setting
    public State state =State.Idle;

    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        PlayerRigidbody = GetComponent<Rigidbody>();
        Capsule = GetComponent<CapsuleCollider>();
        CapsuleHeight = Capsule.height;
        CapsuleCenter = Capsule.center;
        PlayerRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        OrigGroundCheckDistance = GroundCheckDistance;
    }

    public void Move(Vector3 move, bool crouch, bool jump)
    {
        bool isPlayWalk = false;
        bool isStopWalk = false;
        int oneShotId = -1;
        
        if (PresentMove != move && crouch == false&& IsGrounded)
        {
            if (soundSource.isPlaying==false)
            {
                isPlayWalk = true;
            }         
            state=State.Move;
        }
        else if (crouch)
        {
            isStopWalk = true;
            state=State.Crouch;
        }
        else if (crouch&&PresentMove != move)
        {
            isStopWalk = true;
            state=State.Crouch;
            if (soundSource.isPlaying==false)
            {
                isPlayWalk = true;
            }
        }
        else if (jump)
        {
            state=State.Jump;
        }
        else if (IsHit==true)
        {
            isStopWalk = true;
            state =State.Hit;
        }
        else if (IsDie==true)
        {
            isStopWalk = true;
            state =State.Die;
        }
        else
        {
            state=State.Idle;
            isStopWalk = true;
        }

        if (NetPlayer != null)
        {
            NetPlayer.ChangeState(state);
            if (isPlayWalk)
            {
                NetPlayer.PlaySound();
            }

            if(isStopWalk)
            {
                NetPlayer.StopSound();
            }  
        }
        else
        {
            if (isPlayWalk)
            {
                soundSource.Play();
            }
            if(isStopWalk)
            {
                soundSource.Stop();
            }

        }
       
        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        CheckGroundStatus();
        move = Vector3.ProjectOnPlane(move, GroundNormal);
        TurnAmount = Mathf.Atan2(move.x, move.z);
        ForwardAmount = move.z;

        ApplyExtraTurnRotation();

        // control and velocity handling is different when grounded and airborne:
        if (IsGrounded)
        {
            HandleGroundedMovement(crouch, jump);
        }
        else
        {
            HandleAirborneMovement();
        }

        ScaleCapsuleForCrouching(crouch);
        PreventStandingInLowHeadroom();

        // send input and other state parameters to the animator
        UpdateAnimator(move);
        PresentMove=move;
    }

    void ScaleCapsuleForCrouching(bool crouch)
    {
        if (IsGrounded && crouch)
        {
            if (Crouching) 
            {
                return;
            }
            Capsule.height = Capsule.height / 2f;
            Capsule.center = Capsule.center / 2f;
            Crouching = true;
        }
        else
        {
            Ray crouchRay = new Ray(PlayerRigidbody.position + Vector3.up * Capsule.radius * k_Half, Vector3.up);
            float crouchRayLength = CapsuleHeight - Capsule.radius * k_Half;
            if (Physics.SphereCast(crouchRay, Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                Crouching = true;
                return;
            }
            Capsule.height = CapsuleHeight;
            Capsule.center = CapsuleCenter;
            Crouching = false;
        }
    }

    void PreventStandingInLowHeadroom()
    {
        // prevent standing up in crouch-only zones
        if (!Crouching)
        {
            Ray crouchRay = new Ray(PlayerRigidbody.position + Vector3.up * Capsule.radius * k_Half, Vector3.up);
            float crouchRayLength = CapsuleHeight - Capsule.radius * k_Half;
            if (Physics.SphereCast(crouchRay, Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                Crouching = true;
            }
        }
    }

    void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        PlayerAnimator.SetFloat("Forward", ForwardAmount, 0.1f, Time.deltaTime);
        PlayerAnimator.SetBool("Crouch", Crouching);
        PlayerAnimator.SetBool("OnGround", IsGrounded);
        PlayerAnimator.SetBool("Hit", IsHit);
        PlayerAnimator.SetBool("Die", IsDie);
        if (!IsGrounded)
        {
            PlayerAnimator.SetFloat("Jump", PlayerRigidbody.velocity.y);
        }

        // calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // (This code is reliant on the specific run cycle offset in our animations,
        // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
        float runCycle =
            Mathf.Repeat(
                PlayerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime + RunCycleLegOffset, 1);
        float jumpLeg = (runCycle < k_Half ? 1 : -1) * ForwardAmount;
        if (IsGrounded)
        {
            PlayerAnimator.SetFloat("JumpLeg", jumpLeg);
        }

        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (IsGrounded && move.magnitude > 0)
        {
            PlayerAnimator.speed = AnimSpeedMultiplier;
        }
        else
        {
            // don't use that while airborne
            PlayerAnimator.speed = 1;
        }
    }

    void HandleAirborneMovement()
    {
        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity * GravityMultiplier) - Physics.gravity;
        PlayerRigidbody.AddForce(extraGravityForce);

        GroundCheckDistance = PlayerRigidbody.velocity.y < 0 ? OrigGroundCheckDistance : 0.01f;
    }

    void HandleGroundedMovement(bool crouch, bool jump)
    {
        // check whether conditions are right to allow a jump:
        if (jump && !crouch)
        {
            //&& PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Grounded")
            // jump!
            PlayerRigidbody.velocity = new Vector3(PlayerRigidbody.velocity.x, JumpPower, PlayerRigidbody.velocity.z);
            IsGrounded = false;
            PlayerAnimator.applyRootMotion = false;
            GroundCheckDistance = 0.1f;
        }
    }

    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(StationaryTurnSpeed, MovingTurnSpeed, ForwardAmount);
        transform.Rotate(0, TurnAmount * turnSpeed * Time.deltaTime, 0);
    }

    public void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        if (IsGrounded && Time.deltaTime > 0)
        {
            Vector3 v = (PlayerAnimator.deltaPosition * MoveSpeedMultiplier) / Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            v.y = PlayerRigidbody.velocity.y;
            PlayerRigidbody.velocity = v;
        }
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * GroundCheckDistance));
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, GroundCheckDistance))
        {
            GroundNormal = hitInfo.normal;
            IsGrounded = true;
            PlayerAnimator.applyRootMotion = true;
        }
        else
        {
            IsGrounded = false;
            GroundNormal = Vector3.up;
            PlayerAnimator.applyRootMotion = false;
        }
    }
}

