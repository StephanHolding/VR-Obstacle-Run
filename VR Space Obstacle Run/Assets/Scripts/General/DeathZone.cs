using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            Obstacle obstacle = other.GetComponent<Obstacle>();
            ObstacleManager.instance.BackToPool(obstacle);
        }
    }
}
