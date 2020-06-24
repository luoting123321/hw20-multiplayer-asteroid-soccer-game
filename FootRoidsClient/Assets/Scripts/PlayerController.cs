﻿using System.Collections.Generic;
using Multiplayer;
using Nakama.TinyJson;
using UnityEngine;
using UnityEngine.XR.WSA;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sr;

    public float thrust;
    public float turnThrust;
    public float deathSpeed;
    public float mediumSpeed;
    public float maxSpeed;

    Vector3 m_PreviousPos = new Vector3();
    
    // Access the GameSceneController
    public GameSceneController gameSceneController;

    private float thrustInput;
    private float turnInput;

    // Start is called before the first frame update
    void Start()
    {
        gameSceneController = FindObjectOfType<GameSceneController>();
        rb = GetComponentInParent<Rigidbody2D>();
        sr = GetComponentInParent<SpriteRenderer>();

        m_PreviousPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
        MovePlayer();
    }

    // Fixed timing update
    void FixedUpdate()
    {
        var pos = transform.position;
        if (m_PreviousPos != pos)
        {
            var matchId = MatchCommunicationManager.Instance.MatchId;
            var opCode = 341;
            
            var newState = new Dictionary<string, float> {{"x", pos.x}, {"y", pos.y}}.ToJson();
            ServerSessionManager.Instance.Socket.SendMatchStateAsync(matchId, opCode, newState);
        }

        m_PreviousPos = pos;
        
        
        // Get input and apply thrust
        thrustInput = Input.GetAxis("Vertical");
        rb.AddRelativeForce(Vector2.up * thrustInput * thrust);
        //rb.AddTorque(-turnInput * turnThrust);
        if (rb.velocity.magnitude > maxSpeed) {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void ChangeColor()
    {
        //change speed color
        if (rb.velocity.magnitude > mediumSpeed && rb.velocity.magnitude < deathSpeed)
        {
            sr.color = Color.yellow;
        }
        else if (rb.velocity.magnitude > deathSpeed)
        {
            sr.color = Color.red;
        }
        else
        {
            sr.color = Color.white;
        }
    }

    private void MovePlayer()
    {
        // Get input and apply thrust
        turnInput = Input.GetAxis("Horizontal");

        //rotate the ship 
        transform.Rotate(Vector3.forward * -turnInput * Time.deltaTime * turnThrust);

        // Screen wrapping
        Vector2 newPos = transform.position;

        // Check if the Asteroid has moved out of screenBounds
        if (transform.position.y > gameSceneController.screenBounds.y)
        {
            newPos.y = -gameSceneController.screenBounds.y;
        }
        if (transform.position.y < -gameSceneController.screenBounds.y)
        {
            newPos.y = gameSceneController.screenBounds.y;
        }

        if (transform.position.x > gameSceneController.screenBounds.x)
        {
            newPos.x = -gameSceneController.screenBounds.x;
        }
        if (transform.position.x < -gameSceneController.screenBounds.x)
        {
            newPos.x = gameSceneController.screenBounds.x;
        }

        // Set the position back to the transform
        transform.position = newPos;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("relative hit: " + col.relativeVelocity.magnitude);
        if(col.relativeVelocity.magnitude > mediumSpeed) {
            Debug.Log("boom");
        }
    }
}
