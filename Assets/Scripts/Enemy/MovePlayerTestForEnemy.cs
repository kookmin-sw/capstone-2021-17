using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerTestForEnemy : MonoBehaviour
{    
    [SerializeField] float movingTurnSpeed = 600;
    [SerializeField] float stationaryTurnSpeed = 120;
    [SerializeField] float jumpPower = 8f;
    [Range(1f, 4f)] [SerializeField] float gravityMultiplier = 2f;
    [SerializeField] float runCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    [SerializeField] float moveSpeedMultiplier = 2f;
    [SerializeField] float animSpeedMultiplier = 1f;
    [SerializeField] float groundCheckDistance = 0.3f;

    TransitionSound transition;
    Rigidbody playerRigidbody;
    Animator playerAnimator;

    CapsuleCollider capsule;
    float capsuleHeight;
    Vector3 capsuleCenter;
    Vector3 groundNormal;

    const float kHalf = 0.5f;
    float turnAmount;
    float forwardAmount;

    public bool isGrounded;
    float origGroundCheckDistance;

    public bool isHit;
    public bool isDie;
    public bool isHeal;

    bool crouching;
    Vector3 presentMove;

    //NetPlayer
    [SerializeField]
    NetGamePlayer netPlayer;

    AudioSource soundSources;
    AnimationSoundEvent animationSoundEvent;
    float timer = 0.0f;
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
    public State state = State.Idle;
    bool isPlayWalk = false;    
    void Start()
    {                
        soundSources = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
        transition = playerAnimator.GetBehaviour<TransitionSound>();
        animationSoundEvent = GetComponent<AnimationSoundEvent>();
        playerRigidbody = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        capsuleHeight = capsule.height;
        capsuleCenter = capsule.center;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        origGroundCheckDistance = groundCheckDistance;
        transition.SetAudioSource(soundSources);
        transition.SetAnimationEvnet(animationSoundEvent);
    }

    public void Move(Vector3 move, bool crouch, bool jump)
    {

        if (presentMove != move && crouch == false && isGrounded)
        {
            isPlayWalk = true;

            state = State.Move;
        }
        else if (crouch)
        {
            isPlayWalk = false;
            if (crouch && presentMove != move)
            {
                isPlayWalk = true;
            }
            state = State.Crouch;
        }
        else if (crouch && presentMove != move)
        {

        }
        else if (jump)
        {
            state = State.Jump;
        }
        else if (isHit == true && isDie == false)
        {
            isPlayWalk = false;
            state = State.Hit;
        }
        else if (isDie == true)
        {
            isPlayWalk = false;
            state = State.Die;
        }
        else
        {
            state = State.Idle;
            isPlayWalk = false;
        }

        if (netPlayer != null)
        {
            //NetPlayer.ChangeState(state);
            if (isPlayWalk && !isHeal)
            {
                netPlayer.PlaySound();
            }

            if (!isPlayWalk)
            {
                netPlayer.StopSound();
            }
        }

        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        CheckGroundStatus();
        move = Vector3.ProjectOnPlane(move, groundNormal);
        turnAmount = Mathf.Atan2(move.x, move.z);
        forwardAmount = move.z;

        ApplyExtraTurnRotation();

        // control and velocity handling is different when grounded and airborne:
        if (isGrounded)
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
        presentMove = move;
    }

    void ScaleCapsuleForCrouching(bool crouch)
    {
        if (isGrounded && crouch)
        {
            if (crouching)
            {
                return;
            }
            capsule.height = capsule.height / 2f;
            capsule.center = capsule.center / 2f;
            crouching = true;
        }
        else
        {
            Ray crouchRay = new Ray(playerRigidbody.position + Vector3.up * capsule.radius * kHalf, Vector3.up);
            float crouchRayLength = capsuleHeight - capsule.radius * kHalf;
            if (Physics.SphereCast(crouchRay, capsule.radius * kHalf, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                crouching = true;
                return;
            }
            capsule.height = capsuleHeight;
            capsule.center = capsuleCenter;
            crouching = false;
        }
    }

    void PreventStandingInLowHeadroom()
    {
        // prevent standing up in crouch-only zones
        if (!crouching)
        {
            Ray crouchRay = new Ray(playerRigidbody.position + Vector3.up * capsule.radius * kHalf, Vector3.up);
            float crouchRayLength = capsuleHeight - capsule.radius * kHalf;
            if (Physics.SphereCast(crouchRay, capsule.radius * kHalf, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                crouching = true;
            }
        }
    }

    void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        playerAnimator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        playerAnimator.SetBool("Crouch", crouching);
        playerAnimator.SetBool("OnGround", isGrounded);
        playerAnimator.SetBool("Hit", isHit);        
        playerAnimator.SetBool("isPlayWalk", isPlayWalk);
        if (!isGrounded)
        {
            playerAnimator.SetFloat("Jump", playerRigidbody.velocity.y);
        }

        // calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // (This code is reliant on the specific run cycle offset in our animations,
        // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
        float runCycle =
            Mathf.Repeat(
                playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime + runCycleLegOffset, 1);
        float jumpLeg = (runCycle < kHalf ? 1 : -1) * forwardAmount;
        if (isGrounded)
        {
            playerAnimator.SetFloat("JumpLeg", jumpLeg);
        }

        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (isGrounded && move.magnitude > 0)
        {
            playerAnimator.speed = animSpeedMultiplier;
        }
        else
        {
            // don't use that while airborne
            playerAnimator.speed = 1;
        }
    }

    void HandleAirborneMovement()
    {
        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
        playerRigidbody.AddForce(extraGravityForce);

        groundCheckDistance = playerRigidbody.velocity.y < 0 ? origGroundCheckDistance : 0.01f;
    }

    void HandleGroundedMovement(bool crouch, bool jump)
    {
        // check whether conditions are right to allow a jump:
        if (jump && !crouch)
        {
            //&& PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Grounded")
            // jump!
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, jumpPower, playerRigidbody.velocity.z);
            isGrounded = false;
            playerAnimator.applyRootMotion = false;
            groundCheckDistance = 0.1f;
        }
    }

    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
        transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
    }

    public void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        if (isGrounded && Time.deltaTime > 0)
        {
            Vector3 v = (playerAnimator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            v.y = playerRigidbody.velocity.y;
            playerRigidbody.velocity = v;
        }
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * groundCheckDistance));
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundCheckDistance))
        {
            groundNormal = hitInfo.normal;
            isGrounded = true;
            playerAnimator.applyRootMotion = true;
        }
        else
        {
            isGrounded = false;
            groundNormal = Vector3.up;
            playerAnimator.applyRootMotion = false;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {            
            Vector3 reactVec  = (transform.position - other.transform.position);
            reactVec.z = 5*Mathf.Abs(reactVec.z) / reactVec.z;
            reactVec.y =1f;
            playerRigidbody.AddRelativeForce(reactVec, ForceMode.Impulse);            
        }
    }    
}
