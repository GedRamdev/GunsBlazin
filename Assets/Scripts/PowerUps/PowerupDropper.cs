using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupDropper : MonoBehaviour
{
    [SerializeField]
    private List<PowerupObject> powerupObjects = new List<PowerupObject>();

    public void DropRandomPowerup(Vector3 location)
    {
        System.Random random = new System.Random();
        int r = random.Next(powerupObjects.Count);
        Drop(powerupObjects[r], location);
    }

    public void Drop(PowerupObject powerupObject, Vector3 location)
    {
        GameObject powerupCopy = GameObject.Instantiate(powerupObject.powerupModel);
        powerupCopy.transform.position = location;
    }
}
