using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public float maxWeight = 10f; // 10 kg
    private List<Item> items = new List<Item>();
    public TMP_Text WeightUI;


public float CurrentWeight()
    {
        float total = 0;
        foreach (Item item in items)
        {
            total += item.TotalWeight();
        }
        return total;
    }

    public bool AddItem(Item newItem)
    {
        float newTotalWeight = CurrentWeight() + newItem.TotalWeight();
        if (newTotalWeight <= maxWeight)
        {
            // ถ้าเจอไอเทมเดียวกันอยู่แล้ว ให้เพิ่มจำนวน
            Item existingItem = items.Find(i => i.itemName == newItem.itemName);
            if (existingItem != null)
            {
                existingItem.quantity += newItem.quantity;
            }
            else
            {
                items.Add(newItem);
            }
            Debug.Log($"{newItem.itemName} added. Current weight: {CurrentWeight()}kg");
            WeightUI.text = "น้ำนัก "+ CurrentWeight().ToString("F2")+"/"+ maxWeight+" กิโล";
            return true;
        }
        else
        {
            Debug.Log($"Cannot add {newItem.itemName}. Exceeds max weight!");
            return false;
        }
    }

    public void RemoveItem(string itemName, int quantity)
    {
        Item item = items.Find(i => i.itemName == itemName);
        if (item != null)
        {
            item.quantity -= quantity;
            if (item.quantity <= 0)
                items.Remove(item);
        }
    }
}
