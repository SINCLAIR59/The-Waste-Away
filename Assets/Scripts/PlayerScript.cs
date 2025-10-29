using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float Speed = 5f;
    public Transform boundObject; // GameObject ที่มี BoxCollider2D
    public Vector2 minBound; // ซ้าย-ล่าง
    public Vector2 maxBound; // ขวา-บน

    [Header("Player Stats")]
    public float Money = 0f;
    public TMP_Text MoneyUI;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private readonly List<ItemScript> itemsInRange = new List<ItemScript>();
    private ItemScript nearestItem;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateMoneyUI();

        // ✅ ดึงค่าขอบจาก BoxCollider2D ของ GameObject ที่อ้างอิง
        if (boundObject != null)
        {
            BoxCollider2D box = boundObject.GetComponent<BoxCollider2D>();
            if (box != null)
            {
                Vector2 center = box.bounds.center;
                Vector2 size = box.bounds.size;

                minBound = center - size / 2f;
                maxBound = center + size / 2f;
            }
            else
            {
                Debug.LogWarning("Bound Object ไม่มี BoxCollider2D");
            }
        }
        else
        {
            Debug.LogWarning("ยังไม่ได้อ้างอิง Bound Object");
        }
    }

    void Update()
    {
        // รับ input การเคลื่อนไหว
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // พลิกตัวละครตามทิศทาง
        if (spriteRenderer && moveInput.x != 0)
            spriteRenderer.flipX = moveInput.x < 0;

        // หา item ใกล้สุด
        UpdateNearestItem();

        // กด E เก็บของ
        if (nearestItem != null && Input.GetKeyDown(KeyCode.E))
        {
            nearestItem.PickUpItem();
            nearestItem = null;
        }
    }

    void FixedUpdate()
    {
        // เคลื่อนที่เฉพาะตอนมี input
        if (moveInput.sqrMagnitude > 0)
        {
            Vector2 move = moveInput.normalized * Speed * Time.fixedDeltaTime;
            Vector2 newPos = rb.position + move;

            // จำกัดไม่ให้ออกนอกขอบ
            newPos.x = Mathf.Clamp(newPos.x, minBound.x, maxBound.x);
            newPos.y = Mathf.Clamp(newPos.y, minBound.y, maxBound.y);

            rb.MovePosition(newPos);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void UpdateMoneyUI()
    {
        if (MoneyUI != null)
            MoneyUI.text = $"เงิน {Money:F2}฿ บาท";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;

        // ตรวจ Item
        ItemScript item = other.GetComponent<ItemScript>();
        if (item != null && !itemsInRange.Contains(item))
        {
            itemsInRange.Add(item);
            item.ShowUI(true);
        }

        // ตรวจ SellPoint
        SellPoint sellPoint = other.GetComponent<SellPoint>();
        if (sellPoint != null)
        {
            sellPoint.ShowUI(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other == null) return;

        // ตรวจ Item
        ItemScript item = other.GetComponent<ItemScript>();
        if (item != null && item.gameObject != null)
        {
            itemsInRange.Remove(item);
            if (item != null)
                item.ShowUI(false);
        }

        // ตรวจ SellPoint
        SellPoint sellPoint = other.GetComponent<SellPoint>();
        if (sellPoint != null && sellPoint.gameObject != null)
        {
            sellPoint.ShowUI(false);
        }
    }


    private void UpdateNearestItem()
    {
        float nearestDist = Mathf.Infinity;
        ItemScript closest = null;

        for (int i = itemsInRange.Count - 1; i >= 0; i--)
        {
            ItemScript item = itemsInRange[i];
            if (item == null)
            {
                itemsInRange.RemoveAt(i);
                continue;
            }

            float dist = Vector2.Distance(transform.position, item.transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                closest = item;

                if (nearestDist <= 0.05f) break;
            }
        }

        if (nearestItem != closest)
        {
            nearestItem?.ShowUI(false);
            nearestItem = closest;
            nearestItem?.ShowUI(true);
        }
    }
}
