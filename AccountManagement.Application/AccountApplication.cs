using _0_framework.Application;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Domain.AccountAgg;

namespace AccountManagement.Application;

public class AccountApplication : IAccountApplication
{
    private readonly IFileUploader _fileUploader;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAccountRepository _accountRepository;

    public AccountApplication(IAccountRepository accountRepository, IPasswordHasher passwordHasher,
        IFileUploader fileUploader)
    {
        _accountRepository = accountRepository;
        _passwordHasher = passwordHasher;
        _fileUploader = fileUploader;
    }

    public AccountViewModel GetAccountBy(long id)
    {
        throw new NotImplementedException();
    }

    public OperationResult Register(RegisterAccount command)
    {
        var operation = new OperationResult();
        if (_accountRepository.Exists(x => x.Username == command.Username || x.Mobile == command.Mobile))
            return operation.Failed(ApplicationMessages.DuplicatedRecord);

        var password = _passwordHasher.Hash(command.Password);
        var path = "profilePhotos";
        var picturePath = _fileUploader.Upload(command.ProfilePhoto, path);
        var account = new Account(command.Fullname, command.Username, password, command.Mobile, command.RoleId,
            picturePath);

        _accountRepository.Create(account);
        _accountRepository.SaveChanges();
        return operation.Succedded();
    }

    public OperationResult Edit(EditAccount command)
    {
        var operation = new OperationResult();
        var account = _accountRepository.Get(command.Id);
        if (account == null)
            return operation.Failed(ApplicationMessages.RecordNotFound);
        if (_accountRepository.Exists(x =>
                x.Username == command.Username || x.Mobile == command.Mobile && x.Id != command.Id))
            return operation.Failed(ApplicationMessages.DuplicatedRecord);

        var path = "profilePhotos";
        var picturePath = _fileUploader.Upload(command.ProfilePhoto, path);
        account.Edit(command.Fullname, command.Username, command.Mobile, command.RoleId, picturePath);
        _accountRepository.SaveChanges();
        return operation.Succedded();
    }

    public OperationResult ChangePassword(ChangePassword command)
    {
        var operationResult = new OperationResult();
        var account = _accountRepository.Get(command.Id);
        if (account == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);
        if (command.Password != command.RePassword)
            return operationResult.Failed(ApplicationMessages.PasswordsNotMatch);

        var password = _passwordHasher.Hash(command.Password);
        account.ChangePassword(password);
        _accountRepository.SaveChanges();
        return operationResult.Succedded();
    }

    public OperationResult Login(Login command)
    {
        throw new NotImplementedException();
    }

    public EditAccount GetDetails(long id)
    {
        return _accountRepository.GetDetails(id);
    }

    public List<AccountViewModel> Search(AccountSearchModel searchModel)
    {
        return _accountRepository.Search(searchModel);
    }

    public void Logout()
    {
        throw new NotImplementedException();
    }

    public List<AccountViewModel> GetAccounts()
    {
        throw new NotImplementedException();
    }
}