using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverboardMenu : MonoBehaviour
{
    public Rigidbody hb;
    public float mult;
    public float moveForce;
    public float moveForceDefault;
    public float turnTorque;
    public float turnTorqueDefault;
    public float boostForce;
    public float boostTorque;
    
    void Start()
    {
        hb = GetComponent<Rigidbody>();
    }

    public Transform[] anchors = new Transform[4];
    public RaycastHit[] hits = new RaycastHit[4];

    void FixedUpdate()
    {
        for (int i = 0; i < 4; i++)
            ApplyF(anchors[i], hits[i]);

    }

    void ApplyF(Transform anchor, RaycastHit hit)
    {
        if (Physics.Raycast(anchor.position, -anchor.up, out hit))
        {
            float force = 0;
            force = Mathf.Abs(1 / (hit.point.y - anchor.position.y));
            hb.AddForceAtPosition(transform.up * force * mult, anchor.position, ForceMode.Acceleration);
        }
    }
    
}