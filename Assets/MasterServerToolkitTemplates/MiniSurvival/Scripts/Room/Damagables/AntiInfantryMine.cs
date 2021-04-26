using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MasterServerToolkit.Template.MiniSurvival
{
    public class AntiInfantryMine : DamageTrigger
    {
        #region INSPECTOR

        [Header("Mine Settings"), SerializeField]
        protected ParticleSystem[] particlesEmmiters;
        [SerializeField]
        protected AudioSource explosionSound;

        #endregion

        private void Awake()
        {
            if (particlesEmmiters != null)
            {
                foreach(var i in particlesEmmiters)
                {
                    i.gameObject.SetActive(false);
                }
            }
        }

        protected override void OnClientTakeDamage()
        {
            if (particlesEmmiters != null)
            {
                foreach (var i in particlesEmmiters)
                {
                    i.gameObject.SetActive(true);
                    i.Play();
                }
            }

            if (explosionSound)
            {
                explosionSound.Play();
            }
        }
    }
}
