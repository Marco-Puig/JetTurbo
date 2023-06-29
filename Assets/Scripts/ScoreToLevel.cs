using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreToLevel : MonoBehaviour
{
    public Text displayPlayerLevel;

    void Update()
    {
       displayPlayerLevel.text = PlayerPrefs.GetFloat("Level").ToString();
    }
}
