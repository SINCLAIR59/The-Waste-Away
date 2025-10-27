using UnityEngine;

[System.Serializable]
public class Item
{
    public readonly string itemName;
    public float weight;
    public float Price;
    public int quantity;

    public Item(string name, float w, float price, int qty)
    {
        itemName = name;
        weight = w;
        Price = price;
        quantity = Mathf.Max(0, qty); // ป้องกันจำนวนติดลบ
    }

    public float TotalWeight()
    {
        return weight * Mathf.Max(0, quantity); // ป้องกันน้ำหนักติดลบ
    }
}
