using System;
using UnityEngine;

public class TouchDetector : MonoBehaviour
{
    public event Action TouchHasOccurred;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Platform platform))
        {
            TouchHasOccurred?.Invoke();
        }
    }
}
