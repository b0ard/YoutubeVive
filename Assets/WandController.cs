using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Basic implementation of how to use a Vive controller as an input device.
 * Can only interact with items with InteractableBase component
 */
public class WandController : MonoBehaviour {
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    HashSet<InteractableBase> objectsHoveringOver = new HashSet<InteractableBase>();

    private InteractableBase closestItem;
    private InteractableBase interactingItem;

    // Use this for initialization
    void Start() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update() {
        if (controller == null) {
            Debug.Log("Controller not initialized");
            return;
        }

        if (controller.GetPressDown(gripButton) || controller.GetPressDown(triggerButton)) {
            // Find the closest item to the hand in case there are multiple and interact with it
            closestItem = null;
			interactingItem = null;

            float minDistance = float.MaxValue;

            float distance;
            foreach (InteractableBase item in objectsHoveringOver) {
                distance = (item.transform.position - transform.position).sqrMagnitude;

                if (distance < minDistance) {
                    minDistance = distance;
                    closestItem = item;
                }
            }

            interactingItem = closestItem;
            closestItem = null;

            if (interactingItem) {
                // Begin Interaction should already end interaction from previous
                if (controller.GetPressDown(gripButton)) {
                    interactingItem.OnGripPressDown(this);
                }
                if (controller.GetPressDown(triggerButton)) {
                    interactingItem.OnTriggerPressDown(this);
                }
            }
        }

        if (controller.GetPressUp(gripButton) && interactingItem != null) {
            //interactingItem.EndInteraction(this);
            interactingItem.OnGripPressUp(this);
        }

        if (controller.GetPressDown(triggerButton) && interactingItem != null) {
            interactingItem.OnTriggerPressUp(this);
        }
    }

    // Adds all colliding items to a HashSet for processing which is closest
    private void OnTriggerEnter(Collider collider) {
        InteractableBase collidedItem = collider.GetComponent<InteractableBase>();
        if (collidedItem) {
            objectsHoveringOver.Add(collidedItem);
        }
    }

    // Remove all items no longer colliding with to avoid further processing
    private void OnTriggerExit(Collider collider) {
        InteractableBase collidedItem = collider.GetComponent<InteractableBase>();
        if (collidedItem) {
            objectsHoveringOver.Remove(collidedItem);
        }
    }
}
