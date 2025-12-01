using UnityEngine;

public class Layer3Hazard : MonoBehaviour
{
    public float moveSpeed = 0.7f;
    public bool Frozen = false;

    private Vector3 driftDir;
    private float life = 7f;

    void Start()
    {
        driftDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;

        transform.Rotate(0, 0, Random.Range(0f, 360f));
    }

    void Update()
    {
        if (Frozen) return;

        // drifting movement
        transform.position += driftDir * moveSpeed * Time.deltaTime;

        // micro-rotation
        transform.Rotate(0, 0, Mathf.Sin(Time.time * 2f) * 0.3f);

        // cleanup
        life -= Time.deltaTime;
        if (life <= 0)
            Destroy(gameObject);
    }
}
