using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public GameObject interactUI; // UI ปุ่ม E

    void Start()
    {
        // ถ้า UI เป็น Child ของ Item ให้ตัดออกจาก Parent
        if (interactUI != null)
        {
            interactUI.transform.SetParent(null);
            interactUI.SetActive(false);
        }
    }

    // ฟังก์ชันให้ Player เรียกเพื่อโชว์หรือซ่อน UI
    public void ShowUI(bool state)
    {
        if (interactUI != null)
            interactUI.SetActive(state);
    }

    // ฟังก์ชันให้ Player เรียกเพื่อเก็บ Item
    public void PickUpItem()
    {
        Debug.Log($"เก็บ {name} แล้ว!");
        if (interactUI != null)
            interactUI.SetActive(false);
        Destroy(gameObject);
    }
}
