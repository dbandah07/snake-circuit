using UnityEngine;

public class PacketSpawner : MonoBehaviour
{
    public GameObject packetPrefab;
    public Vector2 spawnAreaMin = new Vector2 (0, 0);
    public Vector2 spawnAreaMax = new Vector2 (0, 0);
// Start is called once before the first execution of Update after the MonoBehaviour is created
void Start()
    {
        SpawnSinglePacket();
    }

    public void SpawnSinglePacket()
    {
        float x = Mathf.Round(Random.Range(spawnAreaMin.x, spawnAreaMax.x));
        float y = Mathf.Round(Random.Range(spawnAreaMin.y, spawnAreaMax.y));
        Vector3 spawnPos = new Vector3(x, y, 0f);
        Instantiate(packetPrefab, spawnPos, Quaternion.identity);
    }
}
