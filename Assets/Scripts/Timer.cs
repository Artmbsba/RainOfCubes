using System;
using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
   public event Action OnComplited;

    public void StartCounter(float dealy)
    {
        StartCoroutine(ReturnToPoolAfterDelay(dealy));
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        var wait = new WaitForSeconds(delay);

        yield return wait;

        OnComplited?.Invoke();
    }
}
