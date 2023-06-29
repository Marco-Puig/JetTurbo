using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEndScreen : MonoBehaviour
{
    public EndGameManager manager;

    void OnTriggerEnter(Collider col)
    {
        Destroy(col);
        manager.ready = true;
    }
}
