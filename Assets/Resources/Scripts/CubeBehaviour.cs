using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour : Bolt.EntityEventListener<ICubeState>
{
    public GameObject[] WeaponObjects;
    float resetColorTime;
    Renderer renderer;

    public override void Attached()
    {
        renderer = GetComponent<Renderer>();

        state.SetTransforms(state.CubeTransform, transform);

        if (entity.isOwner)
        {
            // setup cube color for our cube
            state.CubeColor = new Color(Random.value, Random.value, Random.value);

            // setup weapons
            for (int i = 0; i < state.WeaponArray.Length; ++i)
            {
                state.WeaponArray[i].WeaponId = i;
                state.WeaponArray[i].WeaponAmmo = Random.Range(50, 100);
            }

            // by default we dont have a weapon active, index -1 defines that.
            state.WeaponActiveIndex = -1;
        }

        // Setup cube colorchange callback
        state.AddCallback("CubeColor", ColorChanged);

        // Setup active weapon callback
        state.AddCallback("WeaponActiveIndex", WeaponActiveIndexChanged);
    }

    public override void SimulateOwner()
    {
        var speed = 4f;
        var movement = Vector3.zero;

        // movement
        if (Input.GetKey(KeyCode.W)) { movement.z += 1; }
        if (Input.GetKey(KeyCode.S)) { movement.z -= 1; }
        if (Input.GetKey(KeyCode.A)) { movement.x -= 1; }
        if (Input.GetKey(KeyCode.D)) { movement.x += 1; }

        // input for weapon selection
        if (Input.GetKeyDown(KeyCode.Alpha0)) { state.WeaponActiveIndex = -1; }
        if (Input.GetKeyDown(KeyCode.Alpha1)) { state.WeaponActiveIndex = 0;  }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { state.WeaponActiveIndex = 1;  }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { state.WeaponActiveIndex = 2;  }

        
        // cube flashing input
        if (Input.GetKeyDown(KeyCode.F))
        {
            var flash = FlashColorEvent.Create(entity);
            flash.FlashColor = Color.red;
            flash.Send();
        }

        // actual transform movement
        if (movement != Vector3.zero)
        {
            transform.position = transform.position + (movement.normalized * speed * BoltNetwork.frameDeltaTime);
        }
    }

    void OnGUI()
    {
        if (entity.isOwner)
        {
            GUI.color = state.CubeColor;
            GUILayout.Label("@@@");
            GUI.color = Color.white;
        }
    }

    public override void OnEvent(FlashColorEvent evnt)
    {
        resetColorTime = Time.time + 0.2f;
        renderer.material.color = evnt.FlashColor;
    }

    void ColorChanged()
    {
        GetComponent<Renderer>().material.color = state.CubeColor;
    }

    void WeaponActiveIndexChanged()
    {
        // disable active for all weapons
        for (int i = 0; i < WeaponObjects.Length; ++i)
        {
            WeaponObjects[i].SetActive(false);
        }        

        // if a weapon has an active index that is >= 0 set the weapon object to active
        if (state.WeaponActiveIndex >= 0)
        {
            int objectId = state.WeaponArray[state.WeaponActiveIndex].WeaponId;
            WeaponObjects[objectId].SetActive(true);
        }
    }

    void Update()
    {
        if (resetColorTime < Time.time)
        {
            renderer.material.color = state.CubeColor;
        }
    }
}


