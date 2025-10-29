using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float Speed = 5f;
    public Transform boundObject;
    public Vector2 minBound;
    public Vector2 maxBound;

    [Header("Player Stats")]
    public float Money = 0f;
    public TMP_Text MoneyUI;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private readonly List<ItemScript> itemsInRange = new();
    private ItemScript nearestItem;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        UpdateMoneyUI();
        SetupBounds();
    }

    void Update()
    {
        HandleInput();
        UpdateNearestItem();
        HandlePickup();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleInput()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (spriteRenderer && moveInput.x != 0)
            spriteRenderer.flipX = moveInput.x < 0;
    }

    private void HandlePickup()
    {
        if (nearestItem != null && Input.GetKeyDown(KeyCode.E))
        {
            nearestItem.PickUpItem();
            nearestItem = null;
        }
    }

    private void MovePlayer()
    {
        if (moveInput.sqrMagnitude <= 0)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 move = moveInput.normalized * Speed * Time.fixedDeltaTime;
        Vector2 newPos = rb.position + move;
        newPos.x = Mathf.Clamp(newPos.x, minBound.x, maxBound.x);
        newPos.y = Mathf.Clamp(newPos.y, minBound.y, maxBound.y);
        rb.MovePosition(newPos);
    }

    private void SetupBounds()
    {
        if (boundObject == null)
        {
            Debug.LogWarning("ยังไม่ได้อ้างอิง Bound Object");
            return;
        }

        BoxCollider2D box = boundObject.GetComponent<BoxCollider2D>();
        if (box == null)
        {
            Debug.LogWarning("Bound Object ไม่มี BoxCollider2D");
            return;
        }

        Vector2 center = box.bounds.center;
        Vector2 size = box.bounds.size;
        minBound = center - size / 2f;
        maxBound = center + size / 2f;
    }

    public void UpdateMoneyUI()
    {
        if (MoneyUI != null)
            MoneyUI.text = $"เงิน {Money:F2}฿ บาท";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other) return;

        var item = other.GetComponent<ItemScript>();
        if (item && !itemsInRange.Contains(item))
        {
            itemsInRange.Add(item);
            item.ShowUI(true);
        }

        var sellPoint = other.GetComponent<SellPoint>();
        if (sellPoint)
            sellPoint.ShowUI(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other) return;

        var item = other.GetComponent<ItemScript>();
        if (item)
        {
            itemsInRange.Remove(item);
            item.ShowUI(false);
        }

        var sellPoint = other.GetComponent<SellPoint>();
        if (sellPoint)
            sellPoint.ShowUI(false);
    }

    private void UpdateNearestItem()
    {
        float nearestDist = Mathf.Infinity;
        ItemScript closest = null;

        for (int i = itemsInRange.Count - 1; i >= 0; i--)
        {
            var item = itemsInRange[i];
            if (!item)
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
