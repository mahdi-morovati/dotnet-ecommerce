using _0_framework.Infrastructure;
using DiscountManagement.Application.Contract.ColleagueDiscount;
using DiscountManagement.Domain.ColleagueDiscountAgg;
using Microsoft.EntityFrameworkCore;

namespace DiscountManagement.Infrastructure.EFCore.Repository;

public class ColleagueDiscountRepository : RepositoryBase<long, ColleagueDiscount>, IColleagueDiscountRepository
{
    private readonly DiscountContext _context;

    public ColleagueDiscountRepository(DiscountContext context) : base(context)
    {
        _context = context;
    }

    public EditColleagueDiscount GetDetails(long id)
    {
        return _context.ColleagueDiscounts.Select(x => new EditColleagueDiscount
        {
            Id = x.Id,
            DiscountRate = x.DiscountRate,
            ProductId = x.ProductId,
        }).FirstOrDefault(x => x.Id == id);
    }

    public List<ColleagueDiscountViewModel> Search(ColleagueDiscountSearchModel searchModel)
    {
        throw new NotImplementedException();
    }
}