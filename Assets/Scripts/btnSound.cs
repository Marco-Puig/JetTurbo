using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnSound : MonoBehaviour
{
    public AudioSource sound;
    
    public void OnClick(){
        sound.Play();
    }
}
