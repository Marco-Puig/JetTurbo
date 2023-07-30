using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public GameObject girl;
    public GameObject boy;
    
    void Update()  
    {
        if (PlayerPrefs.GetInt("Character") == 0)
        {
            girl.SetActive(false);
            boy.SetActive(true);
        }
        else
        {
            girl.SetActive(true);
            boy.SetActive(false);
        }
    }
}
