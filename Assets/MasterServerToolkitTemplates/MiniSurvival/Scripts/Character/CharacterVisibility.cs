using MasterServerToolkit.Bridges.MirrorNetworking.Character;
using System.Linq;
using UnityEngine;

namespace MasterServerToolkit.Template.MiniSurvival
{
    public class CharacterVisibility : PlayerCharacterBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
#pragma warning disable 0414
        private bool isVisible = true;

        /// <summary>
        /// 
        /// </summary>
        private PlayerCharacterVitals localCharacterVitals;

        /// <summary>
        /// 
        /// </summary>
        private Renderer[] renderers;

        /// <summary>
        /// 
        /// </summary>
        public override bool IsReady => localCharacterVitals;

        /// <summary>
        /// 
        /// </summary>
        private void Start()
        {
            renderers = GetComponentsInChildren<Renderer>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void FindLocalPLayer()
        {
            if (!isLocalPlayer && !localCharacterVitals)
            {
                localCharacterVitals = FindObjectsOfType<PlayerCharacterVitals>().Where(i => i.isLocalPlayer && i.IsAlive).FirstOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            FindLocalPLayer();

            if (IsReady && isClient && !isLocalPlayer)
            //if (IsReady)
            {
                //
                Vector3 myPos = transform.position + new Vector3(0f, 1.5f, 0f);

                //
                Vector3 targetPos = localCharacterVitals.transform.position + new Vector3(0f, 1.5f, 0f);

                //
                if (Physics.Raycast(myPos, targetPos - myPos, out RaycastHit hitInfo, Vector3.Distance(myPos, targetPos) + 1f))
                {
                    SetRendererActive(hitInfo.collider.CompareTag("Player"));
                }

                //
                if (localCharacterVitals && !localCharacterVitals.IsAlive)
                {
                    localCharacterVitals = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private void SetRendererActive(bool value)
        {
            if (renderers != null)
            {
                foreach (var r in renderers)
                {
                    if (r.enabled != value)
                        r.enabled = value;
                }
            }
        }
    }
}