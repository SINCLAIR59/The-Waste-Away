using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float Speed = 5f;

    [Header("Player Stats")]
    public float Money = 0f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private readonly List<ItemScript> itemsInRange = new List<ItemScript>();
    private ItemScript nearestItem;

    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // รับ input การเคลื่อนไหว
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // พลิกตัวละครตามทิศทาง
        if (spriteRenderer)
        {
            if (moveInput.x != 0)
                spriteRenderer.flipX = moveInput.x < 0;
        }

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
        // เคลื่อนที่ (ใช้ใน FixedUpdate เพื่อความสมูท)
        rb.linearVelocity = moveInput.normalized * Speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ItemScript item = other.GetComponent<ItemScript>();
        if (item != null && !itemsInRange.Contains(item))
        {
            itemsInRange.Add(item);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ItemScript item = other.GetComponent<ItemScript>();
        if (item != null)
        {
            itemsInRange.Remove(item);
        }
    }

    private void UpdateNearestItem()
    {
        float nearestDist = Mathf.Infinity;
        ItemScript closest = null;

        // ล้าง item ที่ถูกลบ (destroyed)
        for (int i = itemsInRange.Count - 1; i >= 0; i--)
        {
            if (itemsInRange[i] == null)
            {
                itemsInRange.RemoveAt(i);
                continue;
            }

            float dist = Vector2.Distance(transform.position, itemsInRange[i].transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                closest = itemsInRange[i];

                if (nearestDist <= 0.05f) // ถ้าอยู่ใกล้มากๆ ก็หยุดหาได้เลย
                    break;
            }
        }

        // อัปเดต UI เฉพาะ item ที่ใกล้สุด
        if (nearestItem != closest)
        {
            if (nearestItem != null)
                nearestItem.ShowUI(false);

            nearestItem = closest;

            if (nearestItem != null)
                nearestItem.ShowUI(true);
        }
    }
}
