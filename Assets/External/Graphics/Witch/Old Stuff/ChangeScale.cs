using UnityEngine;
using System.Collections;

public class ChangeScale : MonoBehaviour
{
    private IEnumerator inst = null;
    public void AdjustTime(float time)
    {
        if (inst != null)
        {
            StopCoroutine(inst);
        }
        inst = adjustTime(time);
        StartCoroutine(inst);
    }

    private IEnumerator adjustTime(float targetTime)
    {
        for (int i = 0; i < 10; i++)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, targetTime, 0.2f);
            yield return new WaitForFixedUpdate();
        }
        Time.timeScale = targetTime;
        inst = null;
        yield break;
    }

    public void AdjustTimeInstantly(float time)
    {
        if (inst != null)
        {
            StopCoroutine(inst);
            inst = null;
        }
        Time.timeScale = time;
    }
}
