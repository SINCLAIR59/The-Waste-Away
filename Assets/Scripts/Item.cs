using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public float weight;
    public float pricePerKg;
    public float price => weight * pricePerKg; // ใช้ property แทน field เพื่อคำนวณอัตโนมัติ
    public int quantity;

    public Item(string name, float w, float pricePerKg, int qty)
    {
        itemName = name;
        weight = Mathf.Max(0f, w);           // ป้องกันค่าติดลบ
        this.pricePerKg = Mathf.Max(0f, pricePerKg);
        quantity = Mathf.Max(0, qty);
    }

    // 🔹 น้ำหนักรวมของ Item (weight * quantity)
    public float TotalWeight()
    {
        return weight * Mathf.Max(0, quantity);
    }
}
