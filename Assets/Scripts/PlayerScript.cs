using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float Speed = 5f;

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
        if (moveInput.sqrMagnitude > 0)
            rb.linearVelocity = moveInput.normalized * Speed;
        else
            rb.linearVelocity = Vector2.zero;
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
        if (item != null)
        {
            itemsInRange.Remove(item);
            item.ShowUI(false);
        }

        // ตรวจ SellPoint
        SellPoint sellPoint = other.GetComponent<SellPoint>();
        if (sellPoint != null)
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
