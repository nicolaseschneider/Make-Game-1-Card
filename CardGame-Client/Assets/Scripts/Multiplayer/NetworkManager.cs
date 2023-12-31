using KarateDev.Core;
using Riptide;
using Riptide.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ServerToClientMsg : ushort
{
    ApproveLogin,
}
public enum ClientToServerMsg : ushort
{
    RequestLogin,
}
public class NetworkManager : Singleton<NetworkManager>
{
    protected override void Awake()
    {
        base.Awake();
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, true);
    }

    public Client client;
    [SerializeField] private ushort m_Port = 7777;
    [SerializeField] private string m_Ip = "127.0.0.1";
    private static string s_LocalUsername;
    private void Start() 
    {
        client = new Client();
        client.Connected += OnClientConnected;
    }
    private void OnClientConnected(object sender, EventArgs e)
    {
        PlayerManager.Instance.SpawnInitialPlayer(s_LocalUsername);
    }
    public void Connect(string username)
    {
        s_LocalUsername = string.IsNullOrEmpty(username) ? $"Guest" : username;
        client.Connect($"{m_Ip}:{m_Port}");
    }
    private void FixedUpdate()
    {
        client.Update();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        client.Connected -= OnClientConnected;
    }
}
