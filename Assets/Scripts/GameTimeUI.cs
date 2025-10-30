using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameTimeUI : MonoBehaviour
{
    [Header("Time Settings")]
    public float timeScale = 60f;
    public int startHour = 6;
    public int startMinute = 0;
    public int startDay = 1;

    [Header("UI Settings")]
    public TMP_Text timeText;
    public TMP_Text dateText;

    [Header("2D Light Settings")]
    public Light2D globalLight;
    [Range(0f, 1f)] public float minIntensity = 0.2f;
    [Range(0f, 1f)] public float maxIntensity = 1f;

    [Header("Reference")]
    public ItemSpawner itemSpawner; // 👈 อ้างถึง ItemSpawner

    private float elapsedTime;
    private int currentDay;

    void Start()
    {
        elapsedTime = Mathf.Clamp(startHour, 0, 23) * 3600f + Mathf.Clamp(startMinute, 0, 59) * 60f;
        currentDay = startDay;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime * Mathf.Max(0f, timeScale);

        int totalSeconds = Mathf.FloorToInt(elapsedTime);
        int hour = (totalSeconds / 3600) % 24;
        int minute = (totalSeconds / 60) % 60;
        int newDay = startDay + (totalSeconds / 86400); // 86400 = 24 * 3600

        // 🔄 ตรวจวันเปลี่ยน
        if (newDay != currentDay)
        {
            currentDay = newDay;
            Debug.Log($"New Day: {currentDay}");

            // 👇 ค้นหาและเรียกใช้ทุก ItemSpawner ในฉาก
            ItemSpawner[] spawners = FindObjectsByType<ItemSpawner>(FindObjectsSortMode.None);
            foreach (ItemSpawner spawner in spawners)
            {
                spawner.SendMessage("Start"); // หรือ spawner.SpawnItemsOnStart() ถ้า method เป็น public
            }
            //Debug.Log($"🔁 เรียก ItemSpawner ใหม่ทั้งหมด {spawners.Length} ตัวในฉาก");
        }

        // UI แสดงเวลา + วัน
        if (timeText != null)
            timeText.text = $"{hour:00}:{minute:00}";
        if (dateText != null)
            dateText.text = $"Day {currentDay}";

        // ปรับแสง
        float targetIntensity;
        if (hour >= 5f && hour < 8f)
        {
            float t = (hour - 5f) / 3f;
            targetIntensity = Mathf.Lerp(minIntensity, maxIntensity, t);
        }
        else if (hour >= 8f && hour <= 16f)
        {
            targetIntensity = maxIntensity;
        }
        else if (hour > 16f && hour < 20f)
        {
            float t = (hour - 16f) / 4f;
            targetIntensity = Mathf.Lerp(maxIntensity, minIntensity, t);
        }
        else
        {
            targetIntensity = 0.25f;
        }

        if (globalLight != null)
        {
            globalLight.intensity = Mathf.Lerp(globalLight.intensity, targetIntensity, Time.deltaTime * 2f);
        }
    }
}
