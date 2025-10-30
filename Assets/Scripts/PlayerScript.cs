using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float Speed = 10f;
    public Transform boundObject;
    public Vector2 minBound;
    public Vector2 maxBound;
    public float PlayerMoveX;
    public float PlayerMoveY;
    [SerializeField] private Animator animator;

    [Header("Player Stats")]
    public float Money = 0f;
    public TMP_Text MoneyUI;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private readonly List<ItemScript> itemsInRange = new();
    private ItemScript nearestItem;
    private Vector2 moveInput;
    private ContactFilter2D collisionFilter;
    private readonly RaycastHit2D[] raycastHits = new RaycastHit2D[1];
    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // ตั้งค่า Filter สำหรับตรวจการชนล่วงหน้า (สร้างครั้งเดียว)
        collisionFilter.useLayerMask = true;
        collisionFilter.SetLayerMask(LayerMask.GetMask("Obstacle"));
        collisionFilter.useTriggers = false;
    }

    void Start()
    {
        SetupBounds();
        UpdateMoneyUI();
    }

    void Update()
    {
        HandleInput();
        HandlePickup();
        UpdateNearestItem();
        if (PlayerMoveX != 0 || PlayerMoveY != 0)
        {
            animator.SetBool("isRuning", true);
        }
        else {
            animator.SetBool("isRuning", false);
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleInput()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        PlayerMoveX = moveInput.x;
        PlayerMoveY = moveInput.y;

        if (moveInput.x != 0)
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
        if (moveInput == Vector2.zero)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 move = moveInput.normalized * Speed * Time.fixedDeltaTime;
        Vector2 newPos = rb.position + move;

        // จำกัดขอบแมพ
        newPos.x = Mathf.Clamp(newPos.x, minBound.x, maxBound.x);
        newPos.y = Mathf.Clamp(newPos.y, minBound.y, maxBound.y);

        // ตรวจการชน
        int hitCount = rb.Cast(move.normalized, collisionFilter, raycastHits, move.magnitude);
        if (hitCount == 0)
            rb.MovePosition(newPos);
    }

    private void SetupBounds()
    {
        if (!boundObject)
        {
            Debug.LogWarning("⚠️ Bound Object ยังไม่ถูกอ้างอิง");
            return;
        }

        if (boundObject.TryGetComponent(out BoxCollider2D box))
        {
            Vector2 center = box.bounds.center;
            Vector2 size = box.bounds.size;
            minBound = center - size / 2f;
            maxBound = center + size / 2f;
        }
        else
        {
            Debug.LogWarning("⚠️ Bound Object ไม่มี BoxCollider2D");
        }
    }

    public void UpdateMoneyUI()
    {
        if (MoneyUI)
            MoneyUI.text = $"เงิน {Money:F2} ฿";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other) return;

        if (other.TryGetComponent(out ItemScript item) && !itemsInRange.Contains(item))
        {
            itemsInRange.Add(item);
            item.ShowUI(true);
        }

        if (other.TryGetComponent(out SellPoint sellPoint))
            sellPoint.ShowUI(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other) return;

        if (other.TryGetComponent(out ItemScript item))
        {
            itemsInRange.Remove(item);
            item.ShowUI(false);
        }

        if (other.TryGetComponent(out SellPoint sellPoint))
            sellPoint.ShowUI(false);
    }

    private void UpdateNearestItem()
    {
        float nearestDist = float.MaxValue;
        ItemScript closest = null;

        for (int i = itemsInRange.Count - 1; i >= 0; i--)
        {
            var item = itemsInRange[i];
            if (!item)
            {
                itemsInRange.RemoveAt(i);
                continue;
            }

            float dist = Vector2.SqrMagnitude(item.transform.position - transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                closest = item;
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
