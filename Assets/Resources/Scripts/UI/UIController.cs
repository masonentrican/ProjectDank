using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : BoltSingletonPrefab<UIController>
{
    
    GameObject[] pauseObjects;
    GameObject[] inventoryObjects;
    public GameObject player;

    // Use this for initialization
    void Start()
    {
        
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        inventoryObjects = GameObject.FindGameObjectsWithTag("ShowOnInventory");
        hidePaused();
        hideInventory();
    }    

    public void SetPlayer(BoltEntity entity)
    {
        player = entity;        
    }

    public void SetUiReferences()
    {
        player.GetComponent<PlayerController>()._uiController = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);        
    }

    public void InventoryToggle()
    {
        Debug.Log("UIController.cs : InvetoryToggle() - Call Recieved from PlayerController.ToggleInventory()");

        foreach (GameObject g in inventoryObjects)
        {
            if (g.activeSelf == false)
            {
                showInventory();
            }
            else
            {
                hideInventory();
            }

        }
    }

    //controls the pausing of the scene
    public void pauseControl()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            showPaused();
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            hidePaused();
        }
    }

    //shows objects with ShowOnPause tag
    public void showPaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }

    //hides objects with ShowOnPause tag
    public void hidePaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    //shows objects with ShowOnPause tag
    public void showInventory()
    {
        // Set player uiState to inventory
        player.GetComponent<PlayerController>().uiState = PlayerController.uiStateMachine._inventory;
        
        // Activate all inventory gui elements
        foreach (GameObject g in inventoryObjects)
        {
            g.SetActive(true);
        }
    }

    //hides objects with ShowOnPause tag
    public void hideInventory()
    {
        player.GetComponent<PlayerController>().uiState = PlayerController.uiStateMachine._none;        
        
        // De-activate all inventory gui elements
        foreach (GameObject g in inventoryObjects)
        {
            g.SetActive(false);
        }
    }

}
