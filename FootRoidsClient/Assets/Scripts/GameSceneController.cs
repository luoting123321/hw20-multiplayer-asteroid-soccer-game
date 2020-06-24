﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneController : MonoBehaviour
{
    [Header("Player Settings")]
    [Range(5, 20)]
    public float playerSpeed;

    [Header("Screen Settings")]
    [Space]
    public Vector3 screenBounds;

    [Header("IsMaster")]
    public bool isMaster;

    public int maxAsteroids;
    public GameObject asteroid;

    private int totalPoints;
    private HUDController hUDController;
    private PlayerController player;

    public GameObject[] playerList;
    public GameObject playerObjectPrefab;
    public int numOfPlayers;



    // Start is called before the first frame update
    void Start()
    {
        hUDController = FindObjectOfType<HUDController>();
        screenBounds = GetScreenBounds();
        player = FindObjectOfType<PlayerController>();
        playerObjectPrefab = Resources.Load<GameObject>("ship");
        StartCoroutine(SpawnAsteroids());
        numOfPlayers = 1;
        StartCoroutine(SpawnPlayers());
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Spawn Asteroids in random locations  #Needs work not visible yet
    private IEnumerator SpawnAsteroids()
    {
        for (int currentAsteroids = 0; currentAsteroids < maxAsteroids; currentAsteroids++)
        {
            float horizontalPosition = Random.Range(-screenBounds.x, screenBounds.x);
            float verticalPosition = Random.Range(-screenBounds.y, screenBounds.y);
            Instantiate(asteroid, new Vector2(horizontalPosition, verticalPosition), Quaternion.identity);
            yield return true;
        };
    }

    // Spawn Players in sides of the field ##Needs Fixing
    private IEnumerator SpawnPlayers()

    {
        
        for (int currentPlayers = 0; currentPlayers < numOfPlayers; currentPlayers++)
        {
            float horizontalPosition = Random.Range(-screenBounds.x, screenBounds.x);
            float verticalPosition = Random.Range(-screenBounds.y, screenBounds.y);
            // instantiate a player
            Instantiate(playerObjectPrefab, new Vector2(horizontalPosition, verticalPosition), Quaternion.identity);                
            yield return true;
        };
    }
    

    // Get the screen bounds
    private Vector3 GetScreenBounds()
    {
        Camera mainCamera = Camera.main;
        Vector3 screenVector = new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z);

        return mainCamera.ScreenToWorldPoint(screenVector);
    }
}
