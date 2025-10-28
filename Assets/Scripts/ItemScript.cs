using UnityEngine;

public class ItemScript : MonoBehaviour
{
    [Header("Item Settings")]
    public string ItemName;
    public float weight = 0.05f;
    public int quantity = 1;

    [Header("Sell Settings")]
    public float pricePerKg = 10f; // ราคาขายต่อ 1 กิโลกรัม
    //public float Price;

    [Header("UI Settings")]
    public GameObject interactUI; // UI ปุ่ม E

    private Inventory playerInventory;
    private bool isPickedUp = false; // ป้องกันการเก็บซ้ำ

    void Start()
    {
        // แยก UI ออกจาก Item
        interactUI?.transform.SetParent(null, true);
        interactUI?.SetActive(false);

        // หา Inventory ของ Player
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

    /// <summary>โชว์หรือซ่อน UI "กด E เก็บของ"</summary>
    public void ShowUI(bool state)
    {
        if (interactUI == null) return;  // ✅ ป้องกัน null
        if (this == null) return;        // ✅ ถ้าโดน Destroy ไปแล้ว
        if (gameObject == null) return;  // ✅ ถ้า object หายแล้ว

        interactUI.SetActive(state);
    }


    /// <summary>ให้ Player เก็บ Item</summary>
    public void PickUpItem()
    {
        if (isPickedUp || playerInventory == null) return;

        // สร้าง Item object พร้อมราคา
        Item newItem = new Item(ItemName, weight, pricePerKg, quantity);

        if (!playerInventory.AddItem(newItem)) return;

        isPickedUp = true;

        interactUI?.SetActive(false);
        Destroy(interactUI, 0.05f);
        Destroy(gameObject);
    }

}
