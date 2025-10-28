using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Inventory Settings")]
    public float maxWeight = 10f; // 10 kg
    public TMP_Text WeightUI;


    private readonly List<Item> items = new List<Item>();
    public List<Item> Items => items;


    // คำนวณน้ำหนักรวม
    public float CurrentWeight()
    {
        float total = 0f;
        foreach (Item item in items)
        {
            total += item.TotalWeight();
        }
        return total;
    }

    private void Start()
    {
        UpdateWeightUI();
    }

    public void UpdateWeightUI()
    {
        if (WeightUI != null)
            WeightUI.text = $"น้ำหนัก {CurrentWeight():F2}/{maxWeight} กิโล";
    }

    public bool AddItem(Item newItem)
    {
        if (newItem == null) return false;

        float newTotalWeight = CurrentWeight() + newItem.TotalWeight();
        if (newTotalWeight > maxWeight)
        {
            Debug.LogWarning($"Cannot add {newItem.itemName}. Exceeds max weight!");
            return false;
        }

        // เพิ่มจำนวนถ้ามีอยู่แล้ว
        Item existingItem = items.Find(i => i.itemName == newItem.itemName);
        if (existingItem != null)
        {
            existingItem.quantity += newItem.quantity;
        }
        else
        {
            items.Add(newItem);
        }

        Debug.Log($"{newItem.itemName} added. Current weight: {CurrentWeight():F2} kg");
        UpdateWeightUI();
        return true;
    }

    public void RemoveItem(string itemName, int quantity)
    {
        if (string.IsNullOrEmpty(itemName) || quantity <= 0) return;

        Item item = items.Find(i => i.itemName == itemName);
        if (item == null) return;

        item.quantity -= quantity;
        if (item.quantity <= 0)
            items.Remove(item);

        UpdateWeightUI();
    }

    public void SellAllItems()
    {
        if (items.Count == 0)
        {
            Debug.Log("ไม่มีของในกระเป๋าให้ขาย");
            return;
        }

        float totalWeight = CurrentWeight();
        float totalPrice = 0f;

        foreach (Item item in items)
        {
            totalPrice += item.price * item.quantity;
        }

        items.Clear();
        UpdateWeightUI();

        Debug.Log($"ขายของทั้งหมดแล้ว ได้เงิน {totalPrice:F2} บาท (น้ำหนักที่ขาย {totalWeight:F2} กก.)");
    }

    public void ClearInventory()
    {
        items.Clear();
        UpdateWeightUI();
    }
}
