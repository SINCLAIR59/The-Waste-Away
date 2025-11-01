using UnityEngine;

[System.Serializable]
public class SpawnItem
{
    [Tooltip("Prefab ของ Item ที่จะสุ่ม")]
    public GameObject prefab;

    [Range(0f, 1f), Tooltip("อัตราโอกาสเกิด (0.0 - 1.0)")]
    public float spawnRate = 1f;
}

public class ItemSpawner : MonoBehaviour
{
    [Header("ตั้งค่า Item ที่จะสุ่ม")]
    [SerializeField] private SpawnItem[] items;

    [Header("พื้นที่สุ่ม (SpriteRenderer หรือ Collider2D)")]
    [SerializeField] private GameObject spawnArea;

    [Header("จำนวน Item ที่จะสุ่มวาง")]
    [SerializeField, Min(1)] private int spawnCount = 10;

    private Vector2 spawnAreaMin;
    private Vector2 spawnAreaMax;

    void Start()
    {
        if (items == null || items.Length == 0)
        {
            Debug.LogWarning("⚠️ ไม่มี Item ที่จะสุ่ม (items array ว่าง)");
            return;
        }

        CalculateSpawnArea();
        SpawnItemsOnStart();
    }

    private void CalculateSpawnArea()
    {
        if (spawnArea == null)
        {
            Debug.LogWarning("⚠️ Spawn Area ยังไม่ได้ตั้งค่า — ใช้ค่าเริ่มต้นแทน");
            spawnAreaMin = new Vector2(-10, -5);
            spawnAreaMax = new Vector2(10, 5);
            return;
        }

        // 1️⃣ ถ้ามี SpriteRenderer
        if (spawnArea.TryGetComponent<SpriteRenderer>(out var sr))
        {
            Bounds bounds = sr.bounds;
            spawnAreaMin = bounds.min;
            spawnAreaMax = bounds.max;
            return;
        }

        // 2️⃣ ถ้ามี Collider2D
        if (spawnArea.TryGetComponent<Collider2D>(out var col))
        {
            Bounds bounds = col.bounds;
            spawnAreaMin = bounds.min;
            spawnAreaMax = bounds.max;
            return;
        }

        // 3️⃣ ไม่มีทั้งคู่ → ใช้ตำแหน่งกลางจำลอง
        Vector2 center = spawnArea.transform.position;
        spawnAreaMin = center - new Vector2(10, 5);
        spawnAreaMax = center + new Vector2(10, 5);
        Debug.LogWarning("⚠️ spawnArea ไม่มี SpriteRenderer หรือ Collider2D ใช้ขอบเขตจำลองแทน");
    }

    // ✅ เปลี่ยนจาก private → public เพื่อให้ GameTimeUI เรียกได้
    public void SpawnItemsOnStart()
    {
        int spawned = 0;

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject selectedItem = GetRandomItem();
            if (selectedItem == null) continue;

            Vector3 randomPos = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                Random.Range(-300,300)
            );

            Instantiate(selectedItem, randomPos, Quaternion.identity);
            spawned++;
        }

        Debug.Log($"สร้าง Item แล้วทั้งหมด {spawned}/{spawnCount} ชิ้น");
    }

    private GameObject GetRandomItem()
    {
        if (items == null || items.Length == 0) return null;

        float totalRate = 0f;
        foreach (var item in items)
            totalRate += Mathf.Max(0, item.spawnRate);

        if (totalRate <= 0f)
        {
            Debug.LogWarning("⚠️ ค่า spawnRate ของทุก item = 0 ทั้งหมด");
            return null;
        }

        float randomPoint = Random.value * totalRate;
        float cumulative = 0f;

        foreach (var item in items)
        {
            cumulative += Mathf.Max(0, item.spawnRate);
            if (randomPoint <= cumulative)
                return item.prefab;
        }

        return null;
    }
}
