using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallScript : MonoBehaviour
{

    public GameObject player;
    public Vector3 targetPos;
    public float speed = 10;
    public GameObject explosion;
    public float explosionRadious = 5f;
    private PlayerData playerData;
    private float explosionDamage = 20f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerData = player.GetComponent<PlayerData>();

        //transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z);
        //Vector3 direction = transform.LookAt(player.transform.position); ;
        
        transform.LookAt(player.transform.position);
        transform.parent = null;
        this.GetComponent<Rigidbody>().velocity = (player.transform.position - this.gameObject.transform.position).normalized * speed;
        //this.GetComponent<Rigidbody>().velocity = (player.transform.position - this.gameObject.transform.position) * speed;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag != "RangedEnemy")
        {
            Instantiate(explosion, transform.position, transform.rotation);

            if (Vector3.Distance(player.transform.position, transform.position) < explosionRadious)
            {
                playerData.Damage(explosionDamage);
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "RangedEnemy")
        {
            Instantiate(explosion, transform.position, transform.rotation);

            if (Vector3.Distance(player.transform.position, transform.position) < explosionRadious)
            {
                playerData.Damage(explosionDamage);
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "RangedEnemy")
        {
            Instantiate(explosion, transform.position, transform.rotation);

            if (Vector3.Distance(player.transform.position, transform.position) < explosionRadious)
            {
                playerData.Damage(explosionDamage);
            }

            Destroy(gameObject);
        }
    }

    private void OnColliderEnter(Collider collision)
    {
        if (collision.gameObject.tag != "RangedEnemy")
        {
            Instantiate(explosion, transform.position, transform.rotation);

            if (Vector3.Distance(player.transform.position, transform.position) < explosionRadious)
            {
                playerData.Damage(explosionDamage);
            }

            Destroy(gameObject);
        }
    }
}
