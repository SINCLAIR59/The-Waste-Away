using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameTimeUI : MonoBehaviour
{
    [Header("Time Settings")]
    [Tooltip("1 วินาทีจริง = กี่วินาทีในเกม")]
    public float timeScale = 60f;
    [Range(0, 23)] public int startHour = 6;
    [Range(0, 59)] public int startMinute = 0;

    [Header("UI Settings")]
    public TMP_Text timeText;

    [Header("2D Light Settings")]
    public Light2D globalLight;
    [Range(0f, 1f)] public float minIntensity = 0.2f;
    [Range(0f, 1f)] public float maxIntensity = 1f;

    private float elapsedTime; // เวลารวมเป็นวินาที

    void Start()
    {
        elapsedTime = startHour * 3600f + startMinute * 60f;
    }

    void Update()
    {
        // อัปเดตเวลาแบบ frame-independent
        elapsedTime += Time.deltaTime * Mathf.Max(0f, timeScale);

        int totalSeconds = Mathf.FloorToInt(elapsedTime);
        int hour = (totalSeconds / 3600) % 24;
        int minute = (totalSeconds / 60) % 60;

        // อัปเดต UI เวลา
        if (timeText != null)
            timeText.text = $"{hour:00}:{minute:00}";

        // คำนวณ target intensity ตามช่วงเวลา
        float targetIntensity = hour switch
        {
            >= 5 and < 8 => Mathf.Lerp(minIntensity, maxIntensity, (hour - 5f) / 3f),   // เช้า 05:00 - 08:00
            >= 8 and <= 16 => maxIntensity,                                              // กลางวัน
            > 16 and < 20 => Mathf.Lerp(maxIntensity, minIntensity, (hour - 16f) / 4f),  // เย็น 16:00 - 20:00
            _ => minIntensity                                                                    // กลางคืน 20:00 - 05:00
        };

        // ปรับแสงแบบ smooth
        if (globalLight != null)
            globalLight.intensity = Mathf.Lerp(globalLight.intensity, targetIntensity, Time.deltaTime * 2f);
    }
}
