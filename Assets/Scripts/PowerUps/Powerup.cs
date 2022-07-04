using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Powerup
{
    public PowerupType powerupType;
    public string powerupName;
    public string powerupDescription;

    public Powerup(PowerupType powerupType, string powerupName, string powerupDescription)
    {
        this.powerupType = powerupType;
        this.powerupName = powerupName;
        this.powerupDescription = powerupDescription;
    }

}
