using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public GameObject interactUI; // UI ปุ่ม E
    public string ItemName;
    public float weight = 0.05f;     // น้ำหนักต่อชิ้น
    public int quantity = 1;      // จำนวนชิ้น
    private Inventory playerInventory;

    void Start()
    {
        // ถ้า UI เป็น Child ของ Item ให้ตัดออกจาก Parent
        if (interactUI != null)
        {
            interactUI.transform.SetParent(null);
            interactUI.SetActive(false);
        }

        // หา Player และ Inventory
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
            return;

        playerInventory = player.GetComponent<Inventory>();
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
        if (playerInventory == null)
            return;

        Item newItem = new Item(ItemName, weight, quantity);

        bool added = playerInventory.AddItem(newItem);

        if (added)
        {
            if (interactUI != null)
                interactUI.SetActive(false);
            Destroy(gameObject);
        }
    }
}
