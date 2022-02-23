using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    public GameObject enemy;
    public int numberOfEnemiesSpawnStart;
    public int minSpawnDistance;
    public int maxSpawnDistance;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfEnemiesSpawnStart; i++)
        {
            var temp = Instantiate(enemy, transform);
            temp.transform.position += new Vector3(Random.Range(minSpawnDistance, maxSpawnDistance), 0, Random.Range(minSpawnDistance, maxSpawnDistance));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Vector3.one * (Mathf.Abs(minSpawnDistance) + maxSpawnDistance));
    }
}
