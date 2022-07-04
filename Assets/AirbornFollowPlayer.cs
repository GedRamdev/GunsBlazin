using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirbornFollowPlayer : MonoBehaviour
{

    public int speed = 5;
    public GameObject player;
    public float enemyCurHealth = 0f;
    public float enemyMaxHealth = 100f;
    public float deleteMeAfterTime = 5.0f;
    public float distanceWhenIStartAttacking = 60f;
    public float enemyDamage = 5f;
    public float maxHeight = 50f;
    public float minHeight = 30f;
    public Vector3 targetPos;
    private bool attackTimerStarted = false;
    public GameObject fireball;

    private bool dead = false;

    Rigidbody enemyRig;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyRig = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(player.transform.position, transform.position) < distanceWhenIStartAttacking)
        {
            enemyRig.constraints = RigidbodyConstraints.FreezePosition;
        }
        else
        {
            float targetHeight = Random.Range(minHeight, maxHeight);
            transform.LookAt(player.transform.position);
            Vector3 direction = player.transform.position - transform.position;

            targetPos.Set(player.transform.position.x, minHeight, player.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }

        Attack(); 
        
    }

    void Attack()
    {
        if (dead)
        {
            return;
        }
        if (player == null)
        {
            return;
        }
        if (!attackTimerStarted)
        {
            IEnumerator coroutine = HitTimer();
            StartCoroutine(coroutine);
            attackTimerStarted = true;
        }
    }

    public void TakeDamage(float amount)
    {
        if (dead)
        {
            return;
        }
        enemyCurHealth -= amount;
        if (enemyCurHealth <= 0)
        {
            OnDeathEvent();
        }
    }

    public bool IsDead()
    {
        return dead;
    }

    void OnDeathEvent()
    {
        dead = true;
        GameObject.Destroy(this.gameObject);
    }

    private IEnumerator HitTimer()
    {
        yield return new WaitForSeconds(10f);

        Instantiate(fireball, transform.position, transform.rotation);

        attackTimerStarted = false;
    }
}
