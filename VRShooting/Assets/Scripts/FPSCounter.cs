using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [Range(1, 100)] public int fFont_Size;
    [Range(0, 1)] public float Red, Green, Blue;

    private float deltaTime = 0.0f;
    private string text = "";
    private bool refreshFlag = true;

    private void Start()
    {
        fFont_Size = fFont_Size == 0 ? 50 : fFont_Size;
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / fFont_Size;
        style.normal.textColor = new Color(Red, Green, Blue, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        if(refreshFlag) {
            refreshFlag = false;
            text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            StartCoroutine(RefreshFlagSwitch());
        }
        GUI.Label(rect, text, style);
    }

    IEnumerator RefreshFlagSwitch() 
    {
        yield return new WaitForSeconds(1);
        refreshFlag = true;
    }
}