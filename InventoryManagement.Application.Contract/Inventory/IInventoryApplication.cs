using _0_framework.Application;

namespace InventoryManagement.Application.Contract.Inventory;

public interface IInventoryApplication
{
    OperationResult Create(CreateInventory command);
    OperationResult Edit(EditInventory command);
    OperationResult Increase(IncreaseInventory command);
    OperationResult Reduce(ReduceInventory command);
    OperationResult Reduce(List<ReduceInventory> command);
    EditInventory GetDetails(long id);
    List<InventoryViewModel> Search(InventorySearchModel searchModel);
    List<InventoryOperationViewModel> GetOperationLog(long inventoryId);
}