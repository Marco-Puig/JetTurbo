using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pause : MonoBehaviour
{
    public GameObject ps;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ps.activeInHierarchy == false){
                ps.SetActive(true);
                Cursor.visible = true;
                
            }
            else{
                ps.SetActive(false);
                Cursor.visible = false;
            }
        }

    }
}
