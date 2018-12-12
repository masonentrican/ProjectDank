
using UnityEngine;
[BoltGlobalBehaviour("GettingStarted")]


    public class PlayerCallbacks : Bolt.GlobalEventListener
    {

        public override void SceneLoadLocalDone(string map)
        {
        // this just instantiates our player camera,
        // the Instantiate() method is supplied by the BoltSingletonPrefab<T> class
        PlayerCamera.Instantiate();
        }

        public override void ControlOfEntityGained(BoltEntity entity)
        {
            // give the camera our players pitch
            PlayerCamera.instance.getPitch = () => entity.GetState<IPlayerState>().pitch;
            // this tells the player camera to look at the entity we are controlling
            PlayerCamera.instance.SetTarget(entity);
            // add an audio listener for our character
            entity.gameObject.AddComponent<AudioListener>();
        }
    }

