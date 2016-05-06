using UnityEngine;
using System.Collections;

public interface EnemyBehaviorInterface {
    void Move();
    IEnumerator Attack();
}
