using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


[RequireComponent(typeof(ThirdPersonCharacter))]
public class ThirdPersonUserControl : MonoBehaviour
{
    private ThirdPersonCharacter Character; // A reference to the ThirdPersonCharacter on the object
    public Transform Cam;                  // A reference to the main camera in the scenes transform
    public Vector3 CamForward;
    public int HealthCheck;             // The current forward direction of the camera
    private Vector3 Move;
    private bool Jump;
                     // the world-relative desired move direction, calculated from the camForward and user input.

    [SerializeField]
    private NetGamePlayer NetPlayer;

    private void Start()
    {
        // get the third person character ( this should never be null due to require component )
        Character = GetComponent<ThirdPersonCharacter>();
    }

    private void Update()
    {
        if (!Jump)
        {
            Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }
        LookAround();
    }
    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = Cam.rotation.eulerAngles;

        float x = camAngle.x - mouseDelta.y;
        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }
        Cam.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {

        // read inputs
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        bool crouch = Input.GetKey(KeyCode.C);

        // calculate move direction to pass to character
        if (Cam != null)
        {

            // calculate camera relative direction to move:
            // m_CamForward = new Vector3(m_Cam.forward.x, 0, m_Cam.forward.z).normalized;
            CamForward = Vector3.Scale(Cam.forward, new Vector3(1, 0, 1)).normalized;
            Move = v * CamForward + h * Cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            Move = v * Vector3.forward + h * Vector3.right;
            Debug.Log(v);

        }

        // walk speed multiplier
        if (Input.GetKey(KeyCode.LeftShift))
        { 
            Move *= 0.7f;
        }
        if (Input.GetKeyUp(KeyCode.B)) 
        {
            GetComponent<PlayerHealth>().Hit();
        }

        ChangeSpeed(HealthCheck);

        // pass all parameters to the character control script
        
        if(NetPlayer != null)
        {
            NetPlayer.MoveCharacter(Move, crouch, Jump);
        }
        else
        {
            Character.Move(Move, crouch, Jump);
        }

        Jump = false;
    }
    public void ChangeSpeed(int healthcheck)
    {

    }
}

