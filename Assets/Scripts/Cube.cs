using System;
using UnityEngine;

[RequireComponent(typeof(TouchDetector))]
[RequireComponent(typeof(Timer))]
[RequireComponent(typeof(ColorChanger))]
[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    [SerializeField] private Color _color = Color.white;
    [SerializeField] private float _minLifeTime = 2.0f;
    [SerializeField] private float _maxLifeTime = 5.0f;

    private ColorChanger _colorController;
    private TouchDetector _touchController;
    private Timer _timer;
    private Rigidbody _rigidbody;
    private Renderer _renderer;
    private bool _hasAlreadyBeenTouch;

    public event Action<Cube> LifeTimeEnded;

    public Rigidbody GetRigidbody => _rigidbody;

    private void Awake()
    {
        _touchController = GetComponent<TouchDetector>();
        _timer = GetComponent<Timer>();
        _renderer = GetComponent<Renderer>();
        _colorController = GetComponent<ColorChanger>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _timer.OnComplited += CompleteLifeTime;
        _touchController.TouchHasOccurred += OnTouchOccurred;
        _colorController.SetDefaultColor(_renderer, _color);
        _hasAlreadyBeenTouch = false;
    }

    private void OnDisable()
    {
        _timer.OnComplited -= CompleteLifeTime;
        _touchController.TouchHasOccurred -= OnTouchOccurred;
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

        _timer.StartCounter(lifeTime);
    }

    private void CompleteLifeTime()
    {
        LifeTimeEnded?.Invoke(this);
    }
}
