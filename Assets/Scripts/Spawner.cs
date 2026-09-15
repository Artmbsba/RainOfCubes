using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private Collider _mainPlatformCollaider;
    [SerializeField] private float _spawnHeight = 21;
    [SerializeField] private float _repeatRate = 1;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 10;

    private ObjectPool<GameObject> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_cubePrefab),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }
      
    private void ActionOnGet(GameObject obj)
    {
        if (obj.TryGetComponent(out Cube cube))
            cube.SetPool(_pool);

        obj.transform.position = DefinePosition();
        obj.transform.rotation = Quaternion.identity;
        Rigidbody ridgidbody = obj.GetComponent<Rigidbody>();
        ridgidbody.linearVelocity = Vector3.zero;
        ridgidbody.angularVelocity = Vector3.zero;

        obj.SetActive(true);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _repeatRate);
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private Vector3 DefinePosition()
    {
        Bounds bounds = _mainPlatformCollaider.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, bounds.min.y + _spawnHeight, randomZ);
    }
}
