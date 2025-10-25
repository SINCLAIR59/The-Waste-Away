using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public GameObject interactUI; // UI ปุ่ม E
    private bool canInteract = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // ตรวจสอบ UI ก่อนซ่อน
        if (interactUI != null)
        {
            // ถ้าเป็น Child ของ Item ให้ตัดออกจาก Parent
            interactUI.transform.SetParent(null);
            interactUI.SetActive(false);
        }
    }

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            PickUpItem();
        }
    }

    private void PickUpItem()
    {
        Debug.Log("เก็บไอเท็มแล้ว!");
        // ซ่อน UI ก่อนทำลาย Item
        if (interactUI != null)
            interactUI.SetActive(false);

        Destroy(gameObject); // ลบ Item
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = true;

            if (interactUI != null)
                interactUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = false;

            if (interactUI != null)
                interactUI.SetActive(false);
        }
    }
}
