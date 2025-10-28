using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public float weight;
    public float pricePerKg;
    public float price;
    public int quantity;

    public Item(string name, float w, float pricePerKg, int qty)
    {
        itemName = name;
        weight = w;
        this.pricePerKg = pricePerKg;
        quantity = qty;
        price = w * pricePerKg;
    }

    public float TotalWeight()
    {
        return weight * Mathf.Max(0, quantity); // ป้องกันน้ำหนักติดลบ
    }
}
