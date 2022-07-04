using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class MobSpawn
{
    public GameObject mobPrefab;
    public float spawnRarity;
}

public class MobSpawner : MonoBehaviour
{
    [SerializeField]
    private List<MobSpawn> mobSpawns = new List<MobSpawn>();
    [SerializeField]
    public bool spawnerEnabled = true;
    [SerializeField]
    private float spawnTimeDelay = 1;
    [SerializeField]
    public GameObject playerObject;
    [SerializeField]
    private float spawnDisableDistance = 5;
    [SerializeField]
    private float height = 20;


    private float timePassed = 0;


    void Update()
    {
        //Debug.Log(Vector3.Distance(playerObject.transform.position, this.gameObject.transform.position));
        if (playerObject != null)
        {
            if (Vector3.Distance(playerObject.transform.position, this.gameObject.transform.position) <= spawnDisableDistance)
            {
                return;
            }
        }

        timePassed += Time.deltaTime;
        if(timePassed > spawnTimeDelay)
        {
            SpawnRandomMob();
            timePassed = 0;
        }
    }

    public void SpawnRandomMob()
    {
        float randomFloat = UnityEngine.Random.Range(0f, 1f);
        MobSpawn mobSpawn = mobSpawns.Where(m => randomFloat <= m.spawnRarity).Reverse().First();
        if(mobSpawn != null)
        {
            SpawnMob(mobSpawn.mobPrefab);
        }

    }

    public void SpawnMob(GameObject mobPrefab)
    {

        GameObject mob = GameObject.Instantiate(mobPrefab);
        if(mob.tag == "RangedEnemy")
        {
            mob.transform.position = this.transform.position + new Vector3(0, height, 0);
            return;
        }
        mob.transform.position = this.transform.position;
    }
}
