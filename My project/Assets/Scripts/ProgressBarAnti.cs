using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBarAnti : MonoBehaviour
{
    public Image fillBar;

    public IEnumerator PlayScanBar(float duration, float targetFill)
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            fillBar.fillAmount = Mathf.Lerp(0, targetFill, t / duration);
            yield return null;
        }

        fillBar.fillAmount = targetFill;
    }

}
