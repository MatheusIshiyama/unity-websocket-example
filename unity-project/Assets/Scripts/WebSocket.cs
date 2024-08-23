using UnityEngine;
using NativeWebSocket;
using System;


public class WebSocketClient : MonoBehaviour
{
  private WebSocket webSocket;
  private bool reconnecting = false;

  async void Start()
  {
    await ConnectWebSocket();
  }

  private async System.Threading.Tasks.Task ConnectWebSocket()
  {
    webSocket = new WebSocket("ws://localhost:3000");

    webSocket.OnOpen += () =>
    {
      Debug.Log("WebSocket connection opened.");
    };

    webSocket.OnMessage += (bytes) =>
    {
      string message = System.Text.Encoding.UTF8.GetString(bytes);
      Debug.Log("Received from server: " + message);
    };

    webSocket.OnError += (error) =>
    {
      Debug.LogError("WebSocket error: " + error);
    };

    webSocket.OnClose += async (reason) =>
    {
      Debug.Log($"WebSocket connection closed reason: {reason}");
      if (!reconnecting)
      {
        reconnecting = true;
        await ReconnectWebSocket();
      }
    };

    try
    {
      await webSocket.Connect();
    }
    catch (Exception ex)
    {
      Debug.LogError($"WebSocket connection failed: {ex.Message}");
    }
  }

  private async System.Threading.Tasks.Task ReconnectWebSocket()
  {
    Debug.Log("Attempting to reconnect...");
    await System.Threading.Tasks.Task.Delay(5000); // Wait before reconnecting
    await ConnectWebSocket();
    reconnecting = false;
  }

  void Update()
  {
    webSocket.DispatchMessageQueue();
  }

  private async void OnDestroy()
  {
    if (webSocket != null)
    {
      await webSocket.Close();
    }
  }
}