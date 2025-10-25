using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Inventory playerInventory; // ลาก Player Inventory ลงใน Inspector
    public TMP_Text weightText;       // ลาก TextMeshProUGUI ลงใน Inspector

    void Update()
    {
        if (playerInventory != null && weightText != null)
        {
            weightText.text = $"Weight: {playerInventory.CurrentWeight():0.0} / {playerInventory.maxWeight} kg";
        }
        if (weightText != null)
        {
            weightText.text = "Test UI"; // ลองเปลี่ยนข้อความดู
        }
    }
}
