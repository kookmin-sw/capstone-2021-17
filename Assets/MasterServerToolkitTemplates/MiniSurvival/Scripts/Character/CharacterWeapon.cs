using MasterServerToolkit.Bridges.MirrorNetworking.Character;
using Mirror;
using UnityEngine;

namespace MasterServerToolkit.Template.MiniSurvival
{
    public class CharacterWeapon : PlayerCharacterBehaviour
    {
        #region INSPECTOR

        [Header("Components"), SerializeField]
        private PlayerCharacterInput playerCharacterInput;
        [SerializeField]
        private CharacterVitals playerCharacterVitals;
        [SerializeField]
        private CharacterInventory playerCharacterInventory;
        [SerializeField]
        private PlayerCharacterLook playerCharacterLook;

        [Header("Weapon Settings"), SerializeField]
        private BaseWeapon[] weapons;

        #endregion

        /// <summary>
        /// Current selected weapon index
        /// </summary>
        [SyncVar]
        protected int currentSelectedWeaponIndex = 0;

        /// <summary>
        /// Check if component is ready
        /// </summary>
        public override bool IsReady => playerCharacterInput 
            && playerCharacterVitals 
            && playerCharacterVitals.IsAlive 
            && playerCharacterInventory 
            && playerCharacterLook;

        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            if (isLocalPlayer && IsReady)
            {
                if (playerCharacterInput.IsArmed() && playerCharacterInput.IsAttack())
                {
                    MakeShot();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            ChangeWeapon(0);
        }

        /// <summary>
        /// Returns direction from 
        /// </summary>
        /// <returns></returns>
        public virtual Vector3 ShotDirection()
        {
            var weapon = GetCurrentWeapon();

            if(weapon != null && playerCharacterInput.MouseToWorldHitPoint(out RaycastHit hit))
            {
                return hit.point - weapon.ShellSpawnPoint.position;
            }
            else
            {
                return transform.TransformDirection(Vector3.forward);
            }
        }

        /// <summary>
        /// Make shot on the client side
        /// </summary>
        public void MakeShot()
        {
            if (isLocalPlayer)
            {
                // Try get weapon by index
                var weapon = GetWeaponByIndex(currentSelectedWeaponIndex);

                // We have weapon
                if (weapon != null && weapon.FireIsAllowed && playerCharacterInventory.HasItem(weapon.Id) && weapon.HasAmmo)
                {
                    // Melee weapon
                    if (weapon.WeaponType == WeaponType.Melee)
                    {
                        // Here is a melee weapon logic
                    }
                    // Ranged weapon
                    else
                    {
                        // if we are host
                        if (!isServer)
                        {
                            weapon.MakeShot(ShotDirection());
                        }

                        Cmd_MakeShot(ShotDirection());
                    }
                }
            }
        }

        /// <summary>
        /// Make shot on server
        /// </summary>
        [Command]
        protected virtual void Cmd_MakeShot(Vector3 direction)
        {
            var weapon = GetWeaponByIndex(currentSelectedWeaponIndex);

            // Check if weapon is available
            if (weapon != null && playerCharacterInventory.HasItem(weapon.Id) && weapon.HasAmmo)
            {
                weapon.MakeShot(direction);
                Rpc_MakeShot(direction);
            }
        }

        /// <summary>
        /// Make shot on remote client
        /// </summary>
        [ClientRpc]
        protected virtual void Rpc_MakeShot(Vector3 direction)
        {
            if (!isLocalPlayer)
            {
                var weapon = GetWeaponByIndex(currentSelectedWeaponIndex);
                weapon.MakeShot(direction);
            }
        }

        /// <summary>
        /// Tries to change weapon
        /// </summary>
        /// <param name="weaponIndex"></param>
        public void ChangeWeapon(int weaponIndex)
        {
            if (isLocalPlayer)
            {
                Cmd_ChangeWeapon(weaponIndex);
            }
        }

        /// <summary>
        /// Tries to change weapon on server
        /// </summary>
        /// <param name="weaponIndex"></param>
        [Command]
        protected virtual void Cmd_ChangeWeapon(int weaponIndex)
        {
            var weapon = GetWeaponByIndex(weaponIndex);

            if (weapon != null)
            {
                currentSelectedWeaponIndex = weaponIndex;
            }
        }

        /// <summary>
        /// Get weapon by its index in list
        /// </summary>
        /// <param name="weaponIndex"></param>
        /// <returns></returns>
        public BaseWeapon GetWeaponByIndex(int weaponIndex)
        {
            if (weaponIndex >= 0 && weapons != null && weapons.Length > weaponIndex)
            {
                return weapons[weaponIndex];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get currently selected weapon
        /// </summary>
        /// <returns></returns>
        public BaseWeapon GetCurrentWeapon()
        {
            return GetWeaponByIndex(currentSelectedWeaponIndex);
        }
    }
}