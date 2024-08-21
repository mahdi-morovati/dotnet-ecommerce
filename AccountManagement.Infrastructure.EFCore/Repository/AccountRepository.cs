using System.Linq.Expressions;
using _0_framework.Infrastructure;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Domain.AccountAgg;

namespace AccountManagement.Infrastructure.EFCore.Repository;

public class AccountRepository : RepositoryBase<long, Account>, IAccountRepository
{
    private readonly AccountContext _context;

    public AccountRepository(AccountContext context) : base(context)
    {
        _context = context;
    }

    public Account Get(long id)
    {
        return _context.Accounts.FirstOrDefault(a => a.Id == id);
    }

    public List<Account> Get()
    {
        throw new NotImplementedException();
    }

    public void Create(Account entity)
    {
        throw new NotImplementedException();
    }

    public void SaveChanges()
    {
        throw new NotImplementedException();
    }

    public bool Exists(Expression<Func<Account, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public Account GetBy(string username)
    {
        throw new NotImplementedException();
    }

    public EditAccount GetDetails(long id)
    {
        return _context.Accounts.Select(x => new EditAccount
        {
            Id = x.Id,
            Fullname = x.Fullname,
            Mobile = x.Mobile,
            RoleId = x.RoleId,
            Username = x.Username,
        }).FirstOrDefault(x => x.Id == id);
    }

    public List<AccountViewModel> GetAccounts()
    {
        throw new NotImplementedException();
    }

    public List<AccountViewModel> Search(AccountSearchModel searchModel)
    {
        var query = _context.Accounts.Select(x => new AccountViewModel
        {
            Id = x.Id,
            Fullname = x.Fullname,
            Mobile = x.Mobile,
            Role = "system administrator",
            RoleId = 2,
            Username = x.Username,
        });

        if (!string.IsNullOrWhiteSpace(searchModel.Fullname))
            query = query.Where(x => x.Fullname.Contains(searchModel.Fullname);

        if (!string.IsNullOrWhiteSpace(searchModel.Mobile))
            query = query.Where(x => x.Mobile.Contains(searchModel.Mobile);

        if (searchModel.RoleId > 0)
            query = query.Where(x => x.RoleId == searchModel.RoleId);

        return query.OrderByDescending(x => x.Id).ToList();
    }
}