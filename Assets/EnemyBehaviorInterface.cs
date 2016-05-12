using UnityEngine;
using System.Collections;

/**
 * Enemies implement this interface to define
 * how the enemy unit moves and attacks the castle.
 *
 * Attack() is a coroutine because it has a cooldown
 * to avoid attacking the castle every frame.
 */
public interface EnemyBehaviorInterface {
    void Move();
    IEnumerator Attack();
}
