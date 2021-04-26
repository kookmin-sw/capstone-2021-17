using MasterServerToolkit.Bridges.MirrorNetworking.Character;
using Mirror;
using System.Linq;
using UnityEngine;

namespace MasterServerToolkit.Template.MiniSurvival
{
    public class CharacterAvatar : PlayerCharacterBehaviour
    {
        /// <summary>
        /// Character avatar Id
        /// </summary>
        [SyncVar(hook = nameof(AvatarIdChanged))]
        protected string assignedAvatarId = string.Empty;

        [Header("Components"), SerializeField]
        protected CharacterAnimator animatorController;
        [SerializeField]
        protected GameObject[] avatarsList;

        /// <summary>
        /// Sets character avatar Id
        /// </summary>
        /// <param name="avatarId"></param>
        [Server]
        public void SetAvatar(string avatarId)
        {
            assignedAvatarId = avatarId;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            // Set character avatar on server
            SetAvatarObject(assignedAvatarId);
        }

        /// <summary>
        /// Invokes on client when <see cref="assignedAvatarId"/> is changed on server
        /// </summary>
        /// <param name="oldId"></param>
        /// <param name="newId"></param>
        private void AvatarIdChanged(string oldId, string newId)
        {
            SetAvatarObject(newId);
        }

        /// <summary>
        /// Sets the avatar object and all its components
        /// </summary>
        /// <param name="avatarId"></param>
        protected virtual void SetAvatarObject(string avatarId)
        {
            if (avatarsList == null || avatarsList.Length == 0)
            {
                throw new System.Exception("No avatar found");
            }

            // Disable all active avatars
            DisableAll();

            // Find avatar by Id
            GameObject avatarObject = avatarsList.ToList().Find(ava => ava.name == assignedAvatarId);

            // If no, let's get random avatar
            if (!avatarObject)
            {
                avatarObject = avatarsList[UnityEngine.Random.Range(0, avatarsList.Length)];
            }
            
            // Set synced var of avatar id
            assignedAvatarId = avatarObject.name;

            // Activate found avatar
            avatarObject.SetActive(true);

            // Set avatar's animator controller
            animatorController.SetAnimator(avatarObject.GetComponent<Animator>());
        }

        /// <summary>
        /// Disable all avatars
        /// </summary>
        public void DisableAll()
        {
            if (avatarsList == null || avatarsList.Length == 0)
            {
                throw new System.Exception("No avatar found");
            }

            foreach(var avatarObject in avatarsList)
            {
                avatarObject.SetActive(false);
            }
        }
    }
}