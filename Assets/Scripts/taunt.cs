using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class taunt : MonoBehaviour
{
    public Animator anim;
    public string[] anims;

    void Update()
    {
        //use left/right/up keys to taunt
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("DpadUpDown") > 0){
            anim.Play(anims[0]);
        }
            
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("DpadSide") > 0){
            anim.Play(anims[1]);
        }
            
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("DpadSide") < 0){
            anim.Play(anims[2]);
        }
            
    }
}
