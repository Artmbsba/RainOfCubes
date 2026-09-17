using System;
using System.Collections;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public void StartCounter(float dealy, Action onComplete)
    {
        StartCoroutine(ReturnToPoolAfterDelay(dealy, onComplete));
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay, Action onComplete)
    {
        var wait = new WaitForSeconds(delay);

        yield return wait;

        onComplete?.Invoke();
    }
}
