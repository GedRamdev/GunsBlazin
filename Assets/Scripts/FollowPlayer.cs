using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    private NavMeshAgent enemy;
    private Transform player;
    public EnemyMelee managerScript;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GetComponent<NavMeshAgent>();
        enemy.enabled = false;
        enemy.enabled = true;
        managerScript = gameObject.GetComponent<EnemyMelee>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!managerScript.getDead())
        {
            enemy.SetDestination(player.position);
        }
    }
}
