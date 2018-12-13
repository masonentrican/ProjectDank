using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour, IHasChanged {

    [SerializeField] Transform slots;
    [SerializeField] Text inventoryText;

    // Use this for initialization
    void Start () {
        HasChanged();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Builds a string of item names.
    public void HasChanged()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        builder.Append(" - ");
        foreach(Transform slotTransform in slots)
        {
            GameObject item = slotTransform.GetComponent<SlotHandler>().item;
            if (item)
            {
                builder.Append(item.name);
                builder.Append(" - ");
            }
        }
        inventoryText.text = builder.ToString();
    }
}


namespace UnityEngine.EventSystems
{
    public interface IHasChanged : IEventSystemHandler
    {
        void HasChanged();
    }
}