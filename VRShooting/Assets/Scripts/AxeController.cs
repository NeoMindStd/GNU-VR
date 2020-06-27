using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : MeleeWeaponController
{
    // 활성화 여부.
    public static bool isActivate = false;

    // Update is called once per frame
    void Update()
    {
        if (isActivate) TryAttack();
    }

    public override void Change(MeleeWeapon meleeWeapon)
    {
        base.Change(meleeWeapon);
        isActivate = true;
    }
}
