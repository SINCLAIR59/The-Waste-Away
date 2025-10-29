using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactRange = 2f;

    private ItemScript currentItem;

    void Update()
    {
        FindNearestItem();
        HandleItemPickup();
    }

    private void FindNearestItem()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange);
        ItemScript nearest = null;
        float nearestDist = Mathf.Infinity;

        foreach (var col in hits)
        {
            if (col == null) continue;

            var item = col.GetComponent<ItemScript>();
            if (item == null) continue;

            float dist = Vector2.Distance(transform.position, item.transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = item;
            }
        }

        // อัปเดต UI ของ item ใกล้ตัวที่สุด
        if (currentItem != nearest)
        {
            currentItem?.ShowUI(false);
            currentItem = nearest;
            currentItem?.ShowUI(true);
        }
    }

    private void HandleItemPickup()
    {
        if (currentItem != null && Input.GetKeyDown(KeyCode.E))
        {
            currentItem.PickUpItem();
            currentItem = null;
        }
    }

    // แสดงระยะ interact ใน Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
