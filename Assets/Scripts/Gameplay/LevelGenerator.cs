using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;

public class LevelGenerator : NetworkBehaviour
{
    [SerializeField] PlatformSO platformData;
    [SerializeField] Transform startPosition;
    private List<GameObject> chunks = new();

    void Start()
    {
        GameManager.instance.OnGameStartEvent += GeneratePlanks;
    }
    void GeneratePlanks(object sender,EventArgs e)
    {
        if(!IsServer) return;
        SpawnPlankServerRpc();
    }
    [Rpc(SendTo.Server)]
    private void SpawnPlankServerRpc()
    {
        if (chunks.Count > 1)
        {
            foreach(GameObject chunk in chunks)
            {
                Destroy(chunk);
            }
        }
        for(int i = 0; i < platformData.length; i++)
        {
            GameObject prefab;
            if(i == 0)
            {
                prefab = platformData.firstPlank;
            }
            else if (i == platformData.length - 1)
            {
                prefab = platformData.lastPlank;
            }
            else if (i % platformData.obstacleFrequency != 0){
                prefab = platformData.plainPlank;
            }
            else{
                int j = UnityEngine.Random.Range(0,platformData.obstaclePlanks.Length);
                prefab = platformData.obstaclePlanks[j];
            }
            Vector2 _position = i == 0 ? startPosition.position : chunks[chunks.Count - 1].GetComponent<ChunkBehaviour>().spawnPoint.position;
            GameObject plank = Instantiate(prefab,_position,Quaternion.identity);
            plank.GetComponent<NetworkObject>().Spawn(true);
            chunks.Add(plank);
        }
    }
}
