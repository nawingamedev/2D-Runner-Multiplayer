using System;
using System.Collections.Generic;
using System.Linq;
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
        SpawnPlankServerRpc();
    }
    [Rpc(SendTo.Server)]
    private void SpawnPlankServerRpc()
    {
        for(int i = 0; i < platformData.length; i++)
        {
            GameObject prefab;
            if (i != 0 && i % platformData.obstacleFrequency != 0){
                prefab = platformData.plainPlank;
            }
            else{
                int j = UnityEngine.Random.Range(0,platformData.obstaclePlanks.Length);
                prefab = platformData.obstaclePlanks[j];
            }
            Vector2 _position;
            if (chunks.Count == 0)
            {
                _position = startPosition.position;
            }
            else
            {
                _position = chunks[chunks.Count - 1].GetComponent<ChunkBehaviour>().spawnPoint.position;
            }
            GameObject plank = Instantiate(prefab,_position,Quaternion.identity);
            plank.GetComponent<NetworkObject>().Spawn(true);
            chunks.Add(plank);
        }
    }
}
