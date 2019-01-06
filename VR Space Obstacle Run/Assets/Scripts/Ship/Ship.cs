using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    public static Ship playerShip;

    [Header("Ship References")]
    public ShipController_VR shipControl;
    public VR_Player vR_Player;

    [Header("Ship Proporties")]
    public int health;
    public GameObject laserPrefab;
    public Transform[] laserpoints;
    public List<Laser> laserPool = new List<Laser>();
    public int laserAmountInPool;

    private bool cannotShoot;

    private void Awake()
    {
        if (playerShip == null)
        {
            playerShip = this;
        }
        else
        {
            Debug.LogError("There can be only one player ship in the scene");
            Destroy(gameObject);
        }

        FillLaserPool();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        CheckForDeath();
    }
    private void CheckForDeath()
    {
        if (health <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    private void FillLaserPool()
    {
        for (int i = 0; i <= laserAmountInPool; i++)
        {
            GameObject newObject = Instantiate(laserPrefab, new Vector3(0, 0, -500), laserPrefab.transform.rotation);
            Laser laser = newObject.GetComponent<Laser>();
            laser.TogglePhysics(false);
            laserPool.Add(laser);
        }
    }

    public void Shoot()
    {
        if (!cannotShoot)
        {
            foreach (Transform laserpoint in laserpoints)
            {
                Laser laser = GetAvailableLaser();
                laser.transform.position = laserpoint.transform.position;
                laser.TogglePhysics(true);
                laser.transform.rotation = laserpoint.rotation;
                laser.GetComponent<Rigidbody>().velocity = laser.transform.up * laser.laserSpeed;
                StartCoroutine(laser.LifeTimer(laser.laserLifeTime));
            }
        }
    }

    public void LockControls(bool toggle)
    {
        shipControl.LockSteering(toggle);
        cannotShoot = toggle;
    }

    public void SetInteractionMode(InteractionHandler.InteractionMode interactionMode)
    {
        vR_Player.interactionMode = interactionMode;
    }

    private Laser GetAvailableLaser()
    {
        for (int i = 0; i < laserPool.Count; i++)
        {
            if (laserPool[i].available)
            {
                laserPool[i].available = false;
                return laserPool[i];
            }
        }

        return null;
    }
}
