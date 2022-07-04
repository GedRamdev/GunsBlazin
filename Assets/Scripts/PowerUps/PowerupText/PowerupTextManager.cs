using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerupTextManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text powerupTitleObject;
    [SerializeField]
    private TMP_Text powerupDescriptionObject;

    [SerializeField]
    private float powerupTextDuration = 2.0f;

    private float time = 0.0f;

    private bool powerupTextActive = false;
    
    public void ShowPowerup(Powerup powerup)
    {
        time = 0.0f;
        powerupTitleObject.text = powerup.powerupName;
        powerupDescriptionObject.text = powerup.powerupDescription;
        powerupTitleObject.gameObject.SetActive(true);
        powerupDescriptionObject.gameObject.SetActive(true);
        powerupTextActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!powerupTextActive)
        {
            return;
        }

        time += Time.deltaTime;
        if(time >= powerupTextDuration)
        {
            powerupTitleObject.gameObject.SetActive(false);
            powerupDescriptionObject.gameObject.SetActive(false);
            powerupTextActive = false;
        }
    }
}
