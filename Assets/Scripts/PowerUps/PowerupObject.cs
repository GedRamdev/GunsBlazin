using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupObject : MonoBehaviour
{
    public Powerup powerup;
    [HideInInspector]
    public GameObject powerupModel;

    private void OnEnable()
    {
        powerupModel = this.gameObject;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag.Equals("Player"))
        {
            collider.gameObject.GetComponent<PowerupHandler>().ApplyPowerup(this.powerup);
        }
        GameObject.Destroy(this.gameObject);
    }
}
