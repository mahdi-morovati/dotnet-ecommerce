using System.ComponentModel.DataAnnotations;
using _0_framework.Application;

namespace InventoryManagement.Application.Contract.Inventory;

public class EditInventory : CreateInventory
{
    public long Id { get; set; }
}