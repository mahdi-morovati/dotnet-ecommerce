using _0_framework.Domain;
using DiscountManagement.Application.Contract.CustomerDiscount;

namespace DiscountManagement.Domain.CustomerDiscountAgg;

public interface ICustomerDiscountRepository : IRepository<long, CustomerDiscount>
{
    List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel);
    EditCustomerDiscount GetDetails(long id);
}