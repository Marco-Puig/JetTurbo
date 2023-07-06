using UnityEngine;
using System.Collections;

public class PropMovementScript : MonoBehaviour {

    public float TrahsHold;

    public bool isMoving = false;
    public Vector3 targetPosition;
    public float speed;
   

    public bool isRotating = false;
    public Vector3 targetRotation;
    public float speedRotation;


    private Vector3 startPosition;
    private Vector3 startRotation;

	// Use this for initialization
	void Start () {
        startPosition = gameObject.transform.position;
        startRotation = gameObject.transform.localEulerAngles;
        if (TrahsHold < 0.15f)
        {
            TrahsHold = 0.15f;
        }
       
	}
	
	// Update is called once per frame
	void Update () {

        if (isMoving)
        {
            Vector3 objPos = Vector3.MoveTowards(gameObject.transform.position, targetPosition, speed * Time.deltaTime);

            if ((Mathf.Abs(targetPosition.x - objPos.x) < TrahsHold) &&
                (Mathf.Abs(targetPosition.y - objPos.y) < TrahsHold) &&
                (Mathf.Abs(targetPosition.z - objPos.z) < TrahsHold))
            {
                transform.position = startPosition;
            }
            else
            {
                transform.position = objPos;
            }
        }

        if (isRotating)
        {
            Vector3 objRotation = Vector3.RotateTowards(gameObject.transform.localEulerAngles, targetRotation, speedRotation * Time.deltaTime, 1);

            if ((Mathf.Abs(targetRotation.x - objRotation.x) < TrahsHold) &&
               (Mathf.Abs(targetRotation.y - objRotation.y) < TrahsHold) &&
               (Mathf.Abs(targetRotation.z - objRotation.z) < TrahsHold))
            {
                transform.localEulerAngles = startRotation;
            }
            else
            {
                transform.localEulerAngles = objRotation;
            }
        }
	}
}
