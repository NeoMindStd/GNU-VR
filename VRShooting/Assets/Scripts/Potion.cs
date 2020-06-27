using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    public void OnGetItem(PlayerController playerController)
    {
        base.OnGetItem(playerController);
        playerController.Heal(Random.Range(20, 45));
    }
}
