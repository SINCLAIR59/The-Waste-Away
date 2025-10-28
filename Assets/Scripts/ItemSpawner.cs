using UnityEngine;

[System.Serializable]
public class SpawnItem
{
    public GameObject prefab;      // Prefab ของ Item
    [Range(0f, 1f)]
    public float spawnRate = 1f;   // โอกาสเกิดของ Item (0.0 - 1.0)
}

public class ItemSpawner : MonoBehaviour
{
    [Header("ตั้งค่า Item ที่จะสุ่ม")]
    public SpawnItem[] items;      // รายการ Item ที่จะสุ่มจาก

    [Header("พื้นที่สุ่ม (ใช้ GameObject ที่มี SpriteRenderer หรือ Collider2D)")]
    public GameObject spawnArea;   // กำหนดพื้นที่ที่จะใช้สุ่ม

    [Header("จำนวน Item ที่จะสุ่มวาง")]
    public int spawnCount = 10;

    private Vector2 spawnAreaMin;
    private Vector2 spawnAreaMax;

    void Start()
    {
        // คำนวณขนาดของพื้นที่จาก spawnArea
        if (spawnArea != null)
        {
            CalculateSpawnArea();
        }
        else
        {
            Debug.LogWarning("⚠️ Spawn Area ยังไม่ได้ตั้งค่า — จะใช้ค่าเริ่มต้นแทน");
            spawnAreaMin = new Vector2(-10, -5);
            spawnAreaMax = new Vector2(10, 5);
        }

        SpawnItemsOnStart();
    }

    void CalculateSpawnArea()
    {
        // 1️⃣ ถ้า spawnArea มี SpriteRenderer
        SpriteRenderer sr = spawnArea.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Bounds bounds = sr.bounds;
            spawnAreaMin = bounds.min;
            spawnAreaMax = bounds.max;
            return;
        }

        // 2️⃣ ถ้า spawnArea มี Collider2D
        Collider2D col = spawnArea.GetComponent<Collider2D>();
        if (col != null)
        {
            Bounds bounds = col.bounds;
            spawnAreaMin = bounds.min;
            spawnAreaMax = bounds.max;
            return;
        }

        // 3️⃣ ถ้าไม่มีทั้งคู่ ให้ใช้ตำแหน่งของ spawnArea เป็นจุดกลาง
        Vector2 center = spawnArea.transform.position;
        spawnAreaMin = center - new Vector2(10, 5);
        spawnAreaMax = center + new Vector2(10, 5);

        Debug.LogWarning("⚠️ spawnArea ไม่มี SpriteRenderer หรือ Collider2D ใช้ขอบเขตจำลองแทน");
    }

    void SpawnItemsOnStart()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject selectedItem = GetRandomItem();
            if (selectedItem != null)
            {
                Vector2 randomPos = new Vector2(
                    Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                    Random.Range(spawnAreaMin.y, spawnAreaMax.y)
                );

                Instantiate(selectedItem, randomPos, Quaternion.identity);
            }
        }
    }

    GameObject GetRandomItem()
    {
        float totalRate = 0f;
        foreach (var item in items)
            totalRate += item.spawnRate;

        float randomPoint = Random.value * totalRate;
        float cumulative = 0f;

        foreach (var item in items)
        {
            cumulative += item.spawnRate;
            if (randomPoint <= cumulative)
                return item.prefab;
        }

        return null;
    }
}
