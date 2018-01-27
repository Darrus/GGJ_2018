using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField]
    GameObject enemyPrefab;
    [SerializeField]
    Tilemap tilemap;

    public List<Units> enemyList = new List<Units>();

    public void Spawn()
    {
        GameObject enemy = Instantiate<GameObject>(enemyPrefab);
        Units unit = enemy.GetComponent<Units>();
        enemy.transform.SetParent(transform);

        int halvedSizeX = tilemap.size.x >> 1;
        int halvedSizeY = tilemap.size.y >> 1;

        // 0 = North, 1 = South, 2 = East, 3 = West
        int spawnDir = Random.Range(0, 3);
        int y = 0;
        int x = 0;

        switch (spawnDir)
        {
            case 0:
                y = halvedSizeY - 1;
                x = Random.Range(halvedSizeX * -1, halvedSizeX - 1);
                break;
            case 1:
                y = halvedSizeY * -1;
                x = Random.Range(halvedSizeX * -1, halvedSizeX - 1);
                break;
            case 2:
                y = Random.Range(halvedSizeY * -1, halvedSizeY - 1);
                x = halvedSizeX - 1;
                break;
            case 3:
                y = Random.Range(halvedSizeY * -1, halvedSizeY - 1);
                x = halvedSizeX * -1;
                break;
        }

        Vector3 position = new Vector3(x, y);
        enemy.transform.position = position;
    }
}
