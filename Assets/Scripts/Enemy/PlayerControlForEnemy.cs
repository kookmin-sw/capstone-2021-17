using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(MovePlayerTestForEnemy))]
public class PlayerControlForEnemy : MonoBehaviour
{
    private MovePlayerTestForEnemy character; // A reference to the ThirdPersonCharacter on the object
    public Transform cam;                  // A reference to the main camera in the scenes transform
    public Vector3 camForward;
    public int healthCheck;             // The current forward direction of the camera
    private Vector3 move;
    private bool jump;
    // the world-relative desired move direction, calculated from the camForward and user input.

    [SerializeField]
    private NetGamePlayer netPlayer;

    private void Start()
    {
        // get the third person character ( this should never be null due to require component )
        character = GetComponent<MovePlayerTestForEnemy>();
    }

    private void Update()
    {
        if (!jump)
        {
            jump = Input.GetKeyDown(KeyCode.Space);
        }
        LookAround();
    }

    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cam.rotation.eulerAngles;

        float x = camAngle.x - mouseDelta.y;
        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }
        cam.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // read inputs
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        bool crouch = Input.GetKey(KeyCode.C);

        // calculate move direction to pass to character
        if (cam != null)
        {

            // calculate camera relative direction to move:
            // m_CamForward = new Vector3(m_Cam.forward.x, 0, m_Cam.forward.z).normalized;
            camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            move = v * camForward + h * cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            move = v * Vector3.forward + h * Vector3.right;
            Debug.Log(v);

        }

        // walk speed multiplier
        if (Input.GetKey(KeyCode.LeftShift))
        {
            move *= 0.7f;
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            GetComponent<PlayerHealth>().Hit();

        }

        // pass all parameters to the character control script

        if (netPlayer != null)
        {
            netPlayer.MoveCharacter(move, crouch, jump);
        }
        else
        {
            character.Move(move, crouch, jump);
        }

        jump = false;
    }
}
