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