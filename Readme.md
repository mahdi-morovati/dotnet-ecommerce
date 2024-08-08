### ProductCategory CRUD

* create Domain Class Library (ShopManagement.Domain)
  create ProductCategory class
  create IProductCategoryRepository


* Create 0_framework class library

1- create EntityBase class for models.(Domain models implement it)

* create ShopManagement.Application.Contracts
  1- ProductCategory directory create CreateProductCategory, EditProductCategory classes
  2- create ProductCategorySearchModel, ProductCategoryViewModel
  3 - create IProductCategoryApplication interface

* create ProductCategoryApplication in ShopManagement.Application
*

**Layer Orders**
UI => call application layer
Application call repository from infrastructure
Application call Domain

* create ProductCategoryApplication in ShopManagement.Application

* create ShopManagement.Infrastructure.EFCore
    * creating mappings, repositories, DbContext
    * install Microsoft.EntityFrameworkCore/8.0.7
    * install Microsoft.EntityFrameworkCore.SqlServer/8.0.7
    * install Microsoft.EntityFrameworkCore.Tools/8.0.7
    * create ProductCategoryMapping
    * create ProductCategoryRepository

* create IRepository in 0_framework/Domain
* create Repository in 0_framework/Infrastructure
* implement IProductCategoryRepository and ProductCategoryRepository from IRepository

* کانفیگ کردن اینترفیس ها و پیاده سازیشون
* so create ShopManagement.Configuration
* and create ShopManagementBootstrapper

* Create ServiceHost Project and inject ShopManagementBootstrapper to Program.cs


* create ServiceHost Project and add admin theme in Areas/Administration/
    * implement ServiceHost.Areas.Administration.Pages.Shop.ProductCategories.IndexModel.OnGet

* create Edit page

----------------------------------------------------------------
1- create Product model in ShopManagement.Domain (productAgg directory) and
create ctor for initial data for creating model
create edit for create editing data
2- create IProductRepository in ShopManagement.Domain
(productAgg)

3- in ShopManagement.Application.Contracts create Product folder
the application contract in fact is a place to define application interfaces. that is(that's) mean this classes are DTO
class.

* create CreateProduct class
* create EdtProduct class
* create ProductViewModel class
* create ProductSearchModel class

* create IProductApplication contains this methods

  OperationResult Create(CreateProduct command);
  OperationResult Edit(EditProduct command);
  EditProduct GetDetails(long id);
  List<ProductViewModel> Search(ProductSearchModel searchModel);

* define this methods in IProductRepository

  EditProduct GetDetails(long id);
  List<ProductViewModel> Search(ProductSearchModel searchModel);

* create ShopManagement.Application.ProductApplication that implements IProductApplication

    * create ShopManagement.Infrastructure.EFCore.Mapping.ProductMapping and define relationship with category.(define
      relationship in category too)
* define DbSet<Product> in ShopContext
* create ShopManagement.Infrastructure.EFCore.Repository.ProductRepository

* bind interfaces in ShopManagement.Configuration.ShopManagementBootstrapper

  services.AddTransient<IProductApplication, ProductApplication>();
  services.AddTransient<IProductRepository, ProductRepository>();

* create Products/ razor pages index
* create: 
  * ServiceHost.Areas.Administration.Pages.Shop.Products.IndexModel.ProductCategories
  * ShopManagement.Domain.ProductCategoryAgg.IProductCategoryRepository.GetProductCategories
  * ShopManagement.Infrastructure.EFCore.Repository.ProductCategoryRepository.GetProductCategories
  * ShopManagement.Application.Contracts.ProductCategory.IProductCategoryApplication.GetProductCategories
  * ShopManagement.Application.ProductCategoryApplication.GetProductCategories
  * ServiceHost.Areas.Administration.Pages.Shop.Products.IndexModel.ProductCategories on OnGet method
  * create ProductAdded migration
  
* 