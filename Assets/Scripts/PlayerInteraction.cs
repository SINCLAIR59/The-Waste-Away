using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 2f;
    private ItemScript currentItem;

    void Update()
    {
        // ตรวจสอบไอเทมใกล้ตัว
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        if (hit.collider != null)
        {
            ItemScript item = hit.collider.GetComponent<ItemScript>();
            if (item != null)
            {
                if (Vector2.Distance(transform.position, item.transform.position) <= interactRange)
                {
                    currentItem = item;
                    item.ShowUI(true);

                    if (Input.GetKeyDown(KeyCode.E))
                        item.PickUpItem();
                }
            }
        }
        else
        {
            if (currentItem != null)
            {
                currentItem.ShowUI(false);
                currentItem = null;
            }
        }
    }
}
