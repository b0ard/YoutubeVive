using UnityEngine;
using System.Collections;

public class DudeBehavior : InteractableItem, EnemyBehaviorInterface {
    private CastleBehavior castle;
    private Vector3 castlePos;

    private float speed = 0.1f;
    private float sqrAttackRange = 0.2f;

    private int attackPoints = 1;
    private float attackCooldown = 1f;
    private float killThreshold = 1.5f;

    Vector3 destination;

	// Use this for initialization
	void Start () {
        base.Start();
        castle = GameObject.FindWithTag("Castle").GetComponent<CastleBehavior>();
        castlePos = castle.transform.position;
        StartCoroutine("Attack");
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y < -20) {
            Destroy(gameObject);
        }

        base.Update();
        Move();
	}

    public void Move() {
        if (!currentlyInteracting) {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, castlePos, step);
        }
    }

    public IEnumerator Attack() {
        while (true) {
            if ((transform.position - castlePos).sqrMagnitude < sqrAttackRange) {
                GetComponent<Renderer>().material.color = Color.red;
                castle.ProcessDamage(attackPoints);
            } else {
                GetComponent<Renderer>().material.color = Color.white;
            }
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    public void OnCollisionEnter(Collision collision) {
        if (rigidbody.velocity.magnitude > killThreshold) {
            Destroy(gameObject);
        }
    }
}
