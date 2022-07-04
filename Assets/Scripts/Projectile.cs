using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 40f;

    private Vector3 direction;

    [SerializeField]
    private GameObject damageTextObject;
    
    private PlayerData playerData;
    private AudioSource audio;
    public AudioClip decayEffect;
    private GameObject player;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerData = GetComponent<PlayerData>();
        transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z);
        Vector3 direction = new Vector3(transform.parent.right.x, transform.parent.right.y, transform.parent.right.z);
        transform.parent = null;
        this.GetComponent<Rigidbody>().velocity = direction * speed;
        Destroy(this.gameObject, 5f);
    }
    void OnTriggerEnter(Collider other){
        switch (other.gameObject.tag)
        {
            case "Enemy":
                EnemyMelee enemyMelee = other.gameObject.GetComponent<EnemyMelee>();
                if (!enemyMelee.IsDead())
                {
                    enemyMelee.TakeDamage(damage);
                    GameObject textObject = GameObject.Instantiate(damageTextObject);
                    textObject.transform.position = this.transform.position;
                    Quaternion rotation = Quaternion.Inverse(player.transform.rotation);
                    rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, -rotation.eulerAngles.z);
                    textObject.transform.rotation = player.transform.rotation;
                    Debug.Log(textObject.transform.position);
                    //textObject.transform.position = Vector3.zero;
                    textObject.GetComponent<DamageText>().SetText(((int)damage).ToString());
                }
                break;
            case "RangedEnemy":
                AirbornFollowPlayer airbornEnemy = other.gameObject.GetComponent<AirbornFollowPlayer>();
                if (!airbornEnemy.IsDead())
                {
                    airbornEnemy.TakeDamage(damage);
                    GameObject textObject = GameObject.Instantiate(damageTextObject);
                    textObject.transform.position = this.transform.position;
                    textObject.transform.rotation = Quaternion.Euler(-player.transform.rotation.eulerAngles);
                    textObject.GetComponent<DamageText>().SetText(((int)damage).ToString());
                }
                break;
        }
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            return;
        }
        Destroy(this.gameObject);
    }
}

