namespace Inventories.Domain.StockItems;

public class StockItem
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Units { get; private set; }

    private StockItem(Guid id, string name, int units)
    {
        Id = id;
        Name = name;
        Units = units;
    }

    public static StockItem Create(Guid id, string name, int units)
    {
        return new(id, name, units);
    }
}
