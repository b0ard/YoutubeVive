using UnityEngine;
using System.Collections;

public class DudeBehavior : PhysicsInteractable {
    // Defined by prefabs
    public float moveSpeed;
    public float attackRange;
    public int attackPoints;
    public float killThreshold;

    private CastleBehavior castle;
    private Vector3 castlePos;

    private float rotateSpeed = 2f;
    private float sqrAttackRange;

    private float attackCooldown = 1f;

    private Animator animator;

    private Vector3 destination;
    private Vector3 rotateDir;

	// Use this for initialization
	void Start () {
        base.Start();
        sqrAttackRange = attackRange * attackRange;

        castle = GameObject.FindWithTag("Castle").GetComponent<CastleBehavior>();
        castlePos = castle.transform.position;

        animator = GetComponentInChildren<Animator>();

        StartCoroutine("Attack");
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y < -10) {
            Destroy(gameObject);
        }

        base.Update();
        RotateAndMove();
	}

    public void RotateAndMove() {
        if (!currentlyInteracting) {
            rotateDir = Vector3.RotateTowards(transform.forward,
                    castlePos - transform.position,
                    Time.deltaTime * rotateSpeed,
                    1.0F);

            transform.rotation = Quaternion.LookRotation(rotateDir);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);

            if (!IsInAttackRange()) {
                float step = moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, castlePos, step);
            }
        }
    }

    public IEnumerator Attack() {
        while (true) {
            if ((transform.position - castlePos).sqrMagnitude < sqrAttackRange) {
                animator.SetBool("Attack", true);
                castle.ProcessDamage(attackPoints);
            } else {
                animator.SetBool("Attack", false);
            }
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    public void OnCollisionEnter(Collision collision) {
        if (rigidbody.velocity.magnitude > killThreshold) {
            Destroy(gameObject);
            // Destroy the other one too if it's a Physics Interactable
            if (collision.gameObject.GetComponent<PhysicsInteractable>()) {
                Destroy(collision.gameObject);
            }
        }
    }

    /**
     In attack range of Castle
     */
    private bool IsInAttackRange() {
        return ((transform.position - castlePos).sqrMagnitude < sqrAttackRange);
    }
}
