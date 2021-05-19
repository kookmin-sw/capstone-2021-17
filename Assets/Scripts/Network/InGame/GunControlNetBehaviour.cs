using UnityEditor;
using UnityEngine;
using Mirror;
using System.Collections;

public class GunControlNetBehaviour : NetworkBehaviour
{
    [SerializeField]
    GunControl gunControl;

    public GameObject BulletPrefab;

    

    public void SpawnBullet()
    {

        CmdSpawnBullet(gunControl.BulletPos.position, gunControl.BulletPos.rotation);
    }
    [Command]
    public void CmdSpawnBullet(Vector3 position, Quaternion rotation)
    {
        GameObject Bullet = Instantiate(BulletPrefab, gunControl.BulletPos.position, gunControl.BulletPos.rotation);
        
        Rigidbody BulletRigid = Bullet.GetComponent<Rigidbody>(); //Rigidbody creation
        Vector3 forward = gunControl.BulletPos.forward;

        BulletRigid.velocity = new Vector3(forward.x,0,forward.z) * 15;
        NetworkServer.Spawn(Bullet);

        StartCoroutine(Destroy(Bullet, 2.0f));

    }

    public IEnumerator Destroy(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);

        NetworkServer.UnSpawn(go);
    }
}