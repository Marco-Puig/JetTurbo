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
    public double b;

    void Update() 
    {  
        if (!em.ready)
        {
            cnt += Time.deltaTime;         
            b = System.Math.Round(cnt, 2);     
            disvar.text = (b.ToString());
        }         
    }
}
