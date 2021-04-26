using MasterServerToolkit.Games;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MasterServerToolkit.Template.MiniSurvival
{
    public abstract class DamageTrigger : NetworkBehaviour
    {
        #region INSPECTOR

        [Header("Base Settings"), SerializeField]
        protected float damageValue = 1f;

        [SerializeField]
        protected bool destroyAfterTrigger = true;

        [SerializeField]
        protected float destroyDelay = 0f;

        public UnityEvent OnServerTriggerEnterEvent;
        public UnityEvent OnClientTriggerEnterEvent;

        #endregion

        protected Collider collidedObject;
        protected IDamageable damageableObject;

        protected virtual void OnValidate()
        {
            if (damageValue < 0f)
            {
                damageValue = 0f;
            }

            if (destroyDelay < 0f)
            {
                destroyDelay = 0f;
            }
        }

        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            damageableObject = other.GetComponent<IDamageable>();

            if(damageableObject != null)
            {
                OnServerTakeDamage();
                OnServerTriggerEnterEvent?.Invoke();
                Rpc_OnTriggerEnter();

                if (destroyAfterTrigger)
                {
                    Destroy(gameObject, destroyDelay);
                }
            }
        }

        [ClientRpc]
        private void Rpc_OnTriggerEnter()
        {
            OnClientTakeDamage();
            OnClientTriggerEnterEvent?.Invoke();
        }

        protected virtual void OnServerTakeDamage()
        {
            damageableObject.TakeDamage(damageValue);
        }

        protected virtual void OnClientTakeDamage() { }
    }
}