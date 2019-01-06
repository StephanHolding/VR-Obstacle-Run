using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Obstacle {

    public float health;

    internal Material dissolveMaterial;
    private float dissolveTimer = 0;

    private void Awake()
    {
        dissolveMaterial = GetComponent<MeshRenderer>().material;
    }

    private void FixedUpdate()
    {
        if (move)
        {
            Move();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnHit(collision.transform.GetComponent<Ship>());
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    public IEnumerator Die()
    {
        EffectsManager.instance.PlayParticle(EffectsManager.instance.FindParticle("Big Explosion"), transform.name + " Big Explosion", transform.position, Quaternion.identity);

        while (dissolveTimer < 1)
        {
            dissolveTimer += Time.deltaTime / 2;
            dissolveMaterial.SetFloat("_DissolvePercentage", dissolveTimer);
            yield return null;
        }

        ObstacleManager.instance.BackToPool(this);
    }


}
