using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour {

    public int damageAmount;
    public Vector2 scaling;
    public float moveSpeed { get { return ObstacleManager.instance.obstacleMoveSpeed; } }
    public bool move;

    public virtual void OnHit(Ship gotHit)
    {
        gotHit.TakeDamage(damageAmount);
    }

    public virtual void Move()
    {
        transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
    }
    
    public virtual void TogglePhysics(bool toggle)
    {
        transform.GetComponent<Rigidbody>().detectCollisions = toggle;
        Collider[] colliders = transform.GetComponents<Collider>();
        foreach(Collider c in colliders)
        {
            c.enabled = toggle;
        }
    }
}
