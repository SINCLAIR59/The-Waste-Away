using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 2f;
    private ItemScript currentItem;

    void Update()
    {
        // หา Item ใกล้ตัว
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange);
        ItemScript nearestItem = null;
        float nearestDist = Mathf.Infinity;

        foreach (Collider2D col in hits)
        {
            if (col == null) continue;

            ItemScript item = col.GetComponent<ItemScript>();
            if (item == null) continue;

            float dist = Vector2.Distance(transform.position, item.transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearestItem = item;
            }
        }

        // อัปเดต UI และการเก็บของ
        if (currentItem != nearestItem)
        {
            currentItem?.ShowUI(false);
            currentItem = nearestItem;
            currentItem?.ShowUI(true);
        }

        if (currentItem != null && Input.GetKeyDown(KeyCode.E))
        {
            currentItem.PickUpItem();
            currentItem = null;
        }
    }

    // ช่วยให้เห็นระยะ interact ใน Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
