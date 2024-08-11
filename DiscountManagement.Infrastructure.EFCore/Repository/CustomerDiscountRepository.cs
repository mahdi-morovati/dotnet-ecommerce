using System.Linq.Expressions;
using DiscountManagement.Application.Contract.CustomerDiscount;
using DiscountManagement.Domain.CustomerDiscountAgg;

namespace DiscountManagement.Infrastructure.EFCore.Repository;

public class CustomerDiscountRepository : ICustomerDiscountRepository
{
    private readonly DiscountContext _context;

    public CustomerDiscountRepository(DiscountContext context)
    {
        _context = context;
    }

    public CustomerDiscount Get(long id)
    {
        throw new NotImplementedException();
    }

    public List<CustomerDiscount> Get()
    {
        throw new NotImplementedException();
    }

    public void Create(CustomerDiscount entity)
    {
        throw new NotImplementedException();
    }

    public void SaveChanges()
    {
        throw new NotImplementedException();
    }

    public bool Exists(Expression<Func<CustomerDiscount, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel)
    {
        throw new NotImplementedException();
    }

    public EditCustomerDiscount GetDetails(long id)
    {
        throw new NotImplementedException();
    }
}