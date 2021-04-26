using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MasterServerToolkit.Template.MiniSurvival
{
    public class DollCharacter : MonoBehaviour
    {
        public CharacterController characterController;
        public CapsuleCollider capsuleCollider;

        private void Start()
        {
            characterController.detectCollisions = true;
            characterController.isTrigger = false;
        }

        //private void OnCollisionEnter(Collision collision)
        //{
        //    Debug.Log("Collision Enter");
        //}

        //private void OnTriggerEnter(Collider other)
        //{
        //    Debug.Log("Trigger Enter");
        //}
    }
}