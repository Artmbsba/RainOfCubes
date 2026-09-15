using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    [SerializeField] private Color _color = Color.white;
    [SerializeField] private float _minLifeTime = 2.0f;
    [SerializeField] private float _maxLifeTime = 5.0f;

    private ObjectPool<GameObject> _pool;
    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private bool _hasAlreadyBeenTouch;

    public void SetPool(ObjectPool<GameObject> pool)
    {
        _pool = pool;
    }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _hasAlreadyBeenTouch = false;
        _renderer.material.color = _color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasAlreadyBeenTouch)
            return;

        _renderer.material.color = Random.ColorHSV();
        _hasAlreadyBeenTouch = true;

        StartCoroutine(ReturnToPoolAfterDelay());
    }

    private IEnumerator ReturnToPoolAfterDelay()
    {
        float lifeTime = Random.Range(_minLifeTime, _maxLifeTime);
        yield return new WaitForSeconds(lifeTime);

        _pool.Release(gameObject);
    }
}
