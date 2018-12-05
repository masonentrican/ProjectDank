using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[BoltGlobalBehaviour(BoltNetworkModes.Server)]                      // Attribute (BoltNetworkModes.Server) tells Bolt to only create instance of this class on the server
public class ServerCallbacks : Bolt.GlobalEventListener
{

    
    public override void Connected(BoltConnection connection)
    {
        var log = LogEvent.Create();
        log.Message = string.Format("{0} connected", connection.RemoteEndPoint);
        log.Send();
    }

    public override void Disconnected(BoltConnection connection)
    {
        var log = LogEvent.Create();
        log.Message = string.Format("{0} disconnected", connection.RemoteEndPoint);
        log.Send();
    }
}
