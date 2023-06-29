using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndGameManagerOffline : MonoBehaviour
{
    public bool ready;
    public GameObject playerUI;
    public GameObject winScreen;
    public TMP_Text finaltime;
    public TMP_Text finalscore;
    public TMP_Text highscore;
    public TimerOffline timer;
    public airTimeOffline airTime;
    public Animation fade;
    public bool faded = false;

    
    void Start()
    {
        ready = false;
        fade.Play("fade");
    }

    void Update()
    {
        if (ready)
        {
            //fade once
            if(!faded)
            {
                fade.Play("fade"); 
                faded = true;               
            }

            Cursor.visible = true;
            finaltime.SetText(timer.b.ToString());
            finalscore.SetText(airTime.score_count.ToString());
            winScreen.SetActive(true);
            Destroy(playerUI);

            //save and display highscore
            if ((float)timer.b < PlayerPrefs.GetFloat("HighScore") || PlayerPrefs.GetFloat("HighScore") == 0f){
                PlayerPrefs.SetFloat("HighScore", (float)timer.b);
                PlayerPrefs.Save();
            }

            highscore.SetText(PlayerPrefs.GetFloat("HighScore").ToString());
        }
    }
}
