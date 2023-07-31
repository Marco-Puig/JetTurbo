using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class HoverboardOffline : MonoBehaviour
{
    public Rigidbody hb;
    public airTimeOffline at;
    public bool InAir;
    public float mult;
    public float moveForce;
    public float moveForceDefault;
    public float turnTorque;
    public float turnTorqueDefault;
    public float boostForce;
    public float boostTorque;
    public Camera mainCam;
    public TrailRenderer boostEffect;
    public ParticleSystem speedEffect;
    public bool boostPad = false;
    public TMP_Text speed_count;
    public float speed;
    int drift_countR = 0;
    int drift_countL = 0;
    public bool driftBoost = false;
    public bool hovering = true;
    public TMP_Text driftReady;
    public TMP_Text driftPurple;
    public TMP_Text boostReady;
    bool r = false; //right drift
    bool l = false; //left drift
    bool driftCoolDown = false;
    private float initialMouseX;
    private Quaternion initialRotation;
    bool isPaused = false;
    
    void Start()
    {
        initialMouseX = Input.mousePosition.x;
        initialRotation = hb.transform.rotation;
        ResetMousePosition();

        Input.ResetInputAxes();
    }

    public Transform[] anchors = new Transform[4];
    public RaycastHit[] hits = new RaycastHit[4];

    void Update()
    {
        //keep track of speed and boostings
        speed = hb.velocity.magnitude; 
        double speed_for_text = System.Math.Round(speed, 0); 
        speed_count.SetText((speed_for_text * 3).ToString());

        if (!(Input.GetKey(KeyCode.Mouse1) || Input.GetAxis("RT") > 0))
            boostPad = false;
        
        if (boostPad) boostReady.enabled = false; else boostReady.enabled = true;
        
        //check if player is paused
        if (Input.GetKeyDown(KeyCode.Escape))
        { if (isPaused) isPaused = false; else isPaused = true;}
    }

    void FixedUpdate()
    {
        for (int i = 0; i < 4; i++)
            ApplyF(anchors[i], hits[i]);

        if (InAir == false){
            ApplyMovement(boostForce, boostTorque);
            Drifting(); 
        }
        else{
            //still be able to look around when in the air
            if (!isPaused){
                hb.AddTorque(Input.GetAxis("Mouse X") * turnTorque * hb.transform.up * PlayerPrefs.GetFloat("Mouse Sensitivity"));
                hb.AddTorque(Input.GetAxis("Joystick X") * turnTorque * hb.transform.up * PlayerPrefs.GetFloat("Mouse Sensitivity"));                
            }
        }

    }

    void ApplyF(Transform anchor, RaycastHit hit)
    {
        if (Physics.Raycast(anchor.position, -anchor.up, out hit) && InAir == false /*to prevent a rocket*/ && hovering)
        {
            float force = 0;
            force = Mathf.Abs(1 / (hit.point.y - anchor.position.y));
            hb.AddForceAtPosition(transform.up * force * mult, anchor.position, ForceMode.Acceleration);
        }
        
    }


    // will need a lot of tweaking
    void ApplyMovement(float amountForce, float amountTorque)
    {   
        if (Input.GetKey(KeyCode.Mouse1) || Input.GetAxis("RT") > 0)
        {
            //boosting
            if (Input.GetKey(KeyCode.Mouse1))
                hb.AddForce(Input.GetAxis("Fire2") * moveForce * transform.forward);
            else if (Input.GetAxis("RT") > 0)
                hb.AddForce(Input.GetAxis("RT") * moveForce * transform.forward * 1.263f);
            //if not drifting 
            if (!(l == true || r == true))
                hb.AddForce(Input.GetAxis("Horizontal") * moveForceDefault * transform.right); 
            //if not drifting (also might have remove this eventually):
            if (!(l == true || r == true))
                hb.AddTorque(Input.GetAxis("Horizontal") * 100 * transform.up);  
            Ramping(true, amountForce, moveForceDefault);
            turnTorque = amountTorque;
            ApplyCam(true);
        }
        else
            {
                //regular movement
                hb.AddForce(Input.GetAxis("Vertical") * moveForce * transform.forward);
                //if not drifting 
                if (!(l == true || r == true)) 
                    hb.AddForce(Input.GetAxis("Horizontal") * moveForce * transform.right);   
                Ramping(false, amountForce, moveForceDefault);
                turnTorque = turnTorqueDefault;
                ApplyCam(false);
            }
        //for both boosting and regular
        //Torque will be handled by mouse movement via player.
        //if not drifting:
        if (!(l == true || r == true))
        {
            if (!isPaused){
                hb.AddTorque(Input.GetAxis("Mouse X") * turnTorque * transform.up * PlayerPrefs.GetFloat("Mouse Sensitivity"));
                hb.AddTorque(Input.GetAxis("Joystick X") * turnTorque * transform.up * PlayerPrefs.GetFloat("Mouse Sensitivity"));
            }
        }
    }

    void ApplyCam(bool toggled)
    {
        //better game feel using camera
        if (boostPad == false){
            if (toggled == true){
                mainCam.fieldOfView += 0.3f;
                //show blue boost trial, but dont overide boosting trail color or drifiting trail color
                if (boostPad == true || !(l == true || r == true)){
                    boostEffect.startColor = new Color(0.1439124f, 0.8442528f, 0.9245283f);
                    boostEffect.endColor = new Color(0f, 0f, 0f, 0f);
                }
            }
            else{
                mainCam.fieldOfView -= 0.9f;
                boostEffect.startColor = new Color(1f, 1f, 1f, 0.05f);
                boostEffect.endColor = new Color(0f, 0f, 0f, 0f);
            }
        }

        if (boostPad){
            StartCoroutine(dec());
            StartCoroutine(dec_speed());
            //temp (on boost) max mult
            if (mainCam.fieldOfView >= 110.0f)
            {
                mainCam.fieldOfView = 110.0f; 
            }
            //min mult
            if (mainCam.fieldOfView <= 60.0f)
            {
                mainCam.fieldOfView = 60.0f; 
            }
        }
        else
        {
            //max mult
            if (mainCam.fieldOfView >= 75.0f)
            {
                mainCam.fieldOfView = 75.0f; 
            }
            //min mult
            if (mainCam.fieldOfView <= 60.0f)
            {
                mainCam.fieldOfView = 60.0f; 
            }
        }


    }

    void Ramping(bool toggled, float max, float min)
    {
        if (toggled == true)
            moveForce += 80.0f;
        else
            moveForce -= 40.0f;
       
        //max mult
        if (moveForce >= max)
        {
            moveForce = max; 
        }
        //min mult
        if (moveForce <= min)
        {
            moveForce = min; 
        }

    }

    void Drifting(){
        //torque value
        float driftTorque = Mathf.Lerp(0, 150, 20f);
        //right
        if ((Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D) || Input.GetButton("RB")) && (Input.GetKey(KeyCode.Mouse1) || Input.GetAxis("RT") > 0) && boostPad == false && (!(l == true || r == true)) && driftCoolDown == false)
        {
            r = true;
        } 
        if (r){
            drift_countR += 1; 
            hb.AddTorque(0, driftTorque, 0); 
            //once 3 secs past, then boost(), and change trail to color to show player can boost.
            if (drift_countR >= 80){
                at.coroutineQueue.Enqueue(at.boost());
                driftBoost = true;
                InAir = false;
            }
            else
                boostEffect.startColor = Color.magenta;
                InAir = true;
        }
        else{
            drift_countR = 0; 
        }

        //left
        if ((Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A) || Input.GetButton("LB")) && (Input.GetKey(KeyCode.Mouse1) || Input.GetAxis("RT") > 0) && boostPad == false && (!(l == true || r == true)) && driftCoolDown == false)
        {
            l = true;
        } 
        if (l){
            drift_countL += 1; 
            hb.AddTorque(0, -driftTorque, 0); 
            //once 3 secs past, then boost(), and change trail to color to show player can boost.
            if (drift_countL >= 80){
                at.coroutineQueue.Enqueue(at.boost());
                driftBoost = true;
            }
            else
                boostEffect.startColor = Color.magenta;
        }
        else{
            drift_countL = 0; 
        }

        //cancel and check if drift is ready
        if (!(Input.GetKey(KeyCode.Mouse1) || Input.GetAxis("RT") > 0)){
            StartCoroutine((driftWait()));
        }
        if (boostPad == false || driftCoolDown == false){
            //driftReady.SetText("Drift: Ready");
            driftPurple.enabled = true;
        }
        if (boostPad || driftCoolDown){
            driftPurple.enabled = false;
            //driftReady.SetText("Drift: Not Ready");
        }
    }
    public IEnumerator driftWait()
    {
        l = false;
        r = false;
        driftCoolDown = true;
        yield return new WaitForSeconds(2f);
        driftCoolDown = false;
    }

    public IEnumerator dec(){
        mainCam.fieldOfView += 0.3f;
        speedEffect.Stop();
        speedEffect.Play();
        l = false;
        r = false;            
        yield return new WaitForSeconds(1);
        if (mainCam.fieldOfView >= 75.0f){
            mainCam.fieldOfView -= 0.6f;
        }
    }

    public IEnumerator dec_speed(){
        if ((at.trickBoost || driftBoost)){
            if (Input.GetKey(KeyCode.Mouse1) || Input.GetAxis("RT") > 0){
                boostReady.enabled = false;
                if (Input.GetKey(KeyCode.Mouse1))
                    hb.AddForce(Input.GetAxis("Fire2") * 5000 * transform.forward);
                else if (Input.GetAxis("RT") > 0)
                    hb.AddForce(Input.GetAxis("RT") * 5000 * transform.forward * 1.263f);                
            }

            else{
                speedEffect.Stop();
                at.trickBoost = false;
                driftBoost = false;
                boostReady.enabled = true;
            }
        }
        else{
            //dont force foward boost player when player is already boosting
            if (Input.GetKey(KeyCode.Mouse1) || Input.GetAxis("RT") > 0){
                if (Input.GetKey(KeyCode.Mouse1))
                    hb.AddForce(Input.GetAxis("Fire2") * 5000 * transform.forward);
                else if (Input.GetAxis("RT") > 0)
                    hb.AddForce(Input.GetAxis("RT") * 5000 * transform.forward * 1.263f);  
            }
        }
        boostEffect.startColor = new Color(1f, 0.6f, 0.1f);
        boostEffect.endColor = new Color(0f, 0f, 0f, 0f);
        yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(2);
        if ((Input.GetKey(KeyCode.Mouse1) || Input.GetAxis("RT") > 0)  && l == false && r == true && boostEffect.startColor != Color.magenta)
            boostEffect.startColor = new Color(0.1439124f, 0.8442528f, 0.9245283f);
       // boostPad = false;
    }

    private void ResetMousePosition()
    {
        float currentMouseX = Input.mousePosition.x;
        float deltaX = currentMouseX - initialMouseX;
        Quaternion rotationOffset = Quaternion.Euler(0f, deltaX, 0f);
        transform.rotation = initialRotation * rotationOffset;
    }
    
}
