using System.ComponentModel.DataAnnotations;
using _0_framework.Application;
using ShopManagement.Application.Contracts.ProductCategory;

namespace ShopManagement.Application.Contracts.Product;

public class EditProduct : CreateProduct
{
    public long Id { get; set; }
}