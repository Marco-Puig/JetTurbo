using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEndScreen : MonoBehaviour
{
    //public EndGameManager manager;
    GameObject parent;

    void OnTriggerEnter(Collider col)
    {
        parent = col.GetComponent<Collider>().transform.parent.parent.parent.gameObject;
        col.GetComponent<EndGameManager>().ready = true;
        parent.GetComponent<Hoverboard>().PlayerModel.SetActive(false);
    }
}
