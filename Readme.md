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
### Product CRUD
1- create Product model in ShopManagement.Domain (productAgg directory) and
create ctor for initial data for creating model
create edit for create editing data
2- create IProductRepository in ShopManagement.Domain
(productAgg)

3- in ShopManagement.Application.Contracts create Product folder
the application contract in fact is a place to define application interfaces. that is(that's) mean these classes are DTO
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

* create Products/ razor pages index in ServiceHost
* create: 
  * ServiceHost.Areas.Administration.Pages.Shop.Products.IndexModel.ProductCategories
  * ShopManagement.Domain.ProductCategoryAgg.IProductCategoryRepository.GetProductCategories
  * ShopManagement.Infrastructure.EFCore.Repository.ProductCategoryRepository.GetProductCategories
  * ShopManagement.Application.Contracts.ProductCategory.IProductCategoryApplication.GetProductCategories
  * ShopManagement.Application.ProductCategoryApplication.GetProductCategories
  * ServiceHost.Areas.Administration.Pages.Shop.Products.IndexModel.ProductCategories on OnGet method
  * create ProductAdded migration
  
* fix Edit.cshtml, Create.cshtml, ServiceHost.Areas.Administration.Pages.Shop.Products.IndexModel.OnGetEdit
----------------------------------------------------------------
### ProductPicture

----------------------------------------------------------------
### Slide
ما برای اینکه بتونیم اسلایدهارو بیاریم یک کلاس لایبرری جدا درست میکنیم. و دلیل اینکه مثل همیشه نمیریم تو اپلیکیشن اینه که ما میخوایم مستقیم کانتکس اینجکت کنیم و ممکنه بخوایم چنتا کانتکست اینجکت کنیم و نکته دیگه اینکه لاجیکی نداریم صرفا قراره کوئری زده بشه به دیتابیس و دیتا بیاد 
* create 01_LampshadeQuery Library class
* _01_LampshadeQuery.Contracts.Slide.SlideQueryModel
* create _01_LampshadeQuery.Contracts.Slide.ISlideQuery
* create _01_LampshadeQuery.Query.SlideQuery
* create ServiceHost/Pages/Shared/Components/Slide/Default.cshtml
* create ServiceHost.ViewComponents.SlideViewComponent
* wire up in ShopManagementBootstrapper

----------------------------------------------------------------
## Discount Management

1- create DiscountManagement.Domain class library
2- create DiscountManagement.Domain.CustomerDiscountAgg.CustomerDiscount (CustomerDiscount model, ICustomerDiscountRepository). create ctor and Edit methods for CustomerDiscount Domain.
3- create DiscountManagement.Application.Contract class library
4 - create DTOs in DiscountManagement.Application.Contract.CustomerDiscount (DefineCustomerDiscount, EditCustomerDiscount, CustomerDiscountViewModel, CustomerDiscountSearchModel, ICustomerDiscountApplication)
5- create DiscountManagement.Application (CustomerDiscountApplication : ICustomerDiscountApplication)
6- DiscountManagement.Infrastructure.EFCore (DiscountContext : DbContext). define DbSet<CustomerDiscount>
7- create DiscountManagement.Infrastructure.EFCore.Mapping.CustomerDiscountMapping
8- create DiscountManagement.Infrastructure.EFCore.Repository.CustomerDiscountRepository
9- implement DiscountContext
10- implement CustomDiscountRepository
in CustomDiscountRepository we need the product name for each discount. for do this we need inject the ShopContext, get products and loop on them and match with discount ProductId
11- implement DiscountManagement.Application.CustomerDiscountApplication
12- inject ServiceHost/Program.cs#L10-L10 DiscountManagementBootstrapper.Configure(builder.Services, connectionString);
13- create migration DiscountManagement.Infrastructure.EFCore.Migrations