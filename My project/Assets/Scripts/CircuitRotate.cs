using UnityEngine;
using System.Collections;

public class CircuitRotate : MonoBehaviour
{
    public float duration = 1.1f;    // total glitch duration
    public float snapInterval = 0.08f; // how often it snaps to a new rotation

    private Quaternion originalRot;

    void OnEnable()
    {
        originalRot = transform.rotation;
        StartCoroutine(DoGlitchSpin());
    }

    IEnumerator DoGlitchSpin()
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += snapInterval;

            // pick a random “digital” 90-degree rotation
            int angle = 90 * Random.Range(0, 4);  // 0, 90, 180, 270

            Quaternion snapRot = Quaternion.Euler(0, 0, angle);

            // tween quickly toward the snap rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, snapRot, 0.8f);

            // pixel-jitter position for glitch effect
            float jx = Random.Range(-0.05f, 0.05f);
            float jy = Random.Range(-0.05f, 0.05f);
            transform.position += new Vector3(jx, jy, 0);

            yield return new WaitForSeconds(snapInterval);
        }

        // final stable rotation (0°)
        transform.rotation = originalRot;
    }
}

