using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Player : MonoBehaviour
{
    AudioSource audioSource; // audio source component for the ball hitting sound
    public Transform aimTarget; // the target where we aim to land the ball
    float speed = 5f; // move speed

    bool hitting; // boolean to know if we are hitting the ball or not 

    public Transform ball; 
    Animator animator;

    Vector3 aimTargetInitialPosition; // initial position of the aiming gameObject which is the center of the opposite court

    ShotManager shotManager; // reference to the shotmanager component
    Shot currentShot; // the current shot we are playing to acces its attributes

    private int scoreValue;
    private int highScoreValue;

    public static Player instance;

    void Start()
    {
        animator = GetComponent<Animator>(); // referennce out animator
        aimTargetInitialPosition = aimTarget.position; // initialise the aim position to the center( where we placed it in the editor )
        shotManager = GetComponent<ShotManager>(); // accesing our shot manager component
        currentShot = shotManager.topSpin; // defaulting our current shot as topspin
        //initializing the scoreboard elements
        scoreValue = 0;
        highScoreValue = 0;
        instance = this; // pointer to the player instance; used so other scripts can access the player
        audioSource = GetComponent<AudioSource>(); // reference to our audio source to access the ball hitting sound 
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal"); // get the horizontal axis of the keyboard
        float v = Input.GetAxisRaw("Vertical"); // get the vertical axis of the keyboard

        if (Input.GetKeyDown(KeyCode.F))
        {
            hitting = true; // we are trying to hit the ball and aim where to make it land
            currentShot = shotManager.topSpin; // set our current shot to top spin

        } else if (Input.GetKeyUp(KeyCode.F))
        {
            hitting = false; // we let go of the key so we are not hitting anymore and this
                             // is used to alternate between moving the aim target and ourself
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            hitting = true; // we are trying to hit the ball and aim where to make it land
            currentShot = shotManager.flat; // set our current shot to flat
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            hitting = false;
        }

        if (hitting) // if we are trying to hit the ball
        {
            aimTarget.Translate(new Vector3(h, 0, 0) * speed * 2 * Time.deltaTime); //translate the aiming gameObject on the court horizontallly
        }

        if ((h != 0 || v != 0) && !hitting) //if there's no ball hitting but we're still trying to move
        {
            transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball")) // if we collide with the ball 
        {
            Vector3 dir = aimTarget.position - transform.position; // get the direction to where we want to send the ball

            //add force to the ball plus some upward force according to the shot being played
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);

            Vector3 ballDir = ball.position - transform.position; // get the direction of the ball compared to us to know if it is on out right or left side 
            if (ballDir.x >= 0)
                animator.Play("forehand"); // play a forehand animation if the ball is on our right
            else
                animator.Play("backhand"); // otherwise play a backhand animation 

            aimTarget.position = aimTargetInitialPosition; // reset the position of the aiming gameObject to its original position (center)

            audioSource.PlayOneShot(audioSource.clip, 0.75f); // triggers the ball hitting sound

            //updating the score/ highscore
            scoreValue = scoreValue + 1;
            ScoreBoard.instance.UpdateScore(scoreValue);
            if (scoreValue > highScoreValue)
            {
                highScoreValue = scoreValue;
                ScoreBoard.instance.UpdateHighScore(highScoreValue);
            }
        }
    }

    public void ResetScore()
    {
        scoreValue = 0;
    }
}
