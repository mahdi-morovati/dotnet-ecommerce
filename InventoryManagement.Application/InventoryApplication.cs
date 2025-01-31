using _0_framework.Application;
using InventoryManagement.Application.Contract.Inventory;
using InventoryManagement.Domain.InventoryAgg;

namespace InventoryManagement.Application;

public class InventoryApplication : IInventoryApplication
{
    private readonly IInventoryRepository _inventoryRepository;

    public InventoryApplication(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public OperationResult Create(CreateInventory command)
    {
        var operation = new OperationResult();
        if (_inventoryRepository.Exists(x => x.Id == command.ProductId))
        {
            return operation.Failed(ApplicationMessages.DuplicatedRecord);
        }

        var inventory = new Inventory(command.ProductId, command.UnitPrice);
        _inventoryRepository.Create(inventory);
        _inventoryRepository.SaveChanges();
        return operation.Succedded();
    }

    public OperationResult Edit(EditInventory command)
    {
        var operation = new OperationResult();
        var inventory = _inventoryRepository.Get(command.Id);
        if (inventory == null)
        {
            return operation.Failed(ApplicationMessages.RecordNotFound);
        }

        if (_inventoryRepository.Exists(x => x.ProductId == command.ProductId && x.Id != command.Id))
        {
            return operation.Failed(ApplicationMessages.DuplicatedRecord);
        }

        inventory.Edit(command.ProductId, command.UnitPrice);
        _inventoryRepository.SaveChanges();
        return operation.Succedded();
    }

    public OperationResult Increase(IncreaseInventory command)
    {
        var operation = new OperationResult();
        var inventory = _inventoryRepository.Get(command.InventoryId);
        if (inventory == null)
        {
            return operation.Failed(ApplicationMessages.RecordNotFound);
        }

        const long operatorId = 1;
        inventory.Increase(command.Count, operatorId, command.Description);
        _inventoryRepository.SaveChanges();
        return operation.Succedded();
    }

    public OperationResult Reduce(ReduceInventory command)
    {
        var operation = new OperationResult();
        var inventory = _inventoryRepository.Get(command.InventoryId);
        if (inventory == null)
        {
            return operation.Failed(ApplicationMessages.RecordNotFound);
        }

        const long operatorId = 1;
        inventory.Reduce(command.Count, operatorId, command.Description,
            0); // orderId is 0 because it doesn't doin order. (do by storekeeper)  
        _inventoryRepository.SaveChanges();
        return operation.Succedded();
    }

    public OperationResult Reduce(List<ReduceInventory> command)
    {
        var operation = new OperationResult();
        var operatorId = 1;
        foreach (var item in command)
        {
            var inventory = _inventoryRepository.GetBy(item.ProductId);
            inventory.Reduce(item.Count, operatorId, item.Description, item.OrderId);
        }

        _inventoryRepository.SaveChanges();
        return operation.Succedded();
    }

    public EditInventory GetDetails(long id)
    {
        return _inventoryRepository.GetDetails(id);
    }

    public List<InventoryViewModel> Search(InventorySearchModel searchModel)
    {
        return _inventoryRepository.Search(searchModel);
    }

    public List<InventoryOperationViewModel> GetOperationLog(long inventoryId)
    {
        return _inventoryRepository.GetOperationLog(inventoryId);
    }
}