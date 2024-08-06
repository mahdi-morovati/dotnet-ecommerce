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
