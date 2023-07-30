using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDataManager : MonoBehaviour
{
    public TMPro.TMP_Dropdown dropDown;

    void Start()
    {
        dropDown.value = PlayerPrefs.GetInt("Character");    
    }

    void Update()
    {
        if (dropDown.value == 0)
        {
            PlayerPrefs.SetInt("Character", 0);     
        }
        else
        {
            PlayerPrefs.SetInt("Character", 1);
        }

    }
}
