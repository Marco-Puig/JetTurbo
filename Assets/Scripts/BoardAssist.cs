using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardAssist : MonoBehaviour
{
    public Rigidbody hb;
    
    Vector3 startPosition;
    Vector3 startRotation;

    public bool constrainRotationZ;
    public bool constrainRotationX;

    public float rotationZSpeed = 3f;
   
    void Start () 
    {
        startPosition = hb.transform.position;
        startRotation = hb.transform.eulerAngles;
    }

    void Update()
    { 
        if ((SceneManager.GetActiveScene().name == "Map1_Offline"))
            constrainRotationZ = true;
    }
   
    void FixedUpdate () {
       
        Vector3 currentPosition = hb.transform.position;
        Vector3 currentRotation = hb.transform.eulerAngles;
       
        if (constrainRotationZ)
        {
            if (currentRotation.z != 0)
            {
                currentRotation = new Vector3(currentRotation.x, currentRotation.y, startRotation.z);
                Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationZSpeed);
            }
        }

        if (currentRotation.z >= 50.0f)
        {
            constrainRotationZ = true;
        }

        if (constrainRotationX)
        {
            if (currentRotation.x != 0)
            {
               currentRotation = new Vector3(startPosition.x, currentRotation.y, currentRotation.z);
               Quaternion targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y, transform.eulerAngles.z);
               transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
            }
        }
    }
}