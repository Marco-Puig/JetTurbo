using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownStart : MonoBehaviour
{
    
    //public AudioSource audio;
    public GameObject[] stagePieces;
    public TMP_Text countdownText;
    float currentTime;

    void Update()
    {
        currentTime += Time.deltaTime;
        countdownText.SetText(((5f-currentTime)+1f).ToString());

        if (currentTime > 2)
        {
            //play audio 
            //audio.Play();

            if (currentTime > 5)
            {
                for (int i = 0; i < stagePieces.Length; i++){
                    stagePieces[i].SetActive(false);                    
                }

            }
        }
    }
}
