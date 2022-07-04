using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private PowerupTextManager powerupTextManager;

    public void GivePowerup(Powerup powerup)
    {
        this.playerData.powerups.Add(powerup);
        ApplyPowerup(powerup);
    }

    public void ApplyPowerup(Powerup powerup)
    {
        switch (powerup.powerupType)
        {
            case PowerupType.MOVEMENT_SPEED:
                playerController.speed *= 1.05f;
                break;
            case PowerupType.ATTACK_SPEED:
                this.playerData.attackSpeed *= 1.2f;
                break;
            case PowerupType.DAMAGE:
                this.playerData.attackDamage += 20.0f;
                break;
            case PowerupType.JUMP_HEIGHT:
                playerController.jumpHeight += 2;
                break;
            case PowerupType.MAX_HEALTH:
                this.playerData.maxHealth *= 1.3f;
                break;
            case PowerupType.HEALTH_REGEN:
                this.playerData.regenAmount *= 1.3f;
                break;
            case PowerupType.REGEN_SPEED:
                this.playerData.regenRate *= 1.2f;
                break;
            case PowerupType.INSTANT_HEALTH:
                this.playerData.health = this.playerData.health * 1.2f > this.playerData.maxHealth ? this.playerData.maxHealth : this.playerData.health * 1.2f;
                break;
            case PowerupType.MULTI_JUMP:
                playerController.jumpCountMax++;
                break;
        }
        playerData.powerups.Add(powerup);
        ShowPowerupText(powerup);
    }

    private void ShowPowerupText(Powerup powerup)
    {
        powerupTextManager.ShowPowerup(powerup);
    }
}
