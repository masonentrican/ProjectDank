using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BoltGlobalBehaviour(BoltNetworkModes.Server, "GettingStarted")]
public class NetworkCallbacks : Bolt.GlobalEventListener
{
    List<string> logMessages = new List<string>();


    void Awake()
    {
        PlayerObjectRegistry.CreateServerPlayer();
    }

    public override void Connected(BoltConnection connection)
    {
        PlayerObjectRegistry.CreateClientPlayer(connection);
    }

    public override void SceneLoadLocalDone(string map)
    {
        PlayerObjectRegistry.ServerPlayer.Spawn();
    }

    public override void SceneLoadRemoteDone(BoltConnection connection)
    {
        PlayerObjectRegistry.GetPlayer(connection).Spawn();
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

        GUILayout.BeginArea(new Rect(15, 15, 400, 100), GUI.skin.box);

        for (int i = 0; i < maxMessages; ++i)
        {
            GUILayout.Label(logMessages[i]);
        }

        GUILayout.EndArea();
    }
}
