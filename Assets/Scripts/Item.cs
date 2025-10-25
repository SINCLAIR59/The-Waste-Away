[System.Serializable]
public class Item
{
    public string itemName;
    public float weight;
    public int quantity;

    public Item(string name, float w, int qty)
    {
        itemName = name;
        weight = w;
        quantity = qty;
    }

    public float TotalWeight()
    {
        return weight * quantity;
    }
}
