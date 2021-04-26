using MasterServerToolkit.Games;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MasterServerToolkit.Template.MiniSurvival
{
    [RequireComponent(typeof(Rigidbody))]
    public class BaseShell : MonoBehaviour
    {
        protected Vector3 lastPosition;

        #region INSPECTOR

        [Header("Settings"), SerializeField]
        protected float shootPower = 100f;
        [SerializeField]
        protected float lifeTime = 3f;
        [SerializeField]
        protected float damageValue = 10f;
        [SerializeField]
        protected GameObject helperPrefab;

        #endregion

        protected virtual void OnValidate()
        {
            if (shootPower <= 0) shootPower = 1f;
            if (lifeTime <= 0) lifeTime = 1f;
            if (damageValue <= 0) damageValue = 1f;
        }

        protected virtual void Start()
        {
            lastPosition = transform.position;

            GetComponent<Rigidbody>().AddForce(transform.forward * shootPower);
            Destroy(gameObject, lifeTime);
        }

        protected virtual void Update()
        {
            if (lastPosition != transform.position && Physics.Raycast(lastPosition, transform.position - lastPosition, out RaycastHit hitInfo, Vector3.Distance(lastPosition, transform.position)))
            {
                Instantiate(helperPrefab, hitInfo.point, Quaternion.identity);
                hitInfo.collider.GetComponentInChildren<IDamageable>()?.TakeDamage(damageValue);

                //Debug.Log(hitInfo.collider.name);
                Destroy(gameObject);
            }

            lastPosition = transform.position;
        }
    }
}