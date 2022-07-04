using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;

    void Update()
    {
        this.GetComponent<Image>().fillAmount = playerData.health / playerData.maxHealth;    
    }
}
