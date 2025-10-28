using UnityEngine;

public class SellPoint : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject sellUI; // UI แสดงปุ่ม E หรือข้อความ "กด E เพื่อขาย"

    private bool canSell = false;
    private Inventory playerInventory;
    private PlayerScript player;

    void Start()
    {
        if (sellUI != null)
            sellUI.SetActive(false);

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("ไม่พบ Player ใน Scene! ต้องตั้ง Tag = 'Player'");
            return;
        }

        playerInventory = playerObj.GetComponent<Inventory>();
        player = playerObj.GetComponent<PlayerScript>();

        if (player == null)
            Debug.LogError("PlayerScript ไม่พบบน Player!");
    }

    public void ShowUI(bool state)
    {
        if (sellUI != null)
            sellUI.SetActive(state);
    }

    void Update()
    {
        if (canSell && Input.GetKeyDown(KeyCode.E))
        {
            SellAllItems();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        canSell = true;
        ShowUI(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        canSell = false;
        ShowUI(false);
    }

    void SellAllItems()
    {
        if (playerInventory == null || player == null) return;

        float totalPrice = 0f;
        foreach (Item item in playerInventory.Items)
        {
            totalPrice += item.price * item.quantity;
        }

        player.Money += totalPrice;
        player.UpdateMoneyUI();

        Debug.Log($"ขายของทั้งหมดได้ {totalPrice:F2} บาท");

        playerInventory.ClearInventory();
        playerInventory.UpdateWeightUI();
    }

}
