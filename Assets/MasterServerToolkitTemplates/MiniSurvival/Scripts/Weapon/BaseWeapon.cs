using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MasterServerToolkit.Template.MiniSurvival
{
    public class BaseWeapon : MonoBehaviour, IWeapon
    {
        #region INSPECTOR

        [Header("Base Components"), SerializeField]
        protected BaseShell shellPrefab;
        [SerializeField]
        protected Transform shellSpawnPoint;
        [SerializeField]
        protected AudioSource audioSource;
        [SerializeField]
        protected WeaponType weaponType = WeaponType.Ranged;

        [Header("Settings"), SerializeField]
        protected string id = "";
        [SerializeField]
        protected float fireRate = 1f;
        [SerializeField]
        protected float currentAmmo = 1;
        [SerializeField]
        protected float maxAmmo = 1;
        [SerializeField]
        protected bool noAmmo = false;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        protected float lastShotTime = -10f;

        /// <summary>
        /// The point this weapon aims at
        /// </summary>
        protected Vector3 targetPoint = Vector3.zero;

        /// <summary>
        /// 
        /// </summary>
        public string Id
        {
            get
            {
                return id;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Transform ShellSpawnPoint
        {
            get
            {
                return shellSpawnPoint;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public BaseShell ShellPrefab
        {
            get
            {
                return shellPrefab;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool FireIsAllowed
        {
            get
            {
                return lastShotTime <= Time.time;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasAmmo
        {
            get
            {
                return noAmmo || currentAmmo > 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public WeaponType WeaponType
        {
            get
            {
                return weaponType;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void ReduceAmmo()
        {
            currentAmmo = Mathf.Clamp(currentAmmo - 1, 0, maxAmmo);
        }

        /// <summary>
        /// Make shot
        /// </summary>
        public virtual void MakeShot(Vector3 direction)
        {
            lastShotTime = fireRate + Time.time;
            Instantiate(ShellPrefab, ShellSpawnPoint.position, Quaternion.LookRotation(direction, Vector3.up));

            if (audioSource)
            {
                audioSource.Play();
            }
        }

        /// <summary>
        /// Sets the point this weapon aims at
        /// </summary>
        /// <param name="point"></param>
        public void SetTargetPoint(Vector3 point)
        {
            targetPoint = point;
        }
    }
}
