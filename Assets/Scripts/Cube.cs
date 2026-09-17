using System;
using UnityEngine;

[RequireComponent(typeof(TouchController))]
[RequireComponent(typeof(Counter))]
[RequireComponent(typeof(ColorController))]
[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    [SerializeField] private Color _color = Color.white;
    [SerializeField] private float _minLifeTime = 2.0f;
    [SerializeField] private float _maxLifeTime = 5.0f;

    public event Action<Cube> LifeTimeEnded;

    private ColorController _colorController;
    private TouchController _touchController;
    private Counter _counter;
    private Rigidbody _rigidbody;
    private Renderer _renderer;
    private bool _hasAlreadyBeenTouch;

    private void Awake()
    {
        _touchController = GetComponent<TouchController>();
        _counter = GetComponent<Counter>();
        _renderer = GetComponent<Renderer>();
        _colorController = GetComponent<ColorController>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _touchController.TouchHasOccurred += OnTouchOccurred;
        _colorController.SetDefaultColor(_renderer, _color);
        _hasAlreadyBeenTouch = false;
    }

    private void OnDisable()
    {
        _touchController.TouchHasOccurred -= OnTouchOccurred;
    }

    public Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }

    private void OnTouchOccurred()
    {
        if (_hasAlreadyBeenTouch)
            return;

        _hasAlreadyBeenTouch = true;
        _colorController.ChangingColorOfCube(_renderer);

        StartLifeCounter();
    }

    private void StartLifeCounter()
    {
        float lifeTime = UnityEngine.Random.Range(_minLifeTime, _maxLifeTime);

        _counter.StartCounter(lifeTime, CompleteLifeTime);
    }

    private void CompleteLifeTime()
    {
        LifeTimeEnded?.Invoke(this);
    }
}
