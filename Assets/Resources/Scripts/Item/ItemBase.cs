using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    [SerializeField]
    public GameObject shellPrefab;

    [SerializeField]
    public GameObject impactPrefab;

    [SerializeField]
    public GameObject trailPrefab;

    [SerializeField]
    public Transform muzzleFlash;

    [SerializeField]
    public Transform shellEjector;

    [SerializeField]
    public AudioClip fireSound;

    [SerializeField]
    public byte damage = 25;

    [SerializeField]
    public int rpm = 600;

    public int FireInterval
    {
        get
        {
            // calculate rounds per second
            int rps = (rpm / 60);

            // calculate frames between each round
            return BoltNetwork.framesPerSecond / rps;
        }
    }

    public int FireFrame
    {
        get;
        set;
    }

    public virtual void HitDetection(PlayerCommand cmd, BoltEntity entity)
    {
    }

    public virtual void DisplayEffects(BoltEntity entity)
    {
    }
    public virtual void PrimaryFire(BoltEntity entity)
    {

    }
    public virtual void OnOwner(PlayerCommand cmd, BoltEntity entity)
    {

    }

    public virtual void Fx(BoltEntity entity)
    {

    }
}
