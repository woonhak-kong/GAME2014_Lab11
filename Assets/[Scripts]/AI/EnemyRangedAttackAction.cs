using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttackAction : MonoBehaviour, Action
{

    public bool hasLOS;
    [Range(1, 100)]
    public int fireDelay = 20;
    public Transform bulletSpawn;
    public Vector2 targetOffset;
    public GameObject bulletPrefabs;
    public Transform bulletParent;

    private void Awake()
    {
        bulletPrefabs = Resources.Load<GameObject>("Prefabs/Bullet");
    }

    // Update is called once per frame
    void Update()
    {
        hasLOS = transform.parent.GetComponentInChildren<PlayerDetection>().LOS;
        
    }

    private void FixedUpdate()
    {
        if ((hasLOS) && Time.frameCount % fireDelay == 0)
        {
            Excute();

        }
    }
    public void Excute()
    {
        var bullet = Instantiate(bulletPrefabs, bulletSpawn.position, Quaternion.identity, bulletParent);
        bullet.GetComponent<BulletController>().Activate();
        // toto: bullet sound
    }

}
