using UnityEngine;

public class PacketSpawner : MonoBehaviour
{
    public GameObject packetPrefab;
    public Vector2 spawnAreaMin = new Vector2(-7.3f, -3.0f);
    public Vector2 spawnAreaMax = new Vector2(7.1f, 3.0f);
// Start is called once before the first execution of Update after the MonoBehaviour is created
void Start()
    {
        SpawnSinglePacket();
    }

    public void SpawnSinglePacket()
    {
        float safeMinX = -7.3f;
        float safeMaxX = 7.1f;

        float safeMinY = -3.0f;
        float safeMaxY = 3.0f;

        float x = Mathf.Round(Random.Range(safeMinX, safeMaxX));
        float y = Mathf.Round(Random.Range(safeMinY, safeMaxY));

        Vector3 spawnPos = new Vector3(x, y, 0f);
        Instantiate(packetPrefab, spawnPos, Quaternion.identity);
    }
}
