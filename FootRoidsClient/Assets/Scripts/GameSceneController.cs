﻿using System.Collections;
using System.Collections.Generic;
using Multiplayer;
using Nakama;
using Nakama.TinyJson;
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
    
    //temp
    [SerializeField] GameObject m_TempOtherShip;
    GameObject m_TempOtherShipInstance;

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
        
        ServerSessionManager.Instance.Socket.ReceivedMatchState += OnReceivedMatchState;

        m_TempOtherShipInstance = Instantiate(m_TempOtherShip, Vector3.zero, Quaternion.identity);
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

    void OnReceivedMatchState(IMatchState newState)
    {
        var enc = System.Text.Encoding.UTF8;
        var content = enc.GetString(newState.State);
        switch (newState.OpCode)
        {
            case 341:
                Debug.Log(content);
                
                var values = content.FromJson<Dictionary<string, Vector2>>();
                
                Debug.Log(values);

                //var pos = new Vector3(values["x"], values["y"], 0.0f);

                //m_TempOtherShipInstance.transform.position = pos;
                
                break;

            default:
                Debug.LogFormat("User '{0}'' sent '{1}'", newState.UserPresence.Username, content);
                break;
        }
    }
}
