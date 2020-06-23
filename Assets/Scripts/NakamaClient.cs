﻿using System.Collections;
using System.Collections.Generic;
using Nakama;
using Nakama.TinyJson;
using UnityEngine;

public class NakamaClient : MonoBehaviour
{
    private const string RoomName = "FootRoDa";
    private readonly IClient client = new Client("http", "127.0.0.1", 7350, "defaultkey");

    private ISocket socket;

    // Start is called before the first frame update
    async void Start()
    {
        const string email = "hello@example.com";
        const string password = "verysecure";        
        var session = await client.AuthenticateEmailAsync(email, password);
        Debug.Log(session);

        socket = client.NewSocket();
        socket.Connected += () => Debug.Log("Socket connected.");
        socket.Closed += () => Debug.Log("Socket closed");

        socket.ReceivedChannelMessage += async matched =>
        {
            Debug.LogFormat("Match: {0}", matched);
        };

        await socket.ConnectAsync(session);
        await socket.AddMatchmakerAsync("*", 2, 2);

        var channel = await socket.JoinChatAsync(RoomName, ChannelType.Room);
        Debug.LogFormat("Join chat channel: {0}", channel);

        var content = new Dictionary<string, string> { { "hello", "world" } }.ToJson();
        _ = socket.WriteChatMessageAsync(channel, content);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
