using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeaponController : MonoBehaviour {

    // 현재 장착된 Hand형 타입 무기.
    [SerializeField] protected MeleeWeapon currentMeleeWeapon;

    // 공격중??
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;

    protected void Start()
    {
        WeaponManager.currentWeapon = currentMeleeWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentMeleeWeapon.anim;
    }

    protected void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentMeleeWeapon.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentMeleeWeapon.attackDelayA);
        isSwing = true;

        yield return new WaitForSeconds(currentMeleeWeapon.attackDelayB);
        isSwing = false;

        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentMeleeWeapon.attackDelay - currentMeleeWeapon.attackDelayA - currentMeleeWeapon.attackDelayB);
        isAttack = false;
    }

    protected IEnumerator HitCoroutine()
    {
        if (CheckObject())
        {
            Debug.Log(hitInfo.transform.name);
            hitInfo.collider.SendMessage("OnHit", hitInfo.transform);
        }
        yield return null;
    }

    protected bool CheckObject()
    {
        return Physics.Raycast(transform.position, transform.forward, out hitInfo, currentMeleeWeapon.range);
    }

    public virtual void Change(MeleeWeapon meleeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentMeleeWeapon = meleeWeapon;
        WeaponManager.currentWeapon = currentMeleeWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentMeleeWeapon.anim;

        currentMeleeWeapon.transform.localPosition = Vector3.zero;
        currentMeleeWeapon.gameObject.SetActive(true);
    }
}
