using UnityEngine;

public class AntiVirusDrone : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 3f;

    public bool isFrozen = false;

    private Vector3 starPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        starPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float offset = Mathf.PingPong(Time.time * speed, distance * 2) - distance;
        transform.position = new Vector3(starPos.x + offset, starPos.y, starPos.z);
    }
}
