using _0_framework.Application;

namespace DiscountManagement.Application.Contract.CustomerDiscount;

public interface ICustomerDiscountApplication
{
    OperationResult Define(DefineCustomerDiscount command);
    OperationResult Edit(EditCustomerDiscount command);
    List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel);
    EditCustomerDiscount GetDetails(long id);
}