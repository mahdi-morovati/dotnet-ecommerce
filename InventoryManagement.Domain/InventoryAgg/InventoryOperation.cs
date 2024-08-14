namespace InventoryManagement.Domain.InventoryAgg;

public class InventoryOperation
{
    public long Id { get; private set; }
    public bool Operation { get; private set; } // Entry or exit. true => increase, false => decrease
    public long Count { get; private set; } // count of changes
    public long OperatorId { get; private set; } // user id
    public DateTime OperationDate { get; private set; } // Entry or exit date
    public long CurrentCount { get; private set; } // count of product on change time
    public string Description { get; private set; }
    public long OrderId { get; private set; } // if customer has order put the order number
    public long InventoryId { get; private set; }
    public Inventory Inventory { get; private set; }
    
    /// <summary>
    /// initial inventory operation
    /// </summary>
    /// <param name="operation"> ture when we want increase. false when we want decrease</param>
    /// <param name="count">count of increase or decrease</param>
    /// <param name="operatorId">user or operator id</param>
    /// <param name="currentCount">count of product on change time</param>
    /// <param name="description">description</param>
    /// <param name="orderId">if customer has order put the order number</param>
    /// <param name="inventoryId">the inventory id</param>
    public InventoryOperation(bool operation, long count, long operatorId, long currentCount,
        string description, long orderId, long inventoryId)
    {
        Operation = operation;
        Count = count;
        OperatorId = operatorId;
        CurrentCount = currentCount;
        Description = description;
        OrderId = orderId;
        InventoryId = inventoryId;
        OperationDate = DateTime.Now;
    }
    
}