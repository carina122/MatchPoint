﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    Vector3 initialPos; // ball's initial position

    private void Start()
    {
        initialPos = transform.position; // default it to where we first place it in the scene
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Wall")) // if the ball hits a wall
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero; // reset it's velocity to 0 so it doesn't move anymore
            transform.position = initialPos; // reset its position 
            ScoreBoard.instance.UpdateScore(0); // reset the player's score to 0
            Player.instance.ResetScore(); //using the ResetScore method from the player's script
        }
    }
}
