using UnityEngine;

public class HardFreeze : MonoBehaviour
{
    private MonoBehaviour movementScript;
    private Rigidbody2D rb;

    void Start()
    {
        movementScript = GetComponent<MonoBehaviour>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Freeze()
    {
        if (movementScript != null) movementScript.enabled = false;
        if (rb != null) rb.simulated = false;
    }

    public void Unfreeze()
    {
        if (movementScript != null) movementScript.enabled = true;
        if (rb != null) rb.simulated = true;
    }
}
