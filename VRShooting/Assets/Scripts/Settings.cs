using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour 
{
    private void Start() 
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        MouseLock();
    }

    public static void MouseLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public static void MouseUnlock()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}