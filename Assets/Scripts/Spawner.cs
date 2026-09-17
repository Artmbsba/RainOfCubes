using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Counter))]
public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private Collider _mainPlatformCollaider;
    [SerializeField] private float _spawnHeight = 21;
    [SerializeField] private float _delay = 1;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 10;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_cubePrefab),
            actionOnGet: (cube) => PrepareCube(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => Destroy(cube),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(DelaySpawnCubes());
    }

    private IEnumerator DelaySpawnCubes()
    {
        var wait = new WaitForSeconds(_delay);

        while (enabled)
        {
            yield return wait;

            _pool.Get();
        }
    }

    private void PrepareCube(Cube newCube)
    {
        newCube.LifeTimeEnded += OnCubeLifeTimeEnded;

        newCube.transform.position = DefinePosition();
        newCube.transform.rotation = Quaternion.identity;
        Rigidbody ridgidbody = newCube.GetRigidbody();
        ridgidbody.linearVelocity = Vector3.zero;
        ridgidbody.angularVelocity = Vector3.zero;

        newCube.gameObject.SetActive(true);
    }

    private void OnCubeLifeTimeEnded(Cube cube)
    {
        cube.LifeTimeEnded -= OnCubeLifeTimeEnded;

        _pool.Release(cube);
    }

    private Vector3 DefinePosition()
    {
        Bounds bounds = _mainPlatformCollaider.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, bounds.min.y + _spawnHeight, randomZ);
    }
}
