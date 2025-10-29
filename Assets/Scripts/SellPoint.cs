using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SellPoint : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject sellUI;

    private bool canSell;
    private Inventory playerInventory;
    private PlayerScript player;

    void Awake()
    {
        if (sellUI != null)
            sellUI.SetActive(false);
    }

    void Start()
    {
        SetupPlayerReference();
    }

    private void SetupPlayerReference()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("❌ ไม่พบ Player ใน Scene! ต้องตั้ง Tag = 'Player'");
            return;
        }

        playerInventory = playerObj.GetComponent<Inventory>();
        player = playerObj.GetComponent<PlayerScript>();

        if (player == null)
            Debug.LogError("❌ PlayerScript ไม่พบบน Player!");
        if (playerInventory == null)
            Debug.LogError("❌ Inventory ไม่พบบน Player!");
    }

    public void ShowUI(bool state)
    {
        if (sellUI != null)
            sellUI.SetActive(state);
    }

    void Update()
    {
        if (canSell && Input.GetKeyDown(KeyCode.E))
            SellAllItems();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        canSell = true;
        ShowUI(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        canSell = false;
        ShowUI(false);
    }

    private void SellAllItems()
    {
        if (playerInventory == null || player == null) return;

        float totalPrice = 0f;
        foreach (Item item in playerInventory.Items)
        {
            if (item == null) continue;
            totalPrice += item.price * item.quantity;
        }

        if (totalPrice <= 0f)
        {
            Debug.Log("ไม่มีของให้ขาย");
            return;
        }

        player.Money += totalPrice;
        player.UpdateMoneyUI();

        playerInventory.ClearInventory();

        Debug.Log($"✅ ขายของทั้งหมดได้เงิน {totalPrice:F2} บาท");
    }
}
