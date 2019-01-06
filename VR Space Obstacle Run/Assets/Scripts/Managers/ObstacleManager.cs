using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour {

    public static ObstacleManager instance;

    [Header("Instantiation Proporties")]
    public GameObject[] obstaclePrefabs;
    public Vector3 poolSpawnposition;
    private Vector3 editableSpawnPosition;

    [Header("Obstacle Proporties")]
    public List<Obstacle> activeObstacles = new List<Obstacle>();
    public List<Obstacle> objectPool = new List<Obstacle>();
    public float obstacleMoveSpeed;
    public Vector2 obstacleRange;
    public int minObstacleAmount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        FillObjectPool(15);

        for (int i = 0; i < minObstacleAmount; i++)
        {
            NewObstacle();
        }

        MoveObstacles(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            MoveObstacles(true);
        }

        if (activeObstacles.Count < minObstacleAmount)
        {
            NewObstacle();
        }
    }

    public void MoveObstacles(bool toggle)
    {
        foreach (Obstacle obstacle in activeObstacles)
        {
            obstacle.move = toggle;
            
            if (obstacle.GetComponent<Astroid>() != null)
            {
                obstacle.GetComponent<Astroid>().ToggleSpin(toggle);
            }
        }
    }

    public void NewObstacle()
    {
        int astroidToSpawn = Random.Range(0, objectPool.Count);
        Obstacle toMove = objectPool[astroidToSpawn];

        Vector2 spawnPosition = new Vector2(Random.Range(-obstacleRange.x, obstacleRange.x), Random.Range(-obstacleRange.y, obstacleRange.y));

        if (activeObstacles.Count > 0)
        {
            toMove.transform.position =  new Vector3(spawnPosition.x, spawnPosition.y, activeObstacles[activeObstacles.Count - 1].transform.position.z + 50);
        }
        else
        {
            toMove.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 100);
        }

        objectPool.Remove(toMove);
        activeObstacles.Add(toMove);

        toMove.TogglePhysics(true);
        toMove.move = true;
    }

    public void BackToPool(Obstacle toMove)
    {
        toMove.transform.position = poolSpawnposition;
        toMove.TogglePhysics(false);
        toMove.move = false;

        if (toMove.GetComponent<Enemy>() != null)
        {
            toMove.GetComponent<Enemy>().dissolveMaterial.SetFloat("_DissolvePercentage", 0);
        }

        activeObstacles.Remove(toMove);
        objectPool.Add(toMove);
    }

    public void FillObjectPool(int amountPerObject)
    {
        editableSpawnPosition = poolSpawnposition;

        for (int p = 0; p < obstaclePrefabs.Length; p++)
        {
            //editableSpawnPosition.x += 50;
            //editableSpawnPosition.z = poolSpawnposition.z;
            for (int a = 0; a <= amountPerObject; a++)
            {
                GameObject newObject = Instantiate(obstaclePrefabs[p], editableSpawnPosition, obstaclePrefabs[p].transform.rotation);
                Obstacle obstacle = newObject.GetComponent<Obstacle>();

                float scale = Random.Range(obstacle.scaling.x, obstacle.scaling.y);
                newObject.transform.localScale = new Vector3(scale, scale, scale);

                obstacle.TogglePhysics(false);
                objectPool.Add(newObject.GetComponent<Obstacle>());
            }
        }
    }

    public void DestroyObstacle(Obstacle toDestroy)
    {
        activeObstacles.Remove(toDestroy);
        Destroy(toDestroy.gameObject);
    }
}
