using UnityEngine;
using System.Collections;
using UnityEngine;
using Bolt;


    public class ItemWrench : ItemBase
    {
        
        public Transform asset;
        public Material phantomMaterial;
        int currentPrefab = 0;
        PrefabId prefabToSpawn = BoltPrefabs.DoorWay;
        public enum ConstructStateMachine { none, phantom };
        public ConstructStateMachine constructState = ConstructStateMachine.none;

        GameObject phantom;
        GameObject ghostWall = null;
        Transform spawnPoint = null;
        BoltEntity player;
        void Awake()
        {
            player = GetComponentInParent<BoltEntity>();
        }
        void Update()
        {
            if (player.isOwner)
            {
                
                float maxDistance = 100f;
                Vector3 pos;
                Quaternion look;
                IPlayerState state = player.GetState<IPlayerState>();
                // this calculate the looking angle for this specific entity
                PlayerCamera.instance.CalculateCameraAimTransform(player.transform, state.pitch, out pos, out look);
                Debug.DrawRay(pos, look * Vector3.forward);
                RaycastHit hit;
                Ray r = new Ray(pos, look * Vector3.forward);

                if (Physics.Raycast(r, out hit, maxDistance))
                {
                    if (prefabToSpawn == BoltPrefabs.DoorWay && hit.transform.tag.ToString() == "WallTop")
                    {
                        //Debug.Log("hti the raycast");
                        if (ghostWall == null)
                        {
                            ghostWall = hit.transform.GetChild(1).gameObject;
                            ghostWall.SetActive(true);
                            spawnPoint = hit.transform.GetChild(2).transform;
                        }
                    }
                    else if (prefabToSpawn == BoltPrefabs.Wall && hit.transform.tag.ToString() == "WallTop")
                    {
                        //Debug.Log("hti the raycast");
                        if (ghostWall == null)
                        {
                            ghostWall = hit.transform.GetChild(0).gameObject;
                            ghostWall.SetActive(true);
                            spawnPoint = hit.transform.GetChild(2).transform;
                        }
                    }
                    else if (prefabToSpawn == BoltPrefabs.Floor && hit.transform.tag.ToString() == "FloorTop")
                    {
                        if (ghostWall == null)
                        {
                            //Debug.Log("hti the raycast");
                            ghostWall = hit.transform.GetChild(0).gameObject;
                            ghostWall.SetActive(true);
                            spawnPoint = hit.transform.GetChild(1).transform;
                        }
                    }
                    else if (prefabToSpawn == BoltPrefabs.Foundation && hit.transform.tag.ToString() == "Foundation")
                    {
                        if (ghostWall == null)
                        {
                            //Debug.Log("hti the raycast");
                            ghostWall = hit.transform.GetChild(0).gameObject;
                            ghostWall.SetActive(true);
                            spawnPoint = hit.transform.GetChild(1).transform;
                        }
                    }

                    else if (ghostWall != null)
                    {

                        ghostWall.SetActive(false);
                        ghostWall = null;
                        spawnPoint = null;
                    }


                }
            }
            
        }
        public override void PrimaryFire(PlayerCommand cmd, BoltEntity entity)
        {

            if (entity.isOwner)
            {
                if(prefabToSpawn == BoltPrefabs.Foundation && spawnPoint == null)
                {
                    Debug.Log("test");
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
                        BoltNetwork.Instantiate(prefabToSpawn, new Vector3(rh.point.x, rh.point.y, rh.point.z), entity.transform.rotation);
                    }
                }
                if(spawnPoint)
                {
                    BoltNetwork.Instantiate(prefabToSpawn, new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z), spawnPoint.rotation);
                }
                
                /*IPlayerState state = entity.GetState<IPlayerState>();
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
                    if(prefabToSpawn == BoltPrefabs.Floor)
                    {
                        if (rh.transform.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                        {
                            BoltNetwork.Instantiate(prefabToSpawn, new Vector3(rh.point.x, rh.point.y, rh.point.z), entity.transform.rotation);
                        }
                        else
                            BoltNetwork.Instantiate(prefabToSpawn, new Vector3(rh.point.x, rh.point.y, rh.point.z), entity.transform.rotation);
                    }
                    else
                        BoltNetwork.Instantiate(prefabToSpawn, new Vector3(rh.point.x, rh.point.y + 1.5f, rh.point.z), entity.transform.rotation);

                    var en = rh.transform.GetComponent<BoltEntity>();
                }*/
            }
        }

        public override void SecondaryFire(PlayerCommand cmd, BoltEntity entity)
        {
            if (entity.isOwner)
            {
                
                switch (currentPrefab)
                {
                    case 0:
                        prefabToSpawn = BoltPrefabs.Foundation;
                        currentPrefab++;
                        break;
                    case 1:
                        prefabToSpawn = BoltPrefabs.Wall;
                        currentPrefab++;
                        break;
                    case 2:
                        prefabToSpawn = BoltPrefabs.Floor;
                        currentPrefab++;
                        break;
                    case 3:
                        prefabToSpawn = BoltPrefabs.DoorWay;
                        currentPrefab = 0;
                        break;
                    default:
                        if (currentPrefab < 0 || currentPrefab > 3)
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

