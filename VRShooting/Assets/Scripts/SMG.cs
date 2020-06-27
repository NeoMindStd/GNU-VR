using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMG : Item
{
    public void OnGetItem(PlayerController playerController)
    {
        base.OnGetItem(playerController);
        playerController.theGunController.GetAmmo(Random.Range(20,50));
    }
}
