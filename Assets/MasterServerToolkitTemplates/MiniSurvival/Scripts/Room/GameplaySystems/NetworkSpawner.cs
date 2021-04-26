using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MasterServerToolkit.Template.MiniSurvival
{
    public class NetworkSpawner : NetworkBehaviour
    {
        #region INSPECTOR

        [Header("Settings"), SerializeField]
        private bool autoSpawn = true;
        [SerializeField, Range(1, 15)]
        private int maxInstances = 10;
        [SerializeField, Range(1f, 100f)]
        private int spawnRadius = 10;
        [SerializeField, Range(1f, 20f)]
        private int spawnHeight = 10;
        [SerializeField, Range(1f, 600f)]
        private int spawnInterval = 10;

        [Header("Components"), SerializeField]
        private GameObject objectPrefab;

        #endregion

        public List<GameObject> Instances { get; private set; }

        private bool spawnProcessIsRunning = false;

        private void Awake()
        {
            // Lets register our object in network
            NetworkClient.RegisterPrefab(objectPrefab);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, spawnRadius);
            UnityEditor.Handles.DrawWireDisc(transform.position + Vector3.up * spawnHeight, Vector3.up, 1f);
            UnityEditor.Handles.DrawLine(transform.position, transform.position + Vector3.up * spawnHeight);
            UnityEditor.Handles.DrawLine(transform.position + Vector3.up * spawnHeight, transform.position + Vector3.left * spawnRadius);
        }
#endif

        public override void OnStartServer()
        {
            if (Instances == null || Instances.Count <= 0)
                Instances = new List<GameObject>();

            if (autoSpawn)
            {
                Spawn();
                StartCoroutine(SpawnObjectsByInterval());
            }
        }

        private IEnumerator SpawnObjectsByInterval()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(spawnInterval);
                Spawn();
            }
        }

        private void RemoveDestroyed()
        {
            Instances = Instances.Where(i => i != null).ToList();
        }

        private IEnumerator StartSpawnProcess()
        {
            if (spawnProcessIsRunning) yield return null;

            RemoveDestroyed();

            if (Instances.Count < maxInstances && objectPrefab)
            {
                spawnProcessIsRunning = true;

                for (int i = Instances.Count; i < maxInstances; i++)
                {
                    yield return new WaitForSecondsRealtime(0.05f);

                    var newRadius = UnityEngine.Random.Range(0f, spawnRadius);
                    var newRadians = Mathf.Deg2Rad * UnityEngine.Random.Range(0f, 360f);
                    var newPosition = new Vector3(Mathf.Sin(newRadians) * newRadius, 0f, Mathf.Cos(newRadians) * newRadius) + transform.position;

                    if(Physics.Raycast(newPosition + Vector3.up * spawnHeight, Vector3.down, out RaycastHit hitInfo, spawnHeight))
                    {
                        newPosition = hitInfo.point;
                    }

                    var spawnedObject = Instantiate(objectPrefab, newPosition, Quaternion.identity);

                    Instances.Add(spawnedObject);
                    NetworkServer.Spawn(spawnedObject);
                }

                spawnProcessIsRunning = false;
            }
        }

        public void Spawn()
        {
            StartCoroutine(StartSpawnProcess());
        }
    }
}