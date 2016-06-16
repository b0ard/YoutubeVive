using UnityEngine;
using System.Collections;

public abstract class InteractableBase : MonoBehaviour {
    // Default definition for buttons is NOOP
    public virtual void OnTriggerPressDown(WandController wand) {}
    public virtual void OnTriggerPressUp(WandController wand) {}
    public virtual void OnGripPressDown(WandController wand) {}
    public virtual void OnGripPressUp(WandController wand) {}
}
