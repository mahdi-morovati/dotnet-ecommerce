using _0_framework.Application;
using _0_framework.Infrastructure;
using InventoryManagement.Application.Contract.Inventory;
using InventoryManagement.Domain.InventoryAgg;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Infrastructure.EFCore;

namespace InventoryManagement.Infrastructure.EFCore.Repository;

public class InventoryRepository : RepositoryBase<long, Inventory>, IInventoryRepository
{
    private readonly InventoryContext _context;
    private readonly ShopContext _shopDbContext;

    public InventoryRepository(InventoryContext context, ShopContext shopDbContext) : base(context)
    {
        _context = context;
        _shopDbContext = shopDbContext;
    }

    public EditInventory GetDetails(long id)
    {
        return _context.Inventory.Select(x => new EditInventory
        {
            Id = x.Id,
            ProductId = x.ProductId,
            UnitPrice = x.UnitPrice,
        }).FirstOrDefault(x => x.Id == id);
    }

    public Inventory GetBy(long productId)
    {
        return _context.Inventory.FirstOrDefault(x => x.ProductId == productId);
    }

    public List<InventoryViewModel> Search(InventorySearchModel searchModel)
    {
        var products = _shopDbContext.Products.Select(x => new { x.Id, x.Name }).ToList();
        var query = _context.Inventory.Select(x => new InventoryViewModel
        {
            Id = x.Id,
            UnitPrice = x.UnitPrice,
            InStock = x.InStock,
            ProductId = x.ProductId,
            CurrentCount = x.CalculateCurrentCount(),
            CreationDate = x.CreationDate.ToFarsi(),
        });
        if (searchModel.ProductId > 0)
        {
            query = query.Where(x => x.ProductId == searchModel.ProductId);
        }

        if (searchModel.InStock)
        {
            query = query.Where(x => !x.InStock);
        }

        var inventory = query.OrderByDescending(x => x.Id).ToList();
        inventory.ForEach(item => { item.Product = products.FirstOrDefault(x => x.Id == item.ProductId)?.Name; });
        return inventory;
    }
    
    public List<InventoryOperationViewModel> GetOperationLog(long inventoryId)
    {
        var inventory = _context.Inventory.FirstOrDefault(x => x.Id == inventoryId);
        var operations = inventory.Operations.Select(x => new InventoryOperationViewModel
        {
            Id = x.Id,
            Count = x.Count,
            CurrentCount = x.CurrentCount,
            Description = x.Description,
            Operation = x.Operation,
            Operator = "مدیر سیستم",
            OperationDate = x.OperationDate.ToFarsi(),
            OperatorId = x.OperatorId,
            OrderId = x.OrderId
        }).OrderByDescending(x => x.Id).ToList();
        

        return operations;
    }
    
}