using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private int minX;
    [SerializeField] private int maxX;
    [SerializeField] private int minZ;
    [SerializeField] private int maxZ;

    public void Spawn(GameObject spawnObject)
    {
        var x = Random.Range(minX, maxX)/10;
        var z = Random.Range(minZ, maxZ)/10;

        Vector3 spawnPosition = new Vector3(x, 0, z);
        Instantiate(spawnObject, spawnPosition, Quaternion.identity);
    }
}
