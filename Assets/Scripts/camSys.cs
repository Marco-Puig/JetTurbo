using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camSys : MonoBehaviour
{
    public GameObject player;
    public GameObject child;
    public float speed = 8;

    private void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        if (Input.GetKey(KeyCode.Mouse1))
            speed = 15;
        else
            speed = 8;

        gameObject.transform.position = Vector3.Lerp(transform.position, child.transform.position, Time.deltaTime * speed);
        gameObject.transform.LookAt(player.gameObject.transform.position);
        
    }
}
