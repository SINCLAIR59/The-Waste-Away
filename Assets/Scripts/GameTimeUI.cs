using UnityEngine;
using TMPro;

public class GameTimeUI : MonoBehaviour
{
    [Header("Time Settings")]
    [Tooltip("1 วินาทีจริง = กี่วินาทีในเกม")]
    public float timeScale = 60f;
    public int startHour = 6;
    public int startMinute = 0;

    [Header("UI Settings")]
    public TMP_Text timeText;

    private float elapsedTime; // เวลารวมเป็นวินาที

    void Start()
    {
        // กำหนดเวลาเริ่มต้น
        elapsedTime = Mathf.Clamp(startHour, 0, 23) * 3600f + Mathf.Clamp(startMinute, 0, 59) * 60f;
    }

    void Update()
    {
        // อัปเดตเวลาแบบ frame-independent
        elapsedTime += Time.deltaTime * Mathf.Max(0f, timeScale);

        int totalSeconds = Mathf.FloorToInt(elapsedTime);
        int hour = (totalSeconds / 3600) % 24;
        int minute = (totalSeconds / 60) % 60;

        // อัปเดต UI ถ้ามี
        if (timeText != null)
            timeText.text = $"{hour:00}:{minute:00}";
    }
}
