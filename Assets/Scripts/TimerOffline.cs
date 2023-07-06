using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerOffline : MonoBehaviour
{
    public float cnt = -5.0f;
    public TMP_Text disvar;
    public EndGameManagerOffline em;
    public float b;

    void Update() 
    {  
        if (!em.ready)
        {
            cnt += Time.deltaTime;         
            b = cnt;

            int minutes = Mathf.FloorToInt(b / 60f);
            int seconds = Mathf.FloorToInt(b % 60f);
            int milliseconds = Mathf.FloorToInt((b * 100f) % 100f);

            if (cnt < 0)
                disvar.text = (Mathf.FloorToInt(1 - cnt)).ToString();
            else
                disvar.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);   
        }         
    }
}
