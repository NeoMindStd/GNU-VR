using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour 
{
    // 필요한 컴포넌트
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GunController theGunController;
    private Gun currentGun;
    
    // 필요하면 HUD 호출, 필요 없으면 HUD 비활성화.
    [SerializeField] public GameObject goHpHUD;
    [SerializeField] public GameObject goBulletHUD;

    // Hp 관련 텍스트
    [SerializeField] private Text hpCount;

    // 총알 관련 텍스트
    [SerializeField] private Text currentBulletCount;
    [SerializeField] private Text carryBulletCount;
	
	// Update is called once per frame
	void Update () 
    {
        CheckHp();
        CheckBullet();
	}

    private void CheckHp()
    {
        int hp = playerController.GetHp();
        hpCount.text = hp.ToString();
        
        if(hp == 100) hpCount.color = Color.green;
        else if(hp >= 70) hpCount.color = Color.white;
        else if(hp <= 30) hpCount.color = Color.red;
        else hpCount.color = Color.yellow;
    }

    private void CheckBullet()
    {
        currentGun = theGunController.GetGun();
        currentBulletCount.text = currentGun.currentBulletCount.ToString();

        if(currentGun.currentBulletCount == currentGun.reloadBulletCount) currentBulletCount.color = Color.white;
        else if(currentGun.currentBulletCount == 0) currentBulletCount.color = Color.red;
        else currentBulletCount.color = Color.yellow;

        carryBulletCount.text = currentGun.carryBulletCount.ToString();
        
        if(currentGun.carryBulletCount >= currentGun.reloadBulletCount) carryBulletCount.color = Color.white;
        else if(currentGun.carryBulletCount == 0) carryBulletCount.color = Color.red;
        else carryBulletCount.color = Color.yellow;
    }
}
