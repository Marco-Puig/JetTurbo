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

}