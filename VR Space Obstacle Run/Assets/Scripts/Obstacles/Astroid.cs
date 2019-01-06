using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : Obstacle {

    private void FixedUpdate()
    {
        if (move)
        {
            Move();
        }
    }

    public void ToggleSpin(bool Toggle)
    {
        Rigidbody r = GetComponent<Rigidbody>();

        if (Toggle)
        {
            r.angularVelocity = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        }
        else
        {
            r.angularVelocity = Vector3.zero;
        }
    }

    public override void Move()
    {
        base.Move();
        transform.Translate(transform.forward * 2 * Time.deltaTime, Space.Self);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 11)
        OnHit(collision.transform.GetComponent<Ship>());
    }
}
