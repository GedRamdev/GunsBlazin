using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{

    GameObject player;
    bool attacking = false;
    private Animator zombieAnimator;
    Vector3 prevPoss;
    Vector3 curPoss;
    public float enemyCurHealth = 0f;
    public float enemyMaxHealth = 100f;
    public float deleteMeAfterTime = 5.0f;
    public float distanceWhenIStartAttacking = 4f;
    public float enemyDamage = 5f;
    [SerializeField]
    private string walk;
    [SerializeField]
    private string attack;
    [SerializeField]
    private string die;
    private bool attackTimerStarted = false;
    private PlayerData playerData;
    private bool dead;
    [SerializeField]
    GameObject bloodSplatterVFX;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerData = player.GetComponent<PlayerData>();
        zombieAnimator = GetComponent<Animator>();
        enemyCurHealth = enemyMaxHealth;
    }

    public bool IsDead()
    {
        return dead;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            return;
        }

        if (Vector3.Distance(player.transform.position, transform.position) < distanceWhenIStartAttacking && !attacking)
        {
            attacking = true;
            Attack();
        }
        else
        {
            attacking = false;
        }

        zombieAnimator.SetBool(attack, attacking);

        if (curPoss == null)
        {
            curPoss = transform.position;
        }
        else
        {
            prevPoss = curPoss;
            curPoss = transform.position;
        }

        zombieAnimator.SetBool(walk, curPoss != prevPoss);

    }

    void Attack()
    {
        if(player == null)
        {
            return;
        }

        if(dead == true)
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

    private IEnumerator HitTimer()
    {
        yield return new WaitForSeconds(0.8f);
        if(Vector3.Distance(player.transform.position, transform.position) < distanceWhenIStartAttacking)
        {
            playerData.Damage(enemyDamage);
        }
        attackTimerStarted = false;
    }

    void OnDeathEvent()
    {
        dead = true;
        attackTimerStarted = true; // so mob doesn't fight while dying
        zombieAnimator.SetBool(walk, false);
        zombieAnimator.SetBool(die, true);

        if(UnityEngine.Random.Range(0.0f, 1.0f) <= 0.2f)
        {
            GameObject.FindGameObjectWithTag("PowerupDropper").GetComponent<PowerupDropper>().DropRandomPowerup(this.gameObject.transform.position + (Vector3.up * 1));
        }

        GameObject.Destroy(this.GetComponent<FollowPlayer>());
        GameObject.Destroy(this.gameObject, deleteMeAfterTime);
    }

    public void TakeDamage(float amount)
    {
        if (dead)
        {
            return;
        }

        var blood = GameObject.Instantiate(bloodSplatterVFX);
        blood.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        Destroy(blood, 1f);

        enemyCurHealth -= amount;
        if (enemyCurHealth <= 0)
        {
            OnDeathEvent();
        }
    }

    public bool getDead()
    {
        return dead;
    }
}
