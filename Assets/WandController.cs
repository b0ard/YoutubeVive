using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Basic implementation of how to use a Vive controller as an input device.
 * Can only interact with items with InteractableItem component
 */
public class WandController : MonoBehaviour {
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    HashSet<InteractableItem> objectsHoveringOver = new HashSet<InteractableItem>();

    private InteractableItem closestItem;
    private InteractableItem interactingItem;

	// Use this for initialization
	void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (controller == null) {
            Debug.Log("Controller not initialized");
            return;
        }

        if (controller.GetPressDown(gripButton)) {
            // Find the closest item to the hand in case there are multiple and interact with it
            float minDistance = float.MaxValue;

            float distance;
            foreach (InteractableItem item in objectsHoveringOver) {
                distance = (item.transform.position - transform.position).sqrMagnitude;

                if (distance < minDistance) {
                    minDistance = distance;
                    closestItem = item;
                }
            }

            interactingItem = closestItem;
            closestItem = null;

            if (interactingItem) {
                if (interactingItem.IsInteracting()) {
                    interactingItem.EndInteraction(this);
                }

                interactingItem.BeginInteraction(this);
            }
        }

        if (controller.GetPressUp(gripButton) && interactingItem != null) {
            interactingItem.EndInteraction(this);
        }
	}

    // Adds all colliding items to a HashSet for processing which is closest
    private void OnTriggerEnter(Collider collider) {
        InteractableItem collidedItem = collider.GetComponent<InteractableItem>();
        if (collidedItem) {
            objectsHoveringOver.Add(collidedItem);
        }
    }

    // Remove all items no longer colliding with to avoid further processing
    private void OnTriggerExit(Collider collider) {
        InteractableItem collidedItem = collider.GetComponent<InteractableItem>();
        if (collidedItem) {
            objectsHoveringOver.Remove(collidedItem);
        }
    }
}
