using UnityEngine;
using System.Collections;

/**
 * TODO: Mark as abstract if appropriate?
 * TODO: PID controller implementation
 *
 * InteractableItems are GameObjects with a rigidbody that can be picked up using the Vive
 * controllers. To avoid clipping through solid objects, picked objects are manipulated by
 * setting the velocity directly rather than its position.
 *
 * They have mass sensitive interaction built in; that is, a object with heavier mass will
 * more effort from the player to move, whether by investing more time (due to slower velocity)
 * or more movement (to increase the delta vector which increases the velocity).
 */
public class InteractableItem : MonoBehaviour {
    protected Rigidbody rigidbody;
    protected bool currentlyInteracting;

    // velocity_obj = (hand_pos - obj_pos) * velocityFactor / rigidbody.mass
    private float velocityFactor = 20000f;
    private Vector3 posDelta; // posDelta = (hand_pos - obj_pos)

    private float rotationFactor = 600f;
    private Quaternion rotationDelta;
    private float angle;
    private Vector3 axis;

    // The controller this object is picked up by
    private WandController attachedWand; 
    // The point at which the object was grabbed when picked up
    private Transform interactionPoint;

	// Use this for initialization
	protected void Start () {
        rigidbody = GetComponent<Rigidbody>();
        interactionPoint = new GameObject().transform;
        velocityFactor /= rigidbody.mass;
        rotationFactor /= rigidbody.mass;
	}
	
	// Update is called once per frame
    // TODO: Use FixedUpdate for rigidbody manipulation
	protected void Update() {
	    if (attachedWand && currentlyInteracting) {
            posDelta = attachedWand.transform.position - interactionPoint.position;
            this.rigidbody.velocity = posDelta * velocityFactor * Time.fixedDeltaTime;

            rotationDelta = attachedWand.transform.rotation * Quaternion.Inverse(interactionPoint.rotation);
            rotationDelta.ToAngleAxis(out angle, out axis);

            if (angle > 180) {
                angle -= 360;
            }

            this.rigidbody.angularVelocity = (Time.fixedDeltaTime * angle * axis) * rotationFactor;
        }
	}

    public void BeginInteraction(WandController wand) {
        attachedWand = wand;
        interactionPoint.position = wand.transform.position;
        interactionPoint.rotation = wand.transform.rotation;
        interactionPoint.SetParent(transform, true);

        currentlyInteracting = true;
    }

    public void EndInteraction(WandController wand) {
        if (wand == attachedWand) {
            attachedWand = null;
            currentlyInteracting = false;
        }
    }

    public bool IsInteracting() {
        return currentlyInteracting;
    }

    private void OnDestroy() {
        // Destroy the empty game object associated with interaction point
        Destroy(interactionPoint.gameObject);
    }
}
