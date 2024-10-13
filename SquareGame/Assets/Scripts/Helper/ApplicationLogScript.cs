using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationLogScript : MonoBehaviour
{
    uint qsize = 15;  // number of messages to keep
    Queue myLogQueue = new Queue();

    void Start()
    {
        Debug.Log("Started up logging.");
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        myLogQueue.Enqueue("[" + type + "] : " + logString);
        if (type == LogType.Exception)
            myLogQueue.Enqueue(stackTrace);
        while (myLogQueue.Count > qsize)
            myLogQueue.Dequeue();
    }

    void OnGUI()
    {
        GUIStyle customStyle = new GUIStyle(GUI.skin.label);
        customStyle.fontSize = 26; // Set your desired font size here
        customStyle.normal.textColor = Color.white; // Optional: set text color

        GUILayout.BeginArea(new Rect(0, 0, 400, Screen.height));
        GUILayout.Label("\n" + string.Join("\n", myLogQueue.ToArray()) , customStyle);
        GUILayout.EndArea();
    }
}
