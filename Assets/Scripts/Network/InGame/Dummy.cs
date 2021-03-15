using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Mirror;

public class Dummy : NetworkBehaviour
{
    [SerializeField] private CharacterController controller = null;

    private float MovementSpeed = 5f;

    [ClientCallback]
    private void Update()
    {
        if (!hasAuthority)
        {
            return;
        }

        Vector3 movement = new Vector3();
        if(Input.GetKeyDown(KeyCode.A)) { movement.z += 1; }
        if (Input.GetKeyDown(KeyCode.W)) { movement.z -= 1; }
        if (Input.GetKeyDown(KeyCode.S)) { movement.x += 1; }
        if (Input.GetKeyDown(KeyCode.D)) { movement.x -= 1; }

        controller.Move(movement * MovementSpeed * Time.deltaTime);
    }
}
