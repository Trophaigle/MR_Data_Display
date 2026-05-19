using UnityEngine;
using NativeWebSocket;

public class DataFetcher : MonoBehaviour
{
    public KPIPanel kpiPanel;
    public string ip;
    public string port;
    public TemperatureGraph temperatureGraph;
    private bool graphInitialized = false;

    WebSocket websocket;

    async void Start()
    {
        Application.runInBackground = true;

        if(ip == null || ip == "")
        {
            Debug.LogError("IP address is not set!");
            return;
        }
        if(port == null || port == "")
        {
            Debug.LogError("Port is not set!");
            return;
        }

        Debug.Log($"IP = '{ip}'");
        Debug.Log($"PORT = '{port}'");
        Debug.Log($"URL = ws://{ip}:{port}/ws/kpi");

        string url = $"ws://{ip}:{port}/ws/kpi";

        websocket = new WebSocket(url);

        websocket.OnOpen += () =>
        {
            Debug.Log("KPI WebSocket connected!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("WebSocket Error: " + e);
        };

        websocket.OnClose += (code) =>
        {
            Debug.Log("WebSocket closed: " + code);
        };

        websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);

            KPIData data = JsonUtility.FromJson<KPIData>(message);

            Debug.Log("Received KPI: " + message);

            kpiPanel.UpdateKPI("Avg Temp", data.avg_temp.ToString("F1") + "°C");

            if (!graphInitialized)
            {
                temperatureGraph.BuildGraph(data.values, data.days, data.effects);
                graphInitialized = true;
            }
            else
            {
                temperatureGraph.UpdateGraphData(data.values, data.days, data.effects);
            }
                };

        await websocket.Connect();
    }

   void Update()
{
#if !UNITY_WEBGL || UNITY_EDITOR
    if (websocket != null &&
        websocket.State == NativeWebSocket.WebSocketState.Open)
    {
        websocket.DispatchMessageQueue();
    }
#endif
}

    private async void OnApplicationQuit()
    {
        if (websocket != null)
            await websocket.Close();
    }
}

[System.Serializable]
public class KPIData
{
    public string[] days;
    public float[] values;

    public string[] effects;
    public float avg_temp;
}
