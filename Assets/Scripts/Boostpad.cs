using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boostpad : MonoBehaviour
{
    private bool istriggered = false; 
 
    void OnTriggerEnter(Collider coll)
    {

        if (istriggered == false){
            if (SceneManager.GetActiveScene().name == "Dev_Tst_Offline" || SceneManager.GetActiveScene().name == "Map1_Offline")
            {
                coll.gameObject.GetComponent<airTimeOffline>().coroutineQueue.Enqueue(
                    coll.gameObject.GetComponent<airTimeOffline>().boost());
            }
            else
            {
                 coll.gameObject.GetComponent<airTime>().coroutineQueue.Enqueue(
                    coll.gameObject.GetComponent<airTime>().boost());               
            }

            istriggered = true;
        }

    }
 
    void OnTriggerExit(Collider coll)
    {
        istriggered  = false;
    }

}
