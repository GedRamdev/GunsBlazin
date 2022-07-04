using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField]
    public float maxHealth = 100.0f;
    [SerializeField]
    public float health = 100.0f;
    [SerializeField]
    public float attackSpeed = 1.0f;
    [SerializeField]
    public float attackDamage = 50.0f;
    [SerializeField]
    public float regenAmount = 10.0f;
    [SerializeField]
    public float regenRate = 0.1f;

    [SerializeField]
    public GameObject deathScreen;

    [SerializeField]
    public GameObject pauseScreen;

    private float regenTimer = 0.0f;
    private float time = 0f;
    public float damageReductionOverTimeRatio;

    [HideInInspector]
    public List<Powerup> powerups = new List<Powerup>();

    private void Start()
    {
        health = 50f;
    }

    public void Damage(float damage)
    {
        if(health - damage <= 0)
        {
            CharacterDieEvent();
            return;
        }
        this.health -= damage;
    }

    public void CharacterDieEvent()
    {
        Cursor.lockState = CursorLockMode.None;
        deathScreen.SetActive(true);
        Time.timeScale = 0;



        //Application.Quit();
    }


    private void Update()
    {
        time += Time.deltaTime;
        regenTimer += Time.deltaTime;
        if(regenTimer >= 1/regenRate)
        {
            regenTimer = 0;
            health = (health + regenAmount > maxHealth) ? maxHealth : (health + regenAmount);
        }
    }
    public float GetPlayerDamage(){
        return Mathf.Min(this.attackDamage - (time / damageReductionOverTimeRatio), 33f);
    }

    public float GetPlayerAttackSpeed()
    {
        return this.attackSpeed;
    }

}
