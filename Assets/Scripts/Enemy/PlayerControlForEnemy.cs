using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class PlayerControlForEnemy : MonoBehaviour
{
    private MovePlayerTestForEnemy character; // A reference to the MovePlayerTestForEnemy on the object
    public Transform cam;                  // A reference to the main camera in the scenes transform
    public Vector3 camForward;            // The current forward direction of the camera
    private Vector3 move;
    private bool jump;                   //jump
    private bool isAttacked = false;
    [SerializeField]
    private NetGamePlayer netPlayer;

    private void Start()
    {
        character = GetComponent<MovePlayerTestForEnemy>();
    }
    public void SetIsAttacked(bool isAttacked)
    {
        this.isAttacked = isAttacked;
    }

    //Set the direction according to the movement of the mouse
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
        LookAround();
        if (!isAttacked)
        {
            //Press space to change the jump to true
            if (!jump)
            {
                jump = Input.GetKeyDown(KeyCode.Space);
            }
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (cam != null)
            {
                camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
                move = v * camForward + h * cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                move = v * Vector3.forward + h * Vector3.right;
            }

            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift))
            {
                move *= 0.7f;
            }

            // pass all parameters to the character control script
            if (netPlayer != null && character.isDie == false)
            {
                netPlayer.MoveCharacter(move, crouch, jump);
            }
            else
            {
                //Can't move after Die
                if (character.isDie == true)
                {
                    move *= 0;
                    character.Move(move, false, false);
                }
                
                else
                {
                    //Can't jump when it's a hit
                    if (character.isHit == true)
                    {
                        character.Move(move, crouch, false);
                    }
                    else
                    {
                        character.Move(move, crouch, jump);
                    }
                }
            }
            //Jump false after moving
            jump = false;
        }
    }
}
