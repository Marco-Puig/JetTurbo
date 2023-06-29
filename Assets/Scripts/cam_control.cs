using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam_control : MonoBehaviour
{
    public GameObject[] cams; //cams[0] = front_cam
    private int count;
    public bool toggled;

    void Update()
    {
        if(Input.GetKey(KeyCode.E) || Input.GetButton("X"))
        {
            if (toggled == false)
            {
                for(count = 1; count < cams.Length; count++)
                {
                    cams[0].SetActive(true); 
                    cams[count].SetActive(false);      
                }
                toggled = true;
            }
        }
        else
        {
            for(count = 1; count < cams.Length; count++)
            {
                cams[0].SetActive(false); 
                cams[count].SetActive(true);      
            }
            toggled = false;
        }
    }
}