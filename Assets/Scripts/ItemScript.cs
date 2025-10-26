using UnityEngine;

public class ItemScript : MonoBehaviour
{
    [Header("Item Settings")]
    public string ItemName;
    public float weight = 0.05f;
    public float Price;
    public int quantity = 1;

    [Header("UI Settings")]
    public GameObject interactUI; // UI ปุ่ม E

    private Inventory playerInventory;
    private bool isPickedUp = false; // ป้องกันการเก็บซ้ำ

    void Start()
    {
        // แยก UI ออกจาก Item เพื่อไม่ให้หายไปพร้อมกัน
        if (interactUI != null)
        {
            interactUI.transform.SetParent(null, true);
            interactUI.SetActive(false);
        }

        // หา Player และ Inventory อย่างปลอดภัย
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerInventory = player.GetComponent<Inventory>();
        }
        else
        {
            Debug.LogWarning("Player not found! (ItemScript)");
        }
    }

    /// <summary>
    /// โชว์หรือซ่อน UI "กด E เก็บของ"
    /// </summary>
    public void ShowUI(bool state)
    {
        if (interactUI != null && !isPickedUp)
        {
            interactUI.SetActive(state);
        }
    }

    /// <summary>
    /// ฟังก์ชันสำหรับให้ Player เก็บ Item
    /// </summary>
    public void PickUpItem()
    {
        if (isPickedUp) return; // ป้องกันการเก็บซ้ำ
        if (playerInventory == null)
        {
            Debug.LogWarning("Player Inventory not found! (ItemScript)");
            return;
        }

        // สร้าง Item object
        Item newItem = new Item(ItemName, weight, quantity);

        // พยายามเพิ่มลง Inventory
        bool added = playerInventory.AddItem(newItem);
        if (!added) return;

        isPickedUp = true;

        // ซ่อน UI ก่อนลบ object
        if (interactUI != null)
        {
            interactUI.SetActive(false);
            Destroy(interactUI, 0.05f); // หน่วงเล็กน้อยให้แน่ใจว่า UI ปิดแล้ว
        }

        Destroy(gameObject);
    }
}
