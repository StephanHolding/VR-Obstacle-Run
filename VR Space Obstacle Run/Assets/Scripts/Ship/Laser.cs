using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public int damageAmount;
    public int laserSpeed;
    public float laserLifeTime;
    public bool available;

    private float timer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            print("Obstacle");
            if (other.gameObject.tag == "Enemy")
            {
                print("enemy");
                other.GetComponent<Enemy>().TakeDamage(damageAmount);
            }

            ReturnToPool();
        }
    }

    public IEnumerator LifeTimer(float time)
    {
        timer = time;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        ReturnToPool();
    }

    public void TogglePhysics(bool toggle)
    {
        transform.GetComponent<Rigidbody>().detectCollisions = toggle;
        Collider[] colliders = transform.GetComponents<Collider>();
        foreach (Collider c in colliders)
        {
            c.enabled = toggle;
        }
    }

    public void ReturnToPool()
    {
        available = true;
        transform.position = new Vector3(0, 0, -500);
        TogglePhysics(false);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
