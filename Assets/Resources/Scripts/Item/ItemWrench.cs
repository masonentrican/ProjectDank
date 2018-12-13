using UnityEngine;
using System.Collections;
using UnityEngine;
using Bolt;


    public class ItemWrench : ItemBase
    {
        public Transform player;
        public Transform asset;
        public Material phantomMaterial;
        int currentPrefab = 0;
        PrefabId prefabToSpawn = BoltPrefabs.DoorWay;
        public enum ConstructStateMachine { none, phantom };
        public ConstructStateMachine constructState = ConstructStateMachine.none;

        GameObject phantom;


        public override void PrimaryFire(PlayerCommand cmd, BoltEntity entity)
        {
            if (entity.isOwner)
            {
                IPlayerState state = entity.GetState<IPlayerState>();
                PlayerController controller = entity.GetComponent<PlayerController>();

                Vector3 pos;
                Quaternion look;

                // this calculate the looking angle for this specific entity
                PlayerCamera.instance.CalculateCameraAimTransform(entity.transform, state.pitch, out pos, out look);

                // display debug
                Debug.DrawRay(pos, look * Vector3.forward);

                Ray r = new Ray(pos, look * Vector3.forward);
                RaycastHit rh;
                float maxDistance = 10f;

                if (Physics.Raycast(r, out rh, maxDistance))
                {
                    var en = rh.transform.GetComponent<BoltEntity>();
                    var hit = BoltNetwork.Instantiate(prefabToSpawn, new Vector3(rh.point.x, rh.point.y + 1.5f, rh.point.z), entity.transform.rotation);

                }
            }
        }

        public override void SecondaryFire(PlayerCommand cmd, BoltEntity entity)
        {
            if (entity.isOwner)
            {
                
                switch (currentPrefab)
                {
                    case 0:
                        prefabToSpawn = BoltPrefabs.DoorWay;
                        currentPrefab++;
                        break;
                    case 1:
                        prefabToSpawn = BoltPrefabs.Wall;
                        currentPrefab++;
                        break;
                    case 2:
                        prefabToSpawn = BoltPrefabs.Floor;
                        currentPrefab = 0;
                        break;
                    default:
                        if (currentPrefab < 0 || currentPrefab > 2)
                        {
                            currentPrefab = 0;
                        }
                        break;
                }
                
                Debug.Log(currentPrefab);
            }
        }

        public override void Fx(BoltEntity entity)
        {

        }
    }

