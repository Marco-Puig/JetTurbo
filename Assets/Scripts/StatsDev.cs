using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsDev : MonoBehaviour
{
    //public TMP_Text ping;
    public TMP_Text fps;
    float ping_ms;

    void Update()
    {
        fps.SetText("FPS: " + (1/Time.deltaTime));
        // float ping_ms = LocalTime - ServerTime;
        // ping.SetText("Ping: " + ping_ms + "ms");
    }
}
