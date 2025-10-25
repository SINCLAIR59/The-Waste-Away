﻿using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float Speed;
    private Rigidbody2D RB;

    private List<ItemScript> itemsInRange = new List<ItemScript>();
    private ItemScript nearestItem;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Movement
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        RB.linearVelocity = new Vector2(moveX * Speed, moveY * Speed);

        // หา Item ใกล้สุด
        UpdateNearestItem();

        // กด E เก็บ Item ใกล้สุด
        if (nearestItem != null && Input.GetKeyDown(KeyCode.E))
        {
            nearestItem.PickUpItem();
            nearestItem = null; // ป้องกันเรียกซ้ำ
        }
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        ItemScript item = target.GetComponent<ItemScript>();
        if (item != null && !itemsInRange.Contains(item))
        {
            itemsInRange.Add(item);
        }
    }

    private void OnTriggerExit2D(Collider2D target)
    {
        ItemScript item = target.GetComponent<ItemScript>();
        if (item != null)
        {
            itemsInRange.Remove(item);
        }
    }

    private void UpdateNearestItem()
    {
        float nearestDist = Mathf.Infinity;
        ItemScript closest = null;

        foreach (var item in itemsInRange)
        {
            float dist = Vector2.Distance(transform.position, item.transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                closest = item;
            }
        }

        // อัปเดต UI เฉพาะ Item ใกล้สุด
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
