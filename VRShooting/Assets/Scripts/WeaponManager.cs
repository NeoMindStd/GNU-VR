using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {
    // 무기 중복 교체 실행 방지.
    public static bool isChangeWeapon = false;

    // 현재 무기와 현재 무기의 애니메이션.
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    // 현재 무기의 타입.
    [SerializeField] private string currentWeaponType;

    // 무기 교체 딜레이, 무기 교체가 완전히 끝난 시점.
    [SerializeField] private float changeWeaponDelayTime;
    [SerializeField] private float changeWeaponEndDelayTime;

    // 무기 종류들 전부 관리.
    [SerializeField] private Gun[] guns;
    [SerializeField] private MeleeWeapon[] meleeWeapons;

    // 관리 차원에서 쉽게 무기 접근이 가능하도록 만듦.
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, MeleeWeapon> meleeWeaponDictionary = new Dictionary<string, MeleeWeapon>();

    // 필요한 컴포넌트.
    [SerializeField] private GunController gunController;
    [SerializeField] private MeleeWeaponController[] meleeWeaponControllers;
    
    private Dictionary<string, MeleeWeaponController> mwControllers = new Dictionary<string, MeleeWeaponController>();
    [SerializeField] private HUD hud;

    public int currentSlotNum = 0;

    // Use this for initialization
    void Start () 
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < meleeWeapons.Length; i++)
        {
            meleeWeaponDictionary.Add(meleeWeapons[i].meleeWeaponName, meleeWeapons[i]);
            mwControllers.Add(meleeWeapons[i].meleeWeaponName, meleeWeaponControllers[i]);
        }
        StartCoroutine(ChangeWeaponCoroutine("GUN", guns[currentSlotNum].gunName));
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                string weaponType = "GUN";
                if(currentWeaponType == weaponType) currentSlotNum = ++currentSlotNum % guns.Length;
                else currentSlotNum = 0;
                StartCoroutine(ChangeWeaponCoroutine(weaponType, guns[currentSlotNum].gunName));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                string weaponType = "MELEE_WEAPON";
                if(currentWeaponType == weaponType) currentSlotNum = ++currentSlotNum % meleeWeapons.Length;
                else currentSlotNum = 0;
                StartCoroutine(ChangeWeaponCoroutine(weaponType, meleeWeapons[currentSlotNum].meleeWeaponName));
            }
        }
    }

    // 무기 교체 코루틴.
    public IEnumerator ChangeWeaponCoroutine(string type, string name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction();
        WeaponChange(type, name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = type;
        isChangeWeapon = false;
    }

    // 무기 취소 함수.
    private void CancelPreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                gunController.CancelFineSight();
                gunController.CancelReload();
                GunController.isActivate = false;
                break;
            case "MELEE_WEAPON":
                AxeController.isActivate = false;
                HandController.isActivate = false;
                PickaxeController.isActivate = false;
                break;
        }
    }

    // 무기 교체 함수.
    private void WeaponChange(string type, string name)
    {
        if(type == "GUN")
        {
            gunController.Change(gunDictionary[name]);
            hud.goBulletHUD.SetActive(true);
        }
        else if(type == "MELEE_WEAPON")
        {
            mwControllers[name].Change(meleeWeaponDictionary[name]);
            hud.goBulletHUD.SetActive(false);
        }
    }
}
