using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupTester : MonoBehaviour
{
    [SerializeField]
    private PowerupDropper powerupDropper;

    private void Start()
    {
        powerupDropper.DropRandomPowerup(new Vector3(10, 12, -70));
    }
}
