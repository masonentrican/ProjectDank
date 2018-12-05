using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BoltGlobalBehaviour]
public class NetworkCallbacks : Bolt.GlobalEventListener
{
    List<string> logMessages = new List<string>();


    /// <summary>
    /// Function runs on scene loading. Instantiates a cube prefab that has our control logic attached
    /// </summary>
    public override void SceneLoadLocalDone(string map)
    {
        // Random spawn position
        var spawnPosition = new Vector3(Random.Range(-8, 8), 0, Random.Range(-8, 8));

        // instantiate cube
        BoltNetwork.Instantiate(BoltPrefabs.Robot, spawnPosition, Quaternion.identity);
    }


    /// <summary>
    /// Handles receiving events
    /// </summary>    
    public override void OnEvent(LogEvent evnt)
    {
        logMessages.Insert(0, evnt.Message);
    }


    /// <summary>
    /// Create a small box and display 5 newest log messages within.
    /// </summary>
    void OnGUI()
    {
        // only display max the 5 latest log messages
        int maxMessages = Mathf.Min(5, logMessages.Count);

        GUILayout.BeginArea(new Rect(Screen.width / 2 - 200, Screen.height - 100, 400, 100), GUI.skin.box);

        for (int i = 0; i < maxMessages; ++i)
        {
            GUILayout.Label(logMessages[i]);
        }

        GUILayout.EndArea();
    }
}
