using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class airTimeOffline : MonoBehaviour
{
    public GameObject player;
    public BoardAssist boardAssist;
    public HoverboardOffline hbs;
    public bool InAir = false;
    public bool performedTrick = false;
    public Animator anim;
    public Animator anim1;
    public Animator anim2;
    public bool trickBoost;
    public string[] anims;
    public TMP_Text score;
    public GameObject scorePopUp;
    public GameObject UI;
    GameObject clonePopUp;
    Animation cloneScoreAnim;
    public float timer = 3f;
    public int score_count = 0;
    public Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();
    bool animPlayed = false;

    void Start()
    {
        StartCoroutine(CoroutineCoordinator());
    }

    void Update()
    {
        score.SetText("Score: " + score_count);

        if (!animPlayed && coroutineQueue.Count > 0 && Input.GetKey(KeyCode.Mouse1))
        {
            if (PlayerPrefs.GetInt("Character") == 0)
                anim.Play("touchboost");
            else
                anim1.Play("touchboost");
            animPlayed = true;
        }

        // when player is in the air
        if(IsGrounded() == false)
        {
            InAir = true;
        }   

        if (InAir){
            hbs.InAir = true;

            //keep player upright
            boardAssist.constrainRotationZ = true;
            boardAssist.constrainRotationX = true;

            //keep the momentium going! 
            hbs.hb.AddForce(Input.GetAxis("Fire2") * 240000 * hbs.transform.forward * Time.deltaTime); 
            hbs.hb.AddForce(Input.GetAxis("RT") * 240000 * hbs.transform.forward * Time.deltaTime);
            

            //hbs.boostEffect.enabled = true;

            //gravity adjustments for better feel
            if ((SceneManager.GetActiveScene().name == "Map1_Offline"))
                hbs.hb.AddForce(0,-900,0); 
            else
                hbs.hb.AddForce(0,-500,0); 

            if ((Input.GetKey(KeyCode.Mouse0) || Input.GetButton("A"))  && !(AnimatorIsPlaying())){
                // do trick
                int count = Random.Range(0, anims.Length-1);
                // flip is too bad to look at, so just leave it as both spin.
                anim2.Play(anims[count]);
                if (PlayerPrefs.GetInt("Character") == 0)
                    anim.Play(anims[count]);
                else
                    anim1.Play(anims[count]);
                // have different tricks be different score amounts
                score_count += 50;
                clonePopUp = Instantiate(scorePopUp, new Vector3(86.6f, 200f, 0), Quaternion.identity);
                clonePopUp.transform.SetParent(UI.transform);
                cloneScoreAnim = clonePopUp.GetComponent<Animation>();
                cloneScoreAnim.Play("score");
                StartCoroutine(clean());
                performedTrick = true;

                //updated score to level will need to adjust how much exp you get
                PlayerPrefs.SetFloat("Level", PlayerPrefs.GetFloat("Level") + (score_count / 1000));
            }
        }
        else{
            boardAssist.constrainRotationZ = false;
            boardAssist.constrainRotationX = false;
            hbs.InAir = false;
        }
    }

    bool AnimatorIsPlaying()
    {
        return anim.GetCurrentAnimatorStateInfo(0).length > anim.GetCurrentAnimatorStateInfo(0).normalizedTime || anim1.GetCurrentAnimatorStateInfo(0).length > anim1.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(hbs.hb.transform.position, -Vector3.up, 2.0f);
    }
    
    // when player lands
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            InAir = false;
            if (performedTrick){
                coroutineQueue.Enqueue(boost());
                performedTrick = false; 
                trickBoost = true;         
            }
        }              
    }

    // keep ensuring that player is grounded for both trigger stay and trigger exit.
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Ground")
        {
            InAir = false;
            if (performedTrick){
                coroutineQueue.Enqueue(boost());
                performedTrick = false; 
                trickBoost = true;         
            }
        }  
    }   

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Ground")
        {
            InAir = false;
            if (performedTrick){
                coroutineQueue.Enqueue(boost());
                performedTrick = false; 
                trickBoost = true;         
            }
        }  
    }              

    //trick boost
    public IEnumerator boost()
    {
        hbs.boostPad = true;
        yield return new WaitForSeconds(3);
        hbs.boostPad = false;
        animPlayed = false;
    }

    public IEnumerator clean()
    {
        yield return new WaitForSeconds(1);
        Destroy(clonePopUp);
    }
    
    public IEnumerator CoroutineCoordinator()
    {
     while (true)
     {        
        while (coroutineQueue.Count > 0)
            yield return StartCoroutine(coroutineQueue.Dequeue());            
        yield return null;
     }
 }

}