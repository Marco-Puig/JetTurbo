using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEndScreenOffline : MonoBehaviour
{
    public EndGameManagerOffline manager;

    void OnTriggerEnter(Collider col)
    {
        Destroy(col.transform.parent.gameObject);
        manager.ready = true;
    }
}
