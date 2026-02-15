using UnityEngine;

[CreateAssetMenu(fileName = "PlatformSO", menuName = "Scriptable Objects/PlatformSO")]
public class PlatformSO : ScriptableObject
{
    [Range(10,50)]
    public int length = 10;
    [Range(5,1)]
    public int obstacleFrequency = 3;
    public GameObject plainPlank;
    public GameObject[] obstaclePlanks;
}
