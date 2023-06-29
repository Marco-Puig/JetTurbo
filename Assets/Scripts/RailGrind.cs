using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGrind : MonoBehaviour
{
    public Transform startPoint; // Start point of the rail
    public Transform endPoint; // End point of the rail
    public float grindSpeed = 5f; // Speed of the grind

    public Hoverboard hbs; //player
    public BoardAssist ba;
    public airTime at;

    private bool isGrinding = false;
    private float t = 0f; // Lerp parameter

    public GameObject scorePopUp;
    GameObject clonePopUp;
    Animation cloneScoreAnim;

    private void OnTriggerEnter(Collider other)
    {
        //assign to local player
        hbs = other.gameObject.GetComponentInParent<Hoverboard>();
        ba = other.gameObject.GetComponentInParent<BoardAssist>();
        at = other.gameObject.GetComponentInChildren<airTime>();

        if (other.CompareTag("Player"))
        {
            isGrinding = true;
        }
    }

    private void FixedUpdate()
    {
        if (isGrinding)
        {
            // Perform the grind action
            t += grindSpeed * Time.deltaTime;
            hbs.hb.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, t);
            hbs.hb.transform.rotation = Quaternion.Lerp(startPoint.rotation, endPoint.rotation, t);
            ba.constrainRotationZ = true;
            hbs.hovering = false;

            // Check if the grind is complete
            if (t >= 1f)
            {
                isGrinding = false;
                hbs.hovering = true;
                t = 0f;
                at.coroutineQueue.Enqueue(at.boost());

                //push player foward and up
                hbs.hb.AddForce(20 * hbs.moveForce * hbs.transform.forward);
                hbs.hb.AddForce(-5, 10, 0);
                ba.rotationZSpeed = 6f;

                //scoring
                at.score_count += 150;
                clonePopUp = Instantiate(scorePopUp, new Vector3(86.6f, 200f, 0), Quaternion.identity);
                clonePopUp.transform.SetParent(at.UI.transform);
                cloneScoreAnim = clonePopUp.GetComponent<Animation>();
                cloneScoreAnim.Play("score");
                StartCoroutine(at.clean());
            }
            
            ba.rotationZSpeed = 3f;
        }
    }
}