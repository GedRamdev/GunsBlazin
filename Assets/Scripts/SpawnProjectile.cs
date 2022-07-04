using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : MonoBehaviour
{
    public GameObject prefab;
    private float spawnTime = 0f;
    public GameObject aligner;

    private PlayerData playerData;

    private System.Random random = new System.Random();

    private AudioSource audio;
    public AudioClip shootEffect;
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;
        if(Input.GetMouseButton(0))
        {       
            if (spawnTime >= 1/playerData.GetPlayerAttackSpeed())
            {
                GameObject projectile = (GameObject) Instantiate(prefab, new Vector3(aligner.transform.position.x, aligner.transform.position.y, aligner.transform.position.z), transform.rotation);

                Projectile projectileComponent = projectile.GetComponent<Projectile>();
                projectileComponent.damage = random.Next((int) playerData.GetPlayerDamage() - 10, (int) playerData.GetPlayerDamage() + 10);
                projectile.transform.parent = this.transform;

                audio.PlayOneShot(shootEffect);
                spawnTime = 0.0f;
            }
        }
    }
}
