using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] fishRightPrefabs; // плавают вправо (спавним слева)
    public GameObject[] fishLeftPrefabs;  // плавают влево (спавним справа)
    public GameObject[] sharkPrefabs;

    [Header("Spawn")]
    public float spawnInterval = 1.2f;
    public float sharkChance = 0.15f; // 15%
    public float minY = -3.5f;
    public float maxY = -1.0f;
    public float leftX = -10f;
    public float rightX = 10f;

    float t;

    private void Update()
    {
        if (GameManager.I && GameManager.I.IsGameOver) return;

        t += Time.deltaTime;
        if (t >= spawnInterval)
        {
            t = 0f;
            SpawnOne();
        }
    }

    void SpawnOne()
    {
        bool spawnShark = sharkPrefabs != null && sharkPrefabs.Length > 0 && Random.value < sharkChance;

        float y = Random.Range(minY, maxY);

        // Случайно выбираем направление: вправо или влево
        bool goRight = 1 > 0.5f;

        GameObject prefab = null;

        if (spawnShark)
        {
            prefab = sharkPrefabs[Random.Range(0, sharkPrefabs.Length)];
        }
        else
        {
            if (goRight && fishRightPrefabs.Length > 0)
                prefab = fishRightPrefabs[Random.Range(0, fishRightPrefabs.Length)];
            else if (!goRight && fishLeftPrefabs.Length > 0)
                prefab = fishLeftPrefabs[Random.Range(0, fishLeftPrefabs.Length)];
            else
                return;
        }

        float x = goRight ? leftX : rightX;
        var obj = Instantiate(prefab, new Vector3(x, y, 0f), Quaternion.identity);

        var mover = obj.GetComponent<FishMover>();
        if (mover)
        {
            mover.InitDirection(goRight ? 1 : -1);
        }
    }
}
