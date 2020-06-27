using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeadScreenController : MonoBehaviour 
{
    [SerializeField] private Text killZombearText;
    [SerializeField] private Text killZombunnyText;
    [SerializeField] private Text aliveTimeText;
    [SerializeField] private AudioSource bgmSource;

    public static int killZombearCount = 0;
    public static int killZombunnyCount = 0;
	
    public void Show()
    {
        bgmSource.Stop();
        gameObject.SetActive(true);
        killZombearText.text = killZombearText.text + killZombearCount.ToString() + "마리";
        killZombunnyText.text = killZombunnyText.text + killZombunnyCount.ToString() + "마리";
        float aliveTime = PlayerController.aliveTimer;
        int aliveHour = (int)Math.Truncate(aliveTime / 3600);
        aliveTime %= 3600;
        int aliveMin = (int)Math.Truncate(aliveTime / 60);
        aliveTime %= 60;
        int aliveSec = (int)Math.Truncate(aliveTime);
        aliveTime *= 100;
        int aliveTinySec = (int)Math.Truncate(aliveTime % 100);
        aliveTimeText.text = string.Format("{0}{1:00}:{2:00}:{3:00}.{4:00}", aliveTimeText.text, aliveHour, aliveMin, aliveSec, aliveTinySec);
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        PlayerController.aliveTimer = 0f;
        killZombearCount = killZombunnyCount = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
